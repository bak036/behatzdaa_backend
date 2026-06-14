using CreditServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Nofshonit.BL.Exceptions;
using Nofshonit.BL.User;
using Nofshonit.BL.Utils;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Payments;
using Nofshonit.Common.EF.DTS_Online;
//*3using Nofshonit.Common.Http_Procedures;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Common.Utils;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Logs;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsLogsModel;
using Nofshonit.Repositories.DtsOnlineModel;
using OnlineDataService;
using PaymentAPICommon.APIRequest;
using PaymentsAPI.Models;
using PurchaseHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using static CreditServices.Interfaces.ResponseMandatoryProperties;

namespace Nofshonit.BL.Cards
{
    public class CardsBL : BaseBL, ICardsBL
    {
        private Dictionary<int, int> _mappingErrorMessage = JsonConvert.DeserializeObject<Dictionary<int, int>>(ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.HttpClientMessageKeys));
        private readonly IClubRepo _clubRepo;
        private readonly IConfigurationManager _configuration;
        private readonly IDtsLogsRepo _dtsLogsRepo;
        private readonly IDtsOnlineRepo _dtsOnlineRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigBL _configRepo;
        private readonly ICreditGuard _creditGuard;
        private readonly IUserService _userService;
        private string fighterBins;

        //public readonly ICardsService _service;
        MoveBalanceData MBdata2 = new MoveBalanceData();
        private string currentUserTz;// = "032805707"; // "040193286"
        private const string ORGANIZATION_SERIES_NAME = "בהצדעה";//TODO add to configuration file or enum
        private const string MoneyMaxCommissionAmount = "Money_MaxCommissionAmount";
        private const string MoneyNumberOfDaysAllowingCancellat = "Money_NumberOfDaysAllowingCancellat";
        private const string MoneyPercentageCancellationCommission = "Money_PercentageCancellationCommission";

        public CardsBL() : base()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _configuration = Container.Resolve<IConfigurationManager>();
            _dtsLogsRepo = Container.Resolve<IDtsLogsRepo>();
            _configRepo = Container.Resolve<IConfigBL>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _httpContextAccessor = Container.Resolve<IHttpContextAccessor>();
            _creditGuard = Container.Resolve<ICreditGuard>();
            _userService = Container.Resolve<IUserService>();
            fighterBins = _configuration.GetConfigByValue<string>(ConfigurationKey.FighterBins);

            //_service = Container.Resolve<ICardsService>();
            // currentUserTz = ContextManager.CurrentUser().IdentityNumber;
        }
        /// <summary>
        /// Block card in DB and Verifone service add log to VerifonTransactionLog and to Requests tables
        /// </summary>
        /// <returns></returns>
        public async Task<string> BlockCard()
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            string cardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);
            bool status = _clubRepo.BlockCard(currentUser.Id);
            if (!status)
                throw new BusinessException("Error");

            long trasctionId = await _clubRepo.AddVerifoneLog(0, cardNumber, 0, "Block");
            Card_DTS card_DTS = new Card_DTS
            {
                CardNumber = cardNumber,
                Cvv = _clubRepo.GetCardCVV(cardNumber),
                OriginalRequest = trasctionId,
                TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
            };

            OnLineDataClient dataClient = null;
            try
            {
                dataClient = new OnLineDataClient();
                SingelBlockCardResponse response = await dataClient.SingelBlockCardAsync(new SingelBlockCardRequest(card_DTS));
                if (response.SingelBlockCardResult.ResponseStatus_DTS.Equals("Error") || response.SingelBlockCardResult.ResponseStatus_DTS.Equals("Unavailable"))
                {
                    response = await dataClient.SingelBlockCardAsync(new SingelBlockCardRequest(card_DTS));
                    if (response.SingelBlockCardResult.ResponseStatus_DTS.Equals("Error") || response.SingelBlockCardResult.ResponseStatus_DTS.Equals("Unavailable"))
                    {
                        _clubRepo.AddRequestRow(1, cardNumber, null, response.SingelBlockCardResult.ResponseStatus_DTS.ToString(), 31, 0);
                        throw new BusinessException(response.SingelBlockCardResult.ResponseStatus_DTS.ToString());
                    }
                }

                if (response.SingelBlockCardResult.ResponseStatus_DTS.Equals("Succeeded"))
                    _clubRepo.AddRequestRow(1, cardNumber, null, "", 31, 0);

                return response.SingelBlockCardResult.ResponseStatus_DTS.ToString();
            }
            finally
            {
                if (dataClient != null)
                    await dataClient.CloseAsync();
            }
        }

        public bool CancelLoadingWallet()
        {
            throw new NotImplementedException();
        }

        private async Task<LoadWalletResponseDTO> LoadWalletFunc(ExtandPeyerDataDTO epd)
        {
            DischargeData dischargeObj = new DischargeData();
            LoadWalletToFuncDto loadWalletToFuncDto = new LoadWalletToFuncDto();
            ChargeAmountInCreditCardDTO chargeAmountInCreditCardDTO = null;
            long reqId = 0;
            try
            {
                LoadWalletResponseDTO loadingResults = new LoadWalletResponseDTO();
                loadingResults.rows = new LoadWalletRow[] { new LoadWalletRow { ErrorID = "0" } };

                ExtandPeyerDataValidate(epd);

                loadWalletToFuncDto = _clubRepo.LoadWallet(epd.CardNumber, epd.PayerData.WalletID);

                LoadWalletFromDBValidate(loadWalletToFuncDto, epd);

                double totalChargedCurrentMonth = double.Parse(loadWalletToFuncDto.LoadedThisMonth);

                double Amount = 0;

                if (loadWalletToFuncDto.IsLeverage) Amount = epd.PayerData.AmountToCharge;
                else Amount = epd.PayerData.AmountToLoad;

                // calcaulte amount to charge if not set, and if set make sure it's near the calculated value
                double AmountToCharge = epd.PayerData.AmountToCharge;
                double CalculatedAmountToCharge = 0;

                CalculatedAmountToCharge = Amount;
                if (!loadWalletToFuncDto.IsLeverage) CalculatedAmountToCharge = CalculatedAmountToCharge * (1 - loadWalletToFuncDto.DiscountRate / 100);
                // if (AmountToCharge == null) AmountToCharge = CalculatedAmountToCharge;
                if (AmountToCharge < CalculatedAmountToCharge * 0.95 || AmountToCharge > CalculatedAmountToCharge * 1.05)
                    throw new BusinessException(GetMessageText(10267));//???

                double AmountToLoad = epd.PayerData.AmountToLoad;

                if ((totalChargedCurrentMonth + AmountToLoad) > loadWalletToFuncDto.MaxInMonth)
                    throw new BusinessException(GetMessageText(10232));//???

                reqId = _clubRepo.AddRequestRow(3, epd.CardNumber, null, null, 5, 0, (decimal)Amount, int.Parse(epd.PayerData.WalletID));
                chargeAmountInCreditCardDTO = CreditGuardPaymentProcess(epd, loadWalletToFuncDto, AmountToCharge, reqId);

                //Load wallet process

                #region LoadWallet
                bool unknownResult = false;
                try
                {
                    dischargeObj.responseDeposit = await dischargeObj.client.SingelDepositAsync(new VPayServiceWS.SingelDepositRequest()
                    {
                        request = new VPayServiceWS.SingelChargeDischargeRequest_DTS()
                        {
                            chargeDischarge = new VPayServiceWS.ChargeDischarge() { Amount = (decimal)AmountToLoad, WalletID = int.Parse(epd.PayerData.WalletID) },
                            CardNumber = epd.CardNumber,
                            Cvv = short.Parse(loadWalletToFuncDto.Cvv),
                            OrganizationID = int.Parse(loadWalletToFuncDto.OrganizationID),
                            TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId),
                            OriginalRequest = reqId
                        }
                    });
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error(ex, "Failed SingelDepositAsync - " + ex.Message + " --- " + "Member" + ContextManager.CurrentUser().Id + ", CardNumber:" + epd.CardNumber);
                    //במקרה בו אנחנו לא יודדעים מה קרה בווריפון עם הטעינה אנו נמשיך את התהליך כאילו הצליח על מנת שתצא חשבונית
                    unknownResult = true;
                }
                if (unknownResult || dischargeObj.responseDeposit.SingelDepositResult.ResponseStatus_DTS == VPayServiceWS.ReponseStatuses_DTS.Succeeded)
                {
                    try
                    {
                        _clubRepo.UpdateRequestLoadWallet(reqId, chargeAmountInCreditCardDTO.PaymentId, 1, "");
                        loadingResults.rows[0].RequestID = reqId;
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Error(ex, "Failed to update request - " + ex.Message + ", CardNumber:" + epd.CardNumber);
                    }
                }

                else//Failure
                {
                    dischargeObj.cardNumber = epd.PayerData.PayerCardNumber;
                    dischargeObj.amountToRefound = (decimal)epd.PayerData.AmountToCharge;

                    //מבצע זיכוי מול קרדיט קארד
                    ApiClient cg = new ApiClient();
                    try
                    {
                        var t = cg.Refund(new RefundRequest()
                        {
                            GUID = "0",
                            TerminalNumber = loadWalletToFuncDto.LoadMoneyTerminalNumber,
                            TotalPrice = AmountToCharge,
                            AuthNumber = epd.PayerData.PayerCardNumber,
                            MemberID = epd.PayerData.PayerCardTZ.Trim(),
                            OrganizationID = ContextManager.CurrentOrganization().OrgId,
                            TransactionID = chargeAmountInCreditCardDTO.CreditCardTransactionId
                        });
                        long paymentId = 0;

                        try
                        {
                            AmountToCharge *= -1;
                            string Last4Digits = epd.PayerData.PayerCardNumber.Substring(epd.PayerData.PayerCardNumber.Length - 4, 4);
                            XDocument doc = _creditGuard.GetXmlPaymentInformation(loadWalletToFuncDto.DBName, t.ServerTransactionID, t.ServerData.AuthNumber, loadWalletToFuncDto.LoadMoneyTerminalNumber, 1, loadWalletToFuncDto.CardID);
                            paymentId = _clubRepo.InsertPayment(loadWalletToFuncDto.IDMember, (decimal)AmountToCharge, doc.ToString(), epd.PayerData.PayerCardTZ, Last4Digits, t.ServerTransactionID);
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Error(ex, "Failed insert to Payment  - " + ex.Message + " --- " + ex.InnerException + ", CardNumber:" + epd.CardNumber);
                        }
                        //update request
                        try
                        {
                            string msg = $"Vpay Service => {dischargeObj?.responseDeposit?.SingelDepositResult?.ResponseStatus_DTS.ToString()} - {dischargeObj?.responseDeposit?.SingelDepositResult?.ErroreMessage}";
                            _clubRepo.UpdateRequestLoadWallet(reqId, paymentId, 2, msg);
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Error(ex, "Failed to update request by Catch Verifone load - " + dischargeObj.responseDischarge.SingelPartialDischargeResult.ErroreMessage + ", CardNumber:" + epd.CardNumber);
                        }

                        loadingResults.rows[0].ErrorID = "1";
                        throwException(dischargeObj.responseDeposit.SingelDepositResult.ErroreCode, dischargeObj.responseDeposit.SingelDepositResult.ErroreMessage);

                    }
                    catch (Exception ex)
                    {
                        loadingResults.rows[0].ErrorID = "1";
                        LoggerHelper.Error(ex, "Failed to update request - " + ex.Message + " --- " + ex.InnerException + ", CardNumber:" + epd.CardNumber);
                    }
                    #endregion
                }
                return loadingResults;
            }
            catch (Exception ex)
            {
                RollbackWhenFailed(epd, dischargeObj, chargeAmountInCreditCardDTO, loadWalletToFuncDto, reqId, ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private void RollbackWhenFailed(ExtandPeyerDataDTO epd, DischargeData dischargeObj, ChargeAmountInCreditCardDTO chargeAmountInCreditCardDTO, LoadWalletToFuncDto loadWalletToFuncDto, long reqId, string message)
        {
            double AmountToCharge = epd.PayerData.AmountToCharge;
            dischargeObj.cardNumber = epd.PayerData.PayerCardNumber;
            dischargeObj.amountToRefound = (decimal)epd.PayerData.AmountToCharge;

            //מבצע זיכוי מול קרדיט קארד
            ApiClient cg = new ApiClient();
            try
            {
                TransactionResults t = null;
                if (chargeAmountInCreditCardDTO != null)
                {
                    t = cg.Refund(new RefundRequest()
                    {
                        GUID = "0",
                        TerminalNumber = loadWalletToFuncDto.LoadMoneyTerminalNumber,
                        TotalPrice = AmountToCharge,
                        AuthNumber = epd.PayerData.PayerCardNumber,
                        MemberID = epd.PayerData.PayerCardTZ.Trim(),
                        OrganizationID = ContextManager.CurrentOrganization().OrgId,
                        TransactionID = chargeAmountInCreditCardDTO.CreditCardTransactionId
                    });

                    try
                    {
                        AmountToCharge *= -1;
                        string Last4Digits = epd.PayerData.PayerCardNumber.Substring(epd.PayerData.PayerCardNumber.Length - 4, 4);
                        XDocument doc = _creditGuard.GetXmlPaymentInformation(loadWalletToFuncDto.DBName, t.ServerTransactionID, t.ServerData.AuthNumber, loadWalletToFuncDto.LoadMoneyTerminalNumber, 1, loadWalletToFuncDto.CardID);
                        long paymentId = _clubRepo.InsertPayment(loadWalletToFuncDto.IDMember, (decimal)AmountToCharge, doc.ToString(), epd.PayerData.PayerCardTZ, Last4Digits, t.ServerTransactionID);
                    }
                    catch (Exception ex1)
                    {
                        LoggerHelper.Error(ex1, "Failed insert to Payment  - " + ex1.Message + " --- " + ex1.InnerException + ", CardNumber:" + epd.CardNumber);
                    }
                }

                //update request
                try
                {
                    _clubRepo.UpdateRequestLoadWallet(reqId, t != null ? int.Parse(t.ServerTransactionID) : 0, 2, message);
                }
                catch (Exception ex3)
                {
                    LoggerHelper.Error(ex3, "Failed to update request by Catch Verifone load - " + ex3.Message + ", CardNumber:" + epd.CardNumber);
                }
            }
            catch (Exception ex4)
            {
                LoggerHelper.Error(ex4, "Failed to refound");
            }
        }

        private ChargeAmountInCreditCardDTO CreditGuardPaymentProcess(ExtandPeyerDataDTO epd, LoadWalletToFuncDto loadWalletToFuncDto, double AmountToCharge, long reqId)
        {
            long id = 0;
            string r = "";//????
            int PayerCardExpiresMonth = 0;
            int PayerCardExpiresYear = 0;
            string CardID = "";
            string Last4Digits = "";

            if (!string.IsNullOrEmpty(epd.PayerData.PayerCardNumber))
            {
                Last4Digits = epd.PayerData.PayerCardNumber.Substring(epd.PayerData.PayerCardNumber.Length - 4, 4);
            }
            else
            {
                if (loadWalletToFuncDto.CardID.Length > 0 && loadWalletToFuncDto.CardID != "0")
                {
                    r = "0" + loadWalletToFuncDto.CardExpiration.ToString();
                    r = r.Substring(r.Length - 4);
                    PayerCardExpiresMonth = Convert.ToInt16(r.Substring(0, 2));
                    PayerCardExpiresYear = Convert.ToInt16(r.Substring(2));
                    CardID = loadWalletToFuncDto.CardID.ToString().Trim();
                    Last4Digits = loadWalletToFuncDto.CardNum.ToString();
                    epd.PayerData.PayerCardTZ = string.IsNullOrEmpty(epd.PayerData.PayerCardNumber)
                                                ? loadWalletToFuncDto.PayerTZ.Trim()
                                                : epd.PayerData.PayerCardTZ;
                }
            }


            TransactionResults a = new TransactionResults(); //???
            string PayerCardExpires = (PayerCardExpiresMonth < 10 ? "0" : "") + PayerCardExpiresMonth.ToString() + (PayerCardExpiresYear % 100).ToString();

            if (epd.UseQuickLoad)
            {
                // quick load - use card id
                var cgToken = new CardTokenPaymentModelView();
                cgToken.Expiration = PayerCardExpires;
                cgToken.CardToken = loadWalletToFuncDto.CardID.Trim();
                cgToken.TerminalNumber = loadWalletToFuncDto.LoadMoneyTerminalNumber;
                cgToken.TotalPrice = AmountToCharge;
                cgToken.UserPersonalID = epd.PayerData.PayerCardTZ.Trim();
                cgToken.CVV = epd.PayerData.PayerCardCVV.ToString();
                a = _creditGuard.PaymentToken(cgToken);
            }
            else
            {
                a = _creditGuard.Payment(new CardNumberPaymentModelView()
                {
                    TerminalNumber = loadWalletToFuncDto.LoadMoneyTerminalNumber,
                    Expiration = epd.PayerData.PayerCardExpiresMonth + epd.PayerData.PayerCardExpiresYear,
                    CVV = epd.PayerData.PayerCardCVV.ToString(),
                    CardNumber = epd.PayerData.PayerCardNumber.Trim(),
                    UserPersonalID = epd.PayerData.PayerCardTZ.Trim(),
                    TotalPrice = AmountToCharge,
                    NumberOfPayments = 1
                });

            }

            if (a.ServerResponseCode != "000")
            {
                _clubRepo.UpdateRequestLoadWallet(reqId, null, 2, a.Code.ToString() + " " + a.Message);
                string clientMessage = MessagesUtil.GetCreditGurdfriendlyErrorMessage(a.ServerResponseCode);
                throwException(int.Parse(a.ServerResponseCode), clientMessage);
            }

            else//success
            {
                XDocument doc = _creditGuard.GetXmlPaymentInformation(loadWalletToFuncDto.DBName, a.ServerTransactionID, a.ServerData.AuthNumber, loadWalletToFuncDto.LoadMoneyTerminalNumber, 1, loadWalletToFuncDto.CardID);
                try
                {
                    id = _clubRepo.InsertPayment(loadWalletToFuncDto.IDMember, (decimal)AmountToCharge, doc.ToString(), epd.PayerData.PayerCardTZ, Last4Digits, a.ServerTransactionID);
                    _clubRepo.UpdateRequestLoadWallet(reqId, id, 3, "");
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error(ex, "Failed insert to Payment  - " + ex.Message + " --- " + ex.InnerException + ", CardNumber:" + epd.CardNumber);
                }
                try
                {
                    if ((!epd.UseQuickLoad) && epd.PayerData.SaveForQuickLoad)
                        _clubRepo.UpdateOrAddToAllMembersProperties(epd, a.ServerData.CardToken, loadWalletToFuncDto.IDMember);
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error(ex, "Failed to save quick load - " + ex.Message + " --- " + ex.InnerException + ", CardNumber:" + epd.CardNumber);
                }

            }

            return new ChargeAmountInCreditCardDTO()
            {
                PaymentId = id,
                CreditCardTransactionId = a.ServerTransactionID
            };
        }

        private void LoadWalletFromDBValidate(LoadWalletToFuncDto loadWalletToFuncDto, ExtandPeyerDataDTO epd)
        {
            if (loadWalletToFuncDto.WalletLoadMoneyId.Length == 0)
                throw new BusinessException(GetMessageText(10266));
            if (loadWalletToFuncDto.CardsPrefix.Length == 0)
                throw new BusinessException(GetMessageText(10273));
            if (loadWalletToFuncDto.Cvv.Length == 0)
                throw new BusinessException(GetMessageText(10273));
            if (loadWalletToFuncDto.IDMember.Length == 0)
                throw new BusinessException(GetMessageText(10273));
            /*if (loadWalletToFuncDto.CardID.Length == 0 && (!epd.UseQuickLoad && epd.PayerData.SaveForQuickLoad))
                throw new BusinessException(GetMessageText(10273));*/
            if (loadWalletToFuncDto.CardID.Length == 0 && epd.UseQuickLoad)
                throw new BusinessException(GetMessageText(10273));
        }

        private void ExtandPeyerDataValidate(ExtandPeyerDataDTO epd)
        {
            if (epd.PayerData.WalletID == null || epd.PayerData.WalletID.Length == 0 || epd.PayerData.WalletID == "0")
                throw new BusinessException(GetMessageText(10232));

            if (epd.PayerData.AmountToLoad == 0)
                throw new BusinessException(GetMessageText(10264));

            if (epd.PayerData.AmountToCharge == 0)
                throw new BusinessException(GetMessageText(10270));

            if (!epd.UseQuickLoad && epd.PayerData.PayerCardTZ == null)
                throw new BusinessException(GetMessageText(10015));

            if (!epd.UseQuickLoad && epd.PayerData.PayerCardNumber == null)
                throw new BusinessException(GetMessageText(10015));

            if (!epd.UseQuickLoad && epd.PayerData.PayerCardExpiresMonth == null)
                throw new BusinessException(GetMessageText(10017));

            if (!epd.UseQuickLoad && epd.PayerData.PayerCardExpiresYear == null)
                throw new BusinessException(GetMessageText(10017));
        }

        private List<WalletBusinessesData> GetWalletBusinesses(string walletId, string chainId)
        {
            if (chainId == null)
                chainId = "";
            List<WalletBusinessesData> res;
            using (var dtsOnlineContext = new DTS_OnlineContext())
            {
                res = (from Branches in dtsOnlineContext.MwcVpayBranches
                       join mvc in dtsOnlineContext.MwcVpayChains on Branches.ParentChainId equals mvc.ChainId
                       join mvwtc in dtsOnlineContext.MwcVpayWalletsToChains on mvc.ChainId equals mvwtc.ChainId
                       where mvwtc.WalletId == int.Parse(walletId)
                       && (string.IsNullOrEmpty(chainId) || mvwtc.ChainId == int.Parse(chainId))
                       select new WalletBusinessesData
                       {
                           BranchAddress = Branches.Title,
                           BranchName = Branches.Name,
                           ChainName = mvc.Name,
                           ChainID = mvc.ChainId.ToString()
                       }).ToList<WalletBusinessesData>();
            }
            return res;
        }

        private async Task<List<WalletData>> GetCardWallets(string cardNumber, string memberId)
        {

            List<WalletAndBalanceDTO> Wallets = new List<WalletAndBalanceDTO>();
            var cardBalance = await GetCardBalance(new BarCodeDTO() { CardNumber = cardNumber });

            if (cardBalance?.GetCardBalance_DTSResult?.Body is null)
            {
                LoggerHelper.Error($"Error on GetCardWallets, verifone response in null, cardNumber: {cardNumber}" +
                    $" response: {System.Text.Json.JsonSerializer.Serialize(cardBalance)}");
            }

            if (cardBalance.GetCardBalance_DTSResult.Body.Length == 0)
            {
                LoggerHelper.Error($"Error on GetCardWallets, verifone response in zero wallets, cardNumber: {cardNumber}" +
                    $" response: {System.Text.Json.JsonSerializer.Serialize(cardBalance)}");
            }

            foreach (var item in cardBalance.GetCardBalance_DTSResult.Body)
            {
                Wallets.Add(
                    new WalletAndBalanceDTO()
                    {
                        Balance = item.Balance.ToString(),
                        Wallet = item.Id.ToString()
                    });
            }

            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(Wallets.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, Wallets);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }

            List<WalletData> result = _clubRepo.GetWallets(xmlString, cardNumber, memberId);
            if (result is null || result.Count == 0)
            {
                LoggerHelper.Error($"Error on GetCardWallets, sp_GetWalletDataDto response in empty, cardNumber: {cardNumber}");
            }
            return result;
        }

        private async Task<VPayServiceWS.GetCardBalance_DTSResponse> GetCardBalance(BarCodeDTO barCodeDTO)
        {
            try
            {
                VPayServiceWS.VpayServiceClient client = new VPayServiceWS.VpayServiceClient();
                barCodeDTO.CVV = _clubRepo.GetCardCVV(barCodeDTO.CardNumber);

                var cardBalance = await client.GetCardBalance_DTSAsync(new VPayServiceWS.GetCardBalance_DTSRequest()
                {
                    card = new VPayServiceWS.Card_DTS()
                    {
                        Cvv = barCodeDTO.CVV,
                        CardNumber = barCodeDTO.CardNumber,
                        OrganizationID = ContextManager.CurrentOrganization().OrgId,
                        TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                    }
                });

                return cardBalance;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error on GetCardBalance, CardNumber: {barCodeDTO.CardNumber}," +
                    $" Message: {ex.Message}, InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
                throw ex;
            }
        }

        private async Task<List<CardActivitiesData>> GetCardActivitiess(string CardNumber = "", DateTime? StartDate = null, DateTime? EndDate = null)
        {
            if (CardNumber == "test")
            {
                CardNumber = "2010116761413440";
            }

            short cvv = _clubRepo.GetCardCVV(CardNumber);
            OnlineDataService.CardActivitiesReq_DTS VCard = new OnlineDataService.CardActivitiesReq_DTS();
            VCard.CardNumber = CardNumber;
            VCard.Cvv = cvv;

            if (StartDate != null) VCard.DateFrom = StartDate.Value;

            VCard.DateTo = DateTime.Now;

            if (EndDate != null) VCard.DateTo = EndDate.Value;

            var VpayOnlineData = new OnlineDataService.OnLineDataClient();
            OnlineDataService.GetCardActivities_DTSRequest cardActivitiesReq = new GetCardActivities_DTSRequest(VCard);
            var VPayResult = await VpayOnlineData.GetCardActivities_DTSAsync(cardActivitiesReq);

            List<CardActivitiesData> cad = new List<CardActivitiesData>();

            foreach (var Item in VPayResult.GetCardActivities_DTSResult.Body)
            {
                cad.Add(new CardActivitiesData()
                {
                    ActivityTypeName = Item.activityType_DTS,
                    Amount = Item.Amount != 0 ? Item.Amount.ToString() : null,
                    BusinessName = Item.Name,
                    ChainName = Item.ChainName,
                    ActivityID = Item.activityID.ToString(),
                    DateTime = Item.actionDate > DateTime.FromOADate(0) ? Item.actionDate : DateTime.MinValue,
                    InvoiceNumber = Item.InvoiceNumber
                });
            }
            cad.Sort((c1, c2) => DateTime.Compare(c2.DateTime, c1.DateTime));
            return cad;
        }

        public async Task<CardActivitiesDTO> GetCardActivities()
        {
            //Add date filter
            //var res = this.GetWalletBusinessesByChain("2237", "2");
            string cardNumber = _clubRepo.GetActiveCardByIdentity(ContextManager.CurrentUser().Id);//  (ContextManager.CurrentUser().IdentityNumber);
            //CardActivities cardActivities = await HttpRequestManager.HttpClientGet<CardActivities>(GenerateUrl(DtsEcommerceRoutesKeys.GetCardActivities, new List<(string, string)> { ("CardNumber", cardNumber) }));
            var cardActivities = await GetCardActivitiess(cardNumber, null, null);
            List<CardActivitiesData> cad = new List<CardActivitiesData>();
            /*foreach (var item in cardActivities.properties.Data.OrderByDescending(o => o.DateTime))
            {
                item.Amount = item.Amount.Remove(item.Amount.IndexOf('.') + 3);
                cad.Add(item);
            }*/

            foreach (var item in cardActivities)
            {
                item.Amount = item.Amount.Remove(item.Amount.IndexOf('.') + 3);
                cad.Add(item);
            }

            return new CardActivitiesDTO { CardActivitiesList = cad };
        }

        public async Task<CardDTO> GetCardGeneralInfo()
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            string cardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);//(ContextManager.CurrentUser().IdentityNumber);
            CardInfoDTO cid = _clubRepo.GetCardActivationAndExpiryDate(cardNumber);
            List<WalletDTO> walletsDTOs = await GetWalletsData(cardNumber, currentUser);
            double balance = walletsDTOs.Sum(a => a.WalletBalance);
            double moneyPercentageCancellationCommission = GetAppConfigValue<double>(MoneyPercentageCancellationCommission);
            double numberOfDaysAllowingCancellation = GetAppConfigValue<double>(MoneyNumberOfDaysAllowingCancellat);
            double moneyMaxCommissionAmount = GetAppConfigValue<double>(MoneyMaxCommissionAmount);
            return new CardDTO
            {
                Wallets = walletsDTOs,
                CardInfo = cid,
                Balance = balance,
                CardNumber = cardNumber,
                moneyPercentageCancellationCommission = moneyPercentageCancellationCommission,
                numberOfDaysAllowingCancellation = numberOfDaysAllowingCancellation,
                moneyMaxCommissionAmount = moneyMaxCommissionAmount

            };
        }

        //private async Task<string> GetCardNumberByTZ(string tz)
        //{
        //    CardsDTO memberCards = await HttpRequestManager.HttpClientGet<CardsDTO>(GenerateUrl(DtsEcommerceRoutesKeys.GetCards, new List<(string, string)> { ("TZ", tz) }));//conacat params
        //    if (memberCards.properties.Data.Count == 0)
        //        throw new BusinessException(GetMessageText(10219));
        //    return memberCards.properties.Data.FirstOrDefault(c => c.SeriesName.Contains(ORGANIZATION_SERIES_NAME))?.CardNumber ?? throw new BusinessException(GetMessageText(10219)); ;
        //}

        private async Task<List<WalletDTO>> GetWalletsData(string cardNumber, ResponseUserDTO currentUser)
        {

            List<WalletData> cardWallets = await GetCardWallets(cardNumber, currentUser.Id);
            //CardWalletsDTO cardWallets = await HttpRequestManager.HttpClientGet<CardWalletsDTO>(GenerateUrl(DtsEcommerceRoutesKeys.GetCardWallets, new List<(string, string)>
            //{ ("TZ", currentUser.IdentityNumber/*ContextManager.CurrentUser().IdentityNumber*/), ("CardNumber", cardNumber) }));

            List<WalletDTO> walletsDTOs = new List<WalletDTO>();
            foreach (WalletData wallet in /*cardWallets.properties.Data*/ cardWallets)
            {
                bool isLoadMoney;
                bool boolParse = bool.TryParse(wallet.IsLoadMoney, out isLoadMoney);

                var fighterWalletId = _configuration.GetConfigByValue<string>(ConfigurationKey.FighterWalletId);

                int amountToWallet = _clubRepo.GetAmountToWallet(wallet.WalletID);
                LoadingModeDTO loadingMode = new LoadingModeDTO { IsLoadAllowed = boolParse && isLoadMoney ? 1 : 0, Last4Digits = wallet.Last4Digits, QuickLoadMode = wallet.QuickLoadMode };
                WalletDTO walletDto = new WalletDTO
                {
                    DiscountMode = wallet.DiscountMode,
                    DiscountRate = wallet.DiscountRate,
                    LoadingMode = loadingMode,
                    LoadedThisMonth = wallet.LoadedThisMonth,
                    WalletID = wallet.WalletID,
                    WalletName = wallet.Name,
                    WalletBalance = wallet.Balance,
                    AmountToWallet = amountToWallet,
                    MaxAmountToLoad = wallet.MaxAmountToLoad,
                    MaxBalance = wallet.MaxBalance,
                    MaxDeposit = wallet.MaxDeposit,
                    IsLoadMoney = wallet.IsLoadMoney,
                    MaxDepositForMonth = _clubRepo.GetMaxDepositForMonth(wallet.WalletID),
                    BackgroundImageUrl = string.IsNullOrEmpty(wallet.BackgroundImageName) ? "" : ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.walletPictureUrl) + wallet.BackgroundImageName,
                    IsSpecialPaidWallet = wallet.WalletID == fighterWalletId

                };
                walletsDTOs.Add(walletDto);
            }
            ;

            return walletsDTOs;
        }

        public async Task<WalletBusinessesDTO> GetWalletBusinessesByChain(string walletId, string chainId)
        {
            if (chainId == null)
                chainId = string.Empty;
            List<WalletBusinessesData> wb = this.GetWalletBusinesses(walletId, chainId);
            /*WalletBusinesses walletBusinesses = await HttpRequestManager.HttpClientGet<WalletBusinesses>($@"{GenerateUrl(DtsEcommerceRoutesKeys.GetWalletBusinesses, new List<(string, string)> { ("WalletID", walletId), ("ChainID", chainId) })}");
            List<WalletBusinessesData> wb = new List<WalletBusinessesData>();
            foreach (var item in walletBusinesses.properties.Data)
                wb.Add(item);*/


            return new WalletBusinessesDTO { WalletBusinessesList = wb };

        }

        public async Task<List<WalletTagChainsData>> GetWalletChain(string walletId)
        {
            List<WalletTagChainsData> wc = new List<WalletTagChainsData>();
            if (string.IsNullOrEmpty(walletId))
            {
                CardDTO cardInfo = await GetCardGeneralInfo();
                if (cardInfo.Wallets.Count == 0)
                    await GetChainsForWallet(_configuration.GetConfigByValue<string>(ConfigurationKey.DefaultWallet), wc);
                else
                {
                    foreach (var wallet in cardInfo.Wallets)
                        await GetChainsForWallet(wallet.WalletID, wc);
                }
            }
            else
                await GetChainsForWallet(walletId, wc);





            return wc;


        }

        private async Task<List<WalletTagChainsData>> GetChainsForWallet(string walletId, List<WalletTagChainsData> wc)
        {

            foreach (var item in await _clubRepo.GetChainsByWallet(walletId))
            {
                wc.Add(item);
            }
            ;
            return wc;
        }

        public Task<List<WalletChainBranches>> GetWalletChainBranches(string walletId, string chainId)
        {
            return _clubRepo.GetBranchesByWalletAndChain(walletId, chainId);
        }

        public bool ActiveateCard()
        {
            bool status = _clubRepo.ActiveateCard(ContextManager.CurrentUser().Id);
            return status;
        }

        public async Task<WalletDTO> GetWalletInfo(string walletId)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            string cardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);
            List<WalletDTO> walletsDTO = await GetWalletsData(cardNumber, currentUser);
            return walletsDTO.FirstOrDefault(w => w.WalletID.Equals(walletId));
        }

        private bool saveMemberFoeQuikLoad()
        {
            return true;
        }
        private void throwException(int exceptionKey, string ex)
        {
            int messageId;
            string message = ex;

            if (_mappingErrorMessage.TryGetValue(exceptionKey, out messageId))
            {
                var msg = MessagesUtil.GetMessagesByKey(new List<int> { messageId }).FirstOrDefault(m => m.MessageKey == messageId);
                if (msg != null)
                {
                    message = msg.MessageText;
                }
            }

            throw new BusinessException(message);
        }

        public async Task<string> LoadWallet(PayerDataDTO payerData)
        {
            Common.EF.Club.WalletLoadMoney walletLoadMoney = _clubRepo.GetWalletById(payerData.WalletID);
            if (walletLoadMoney.Active == false)
            {
                throw new BusinessException("ארנק לא פעיל");
            }
            Stopwatch stopwatch = new Stopwatch();
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            string MaxClubKey = string.Empty;



            var creditCardState = ECheckCreditCard.WithoutClubCreditCard;

            ValidateFighterWallet(currentUser, payerData, out bool isFighterWallet, out bool isFighterCreditCard);

            if (isFighterWallet && isFighterCreditCard)
            {
                creditCardState = ECheckCreditCard.WithClubCreditCard;
            }
            else
            {
                if (currentUser.ClubCreditCard == 0)
                    throw new BusinessException(GetMessageText(725));

                //TODO ניתן לטעון רק עם כרטיס מקס בהצדעה ששיך ליוזר
                creditCardState = CheckMaxClubCreditCard(currentUser, payerData.PayerCardNumber, payerData.PinCode, payerData.SaveForQuickLoad, out MaxClubKey);
            }

            //TODO להוסיף לטבלתצ הודעות
            if (creditCardState != ECheckCreditCard.WithClubCreditCard || (currentUser.ClubCreditCard <= 0 && !isFighterWallet && !isFighterCreditCard))
                throw new BusinessException(@"כרטיס לא שיך למועדון לא ניתן להמשיך ברכישה");
            if (creditCardState == ECheckCreditCard.WithClubCreditCard)
                if (!string.IsNullOrEmpty(payerData.PinCode) && !string.IsNullOrEmpty(payerData.PayerCardNumber))
                {
                    if (currentUser.Id.Trim() != payerData.PayerCardTZ.Trim())
                        throw new BusinessException(@"לא ניתן לשמור קוד מקוצר לכרטיס אשראי שאינו של חבר המועדון.
                                                ניתן להמשיך בתשלום ללא שמירת קוד מקוצר, או להזין כרטיס אשראי של חבר המועדון.   ");
                }


            if (!string.IsNullOrEmpty(payerData.PayerCardTZ) && !payerData.PayerCardTZ.Equals(currentUser.Id)
                || (!payerData.SaveForQuickLoad && !string.IsNullOrEmpty(payerData.PinCode) && !_clubRepo.IsQuickLoadAllowdToSavedCC()))
                throw new BusinessException(GetMessageText(10015));


            //get money required from client and wallet id
            string cardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);
            // CardWalletsDTO cardWallets = await HttpRequestManager.HttpClientGet<CardWalletsDTO>($@"{ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DTSWalletMVC)}{DtsEcommerceRoutesKeys.GetCardWallets}{GetUserNamePasswordForUrl()}&TZ={currentUser.IdentityNumber}&CardNumber={cardNumber}");

            List<WalletData> wallets = await GetCardWallets(cardNumber, currentUser.Id);
            //List<WalletData> wallets = cardWallets;


            if (wallets.Count == 0)
                throw new BusinessException(GetMessageText(10219));


            WalletData wallet = wallets.FirstOrDefault(w => w.WalletID == payerData.WalletID);
            if (wallet == null)
                throw new BusinessException(GetMessageText(10219));
            string cc = payerData.PayerCardNumber;
            if (wallet?.IsLoadMoney.Equals("false") ?? true)
                throw new BusinessException("Load not allowed");
            if (wallet.IsLoadMoney == "False")
                throw new BusinessException("Load not allowed");

            if (payerData.AmountToLoad > float.Parse(wallet.MaxBalance) - wallet.Balance || payerData.AmountToLoad > wallet.MaxAmountToLoad)
                throw new BusinessException(GetMessageText(10233));
            if (payerData.AmountToLoad + float.Parse(wallet.LoadedThisMonth) > float.Parse(wallet.MaxBalance))
                throw new BusinessException(GetMessageText(10233));

            if (string.IsNullOrEmpty(currentUser.PinCode) && string.IsNullOrEmpty(payerData.PayerCardNumber))
                throw new BusinessException("קוד מקוצר אינו פעיל עבור המשתמש");

            if (!string.IsNullOrEmpty(payerData.PinCode) && string.IsNullOrEmpty(payerData.PayerCardNumber))
            {
                int validationResult = await _userService.ValidatePinCode(currentUser.Id, payerData.PinCode);

                if (validationResult == 1)
                {
                    throw new BusinessException(GetMessageText(10033));
                }
                else if (validationResult == 2)
                {
                    throw new BusinessException(@"חסום");
                }
            }


            if (!string.IsNullOrEmpty(payerData.PinCode) && payerData.SaveForQuickLoad)
            {
                utils.ValidatePinCodeFormat(payerData.PinCode, _configuration.GetConfigByValue<int>(ConfigurationKey.PincodeLength));
                _clubRepo.SavePinCode(payerData.PinCode, currentUser);
            }


            if (payerData.AmountToLoad > wallet?.MaxAmountToLoad)
                throw new BusinessException(string.Format("{0} מקסימום טעינה אפשרית", wallet?.MaxAmountToLoad));

            if (!CheckDiscountCalculation(wallet, payerData))
                throw new BusinessException("חישוב הנחת הטעינה שגוי");

            if (payerData.PayerCardCVV == 0)
            {
                if (!string.IsNullOrEmpty(currentUser.CVV))
                {
                    payerData.PayerCardCVV = int.Parse(currentUser.CVV);
                }
            }

            ExtandPeyerDataDTO epd = new ExtandPeyerDataDTO
            {
                PayerData = payerData,
                CardNumber = cardNumber,
                MaxClubId = MaxClubKey,
                UseQuickLoad = !string.IsNullOrEmpty(payerData.PinCode) && !payerData.SaveForQuickLoad
            };
            try
            {

                string currentCard = epd.CardNumber + "_" + epd.PayerData.WalletID + "_" + epd.PayerData.AmountToCharge;
                if (LoadWalletHelper.DicCardsInLoadProcess.ContainsKey(currentCard))
                {
                    // TODO GIL Message Key
                    LoggerHelper.Error("Error: The Card " + epd.CardNumber + " in WalletID " + epd.CardNumber + " with amount to load " + epd.PayerData.AmountToCharge + " is in process of charging now, Please wait until the end of the process.");
                    throw new BusinessException("הפעולה תתאפשר בעוד מספר שניות, אנא נסה שנית מאוחר יותר.");
                }
                else
                    LoadWalletHelper.DicCardsInLoadProcess.Add(currentCard, DateTime.Now);

                string dicCardsInLoad = epd.CardNumber + "_" + epd.PayerData.WalletID;

                if (LoadWalletHelper.DicCardsInLoadProcessForCancel.ContainsKey(dicCardsInLoad))
                {
                    LoadWalletHelper.DicCardsInLoadProcessForCancel[dicCardsInLoad] = DateTime.Now;
                }
                else
                {
                    LoadWalletHelper.DicCardsInLoadProcessForCancel.Add(dicCardsInLoad, DateTime.Now);
                }
                LoadWalletResponseDTO loadingResults = await LoadWalletFunc(epd);
                stopwatch.Start();
                //LoadWalletResponseDTO loadingResults = await HttpRequestManager.HttpClientGet<LoadWalletResponseDTO>($@"{ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DTSWalletMVC)}{DtsEcommerceRoutesKeys.LoadWallet}{GetUserNamePasswordForUrl()}{epd.ToString()}");


                if (loadingResults.rows[0].ErrorID == "0" && ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.BillingServiceURL) != null)
                {
                    try
                    {
                        string requestId = loadingResults.rows[0].RequestID.ToString();

                        SendInvoiceRequest _request = new SendInvoiceRequest()
                        {
                            WalletName = wallet.Name,
                            TotalSum = payerData.AmountToLoad,
                            CCNUM = !string.IsNullOrEmpty(payerData.PayerCardNumber) ? payerData.PayerCardNumber.Substring(payerData.PayerCardNumber.Length - 4) :
                            _clubRepo.GetCardNumByMemberId(epd.PayerData.PayerCardTZ.Trim()),
                            Discount = payerData.AmountToLoad - payerData.AmountToCharge,
                            Email = !string.IsNullOrEmpty(payerData.Email) ? payerData.Email : currentUser.Email,
                            MemberId = wallet.MemberID,
                            NumOfPayments = 1,
                            OrgId = ContextManager.CurrentOrganization().OrgId,
                            PaymentId = 0,
                            RequestId = requestId
                        };

                        try
                        {
                            _clubRepo.InsertInvoiceManagementRequests(JsonConvert.SerializeObject(_request), ContextManager.CurrentOrganization().OrgId);
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Error(ex, "Error on send invoice request");
                        }

                        SendSmsAfterWalletLoad(payerData, currentUser, wallet);

                    }

                    catch (Exception ex)
                    {
                        LoggerHelper.Error(ex, "CardsBL --> LoadWallet-Send Reciept Error:" + payerData.PayerCardTZ + " - " + "שגיאה בעת שליחת קבלה", ex.Message, ex);
                    }
                }

                return loadingResults.rows[0].ErrorID;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "LoadWallet");

                throw new BusinessException(ex.Message);
            }
            finally
            {
                var SecondsForDeleteDicOfCardNumber = _configuration.GetConfigByValue<string>(ConfigurationKey.PreventDuplicateLoadMoneyInPeriodOfSeconeds);
                DateTime datesForDelete = int.TryParse(SecondsForDeleteDicOfCardNumber, out int sec) ? DateTime.Now.AddSeconds(sec) : DateTime.Now.AddSeconds(-30);
                foreach (var item in LoadWalletHelper.DicCardsInLoadProcess.Where(kvp => kvp.Value < datesForDelete).ToList())
                {
                    LoadWalletHelper.DicCardsInLoadProcess.Remove(item.Key);
                }
                stopwatch.Stop();
                LoggerHelper.Error("Load Wallet Time Is :{0} ms.", stopwatch.ElapsedMilliseconds);
            }

        }

        private void ValidateFighterWallet(ResponseUserDTO currentUser, PayerDataDTO payerData, out bool isFighterWallet, out bool isFighterCreditCard)
        {
            var fighterWalletId = _configuration.GetConfigByValue<string>(ConfigurationKey.FighterWalletId);
            var fighterBinsList = fighterBins
                .Split(';')
                .Select(b => b.Trim())
                .Where(b => !string.IsNullOrEmpty(b))
                .ToList();

            isFighterWallet = payerData.WalletID == fighterWalletId;
            isFighterCreditCard = fighterBinsList.Any(bin => payerData.PayerCardNumber.StartsWith(bin));

            // אם הארנק הוא פייטר
            if (isFighterWallet)
            {
                DtsLoggger.Logger.Info($"ValidateFighterWallet = {currentUser.Id} => Fighter wallet detected ({fighterWalletId})");

                // נשלח PINCODE → שגיאה
                if (!string.IsNullOrEmpty(payerData.PinCode))
                {
                    DtsLoggger.Logger.Error($"ValidateFighterWallet = {currentUser.Id} => PINCODE sent for Fighter wallet - not allowed");
                    throw new BusinessException(GetMessageText(51165));
                }

                // SaveForQuickLoad = true → שגיאה
                if (payerData.SaveForQuickLoad)
                {
                    DtsLoggger.Logger.Error($"ValidateFighterWallet = {currentUser.Id} => SaveForQuickLoad true for Fighter wallet - not allowed");
                    throw new BusinessException(GetMessageText(51165));
                }

                // הכרטיס אינו מהבינים של פייטר → שגיאה
                if (!isFighterCreditCard)
                {
                    DtsLoggger.Logger.Error($"ValidateFighterWallet = {currentUser.Id} => Wallet is Fighter but card is not Fighter BIN");
                    throw new BusinessException(GetMessageText(51165));
                }

                DtsLoggger.Logger.Info($"ValidateFighterWallet = {currentUser.Id} => Fighter wallet + Fighter card verified - allowed");
            }

            // אם הארנק אינו פייטר, אך הכרטיס כן → שגיאה
            if (!isFighterWallet && isFighterCreditCard)
            {
                DtsLoggger.Logger.Error($"ValidateFighterWallet = {currentUser.Id} => Fighter card cannot be used for non-fighter wallet");
                throw new BusinessException("לא ניתן לבצע טעינה לארנק באמצעות כרטיס אשראי זה.");
            }
        }
        private void SendSmsAfterWalletLoad(PayerDataDTO payerData, ResponseUserDTO currentUser, WalletData wallet)
        {
            try
            {
                decimal minAmountToSendSms = _configuration.GetConfigByValue<decimal>(ConfigurationKey.MinAmountToSendSmsAfterLoad);
                if (Convert.ToDecimal(payerData.AmountToLoad) < minAmountToSendSms)
                    return;

                int orgId = ContextManager.CurrentOrganization().OrgId;
                string smsMessage = $"שלום {currentUser.FirstName},\n" +
                    $"לידיעתך – הוטען כסף בתאריך {DateTime.Now:dd/MM/yyyy} על סך {payerData.AmountToLoad} ₪.\n" +
                    $"בכרטיס על שמך של מועדון \"בהצדעה\".\n" +
                    $"במידה ועסקה זו לא מוכרת לך תוכל/י ליצור קשר עם שירות הלקוחות של המועדון.";



                if (!string.IsNullOrEmpty(currentUser.MobilePhone) || !string.IsNullOrEmpty(currentUser.PhoneNumber))
                {
                    var userSms = new SmsQueue
                    {
                        MemberId = wallet.MemberID,
                        OrganizationId = orgId,
                        SenderName = "Behatsdaa",
                        Subscribers = currentUser.MobilePhone ?? currentUser.PhoneNumber,
                        Message = smsMessage,
                        MessageLengh = smsMessage.Length,
                        DeliveryDelayInMinutes = 0,
                        ExpirationDelayInMinutes = 120,
                        SmsType = (byte)orgId,
                        SeveralAttempts = 0,
                        Priority = 0,
                        SmsSend = false
                    };

                    _dtsOnlineRepo.AddContactToSMSQueue(userSms);
                }

                if (!string.IsNullOrEmpty(currentUser.PartnerPhone))
                {
                    var partnerSms = new SmsQueue
                    {
                        MemberId = wallet.MemberID,
                        OrganizationId = orgId,
                        SenderName = "Behatsdaa",
                        Subscribers = currentUser.PartnerPhone,
                        Message = smsMessage,
                        MessageLengh = smsMessage.Length,
                        DeliveryDelayInMinutes = 0,
                        ExpirationDelayInMinutes = 120,
                        SmsType = (byte)orgId,
                        SeveralAttempts = 0,
                        Priority = 0,
                        SmsSend = false
                    };

                    _dtsOnlineRepo.AddContactToSMSQueue(partnerSms);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Error while sending SMS after wallet load");
            }
        }

        public ECheckCreditCard CheckMaxClubCreditCard(ResponseUserDTO currentUser, string PayerCardNumber, string PinCode, bool SaveForQuickLoad, out string ClubKey)
        {
            DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => Strat");

            LeumiCardSvc.CustomerBuyExecuteAnswer leumCardResult = null;
            ClubKey = null;
            bool? skipValidation = Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.SkipMaxCardValidation);

            if (skipValidation.HasValue && skipValidation.Value
                || (string.IsNullOrEmpty(PayerCardNumber))
                || currentUser.ClubCreditCard > 1
                )
            {
                if (string.IsNullOrEmpty(PayerCardNumber))
                    DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => Skip Validation Is On");
                else
                    DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => Skip Validation Card Is Saved");
                return ECheckCreditCard.WithClubCreditCard;

            }
            //List<string> MaxClubId = _configuration.GetConfigByValue<string>("MaxClubId").Split(',').ToList();
            List<string> MaxClubId = _configRepo.GetValueByKey("MaxClubId").Result[0].Value.Split(',').ToList();


            try
            {
                LeumiCardSvc.LCBehatsdaaClient service = new LeumiCardSvc.LCBehatsdaaClient();
                DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => CustomerBuyExecuteAsync Start");
                leumCardResult = service.CustomerBuyExecuteAsync(PayerCardNumber, ContextManager.CurrentUser().Id, ContextManager.CurrentOrganization().OrgId).Result;
                DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => CustomerBuyExecuteAsync End");
                if (leumCardResult != null)
                {

                    if (leumCardResult.RC == 0)
                    {
                        ClubKey = leumCardResult.ClubsList.FirstOrDefault().Key.ToString();
                        DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => SaveForQuickLoad  leumCardResult.ClubKey :{ClubKey}");
                    }
                }
            }
            catch (Exception exp)
            {
                LoggerHelper.Error(exp, "SetMaxClub", exp);
                return ECheckCreditCard.WithoutClubCreditCard;
            }

            DtsLoggger.Logger.Info($"CheckMaxClubCreditCard = {currentUser.Id} => SaveForQuickLoad  leumCardResult.RC  :{leumCardResult.RC}");
            if (leumCardResult.RC != 0 || leumCardResult.ClubsList == null || !checkIfClubListContainsInMaxClubAllow(leumCardResult.ClubsList.Select(x => x.Key).ToList(), MaxClubId))
            {
                return ECheckCreditCard.WithoutClubCreditCard;
            }

            return ECheckCreditCard.WithClubCreditCard;

        }
        private bool checkIfClubListContainsInMaxClubAllow(IEnumerable<int> enumerable, List<string> maxClubId)
        {
            foreach (var item in enumerable)
            {
                if (maxClubId.Contains(item.ToString()))
                    return true;
            }
            return false;
        }

        private string GetMessageText(int messageKey)
        {
            return MessagesUtil.GetMessagesByKey(new List<int> { messageKey }).FirstOrDefault(m => m.MessageKey == messageKey).MessageText;
        }

        public bool GetNewCardOrderPossible()
        {
            var result = _clubRepo.CheckNewCardRequestIsPossible(ContextManager.CurrentUser().Id);

            switch (result)
            {
                case 1:
                    throw new BusinessException(GetMessageText(10237));
                case 2:
                    throw new BusinessException("קיים כרטיס במערכת בתהליך הנפקה ולא ניתן להזמין כרטיס חדש");
                case 3:
                    throw new BusinessException(GetMessageText(10001));
            }

            return true;
        }

        public async Task<BarCodeDTO> GeneratePayCodeAsync()
        {
            VPayService.vPayWebServiceSoapClient vps =
                  new VPayService.vPayWebServiceSoapClient(VPayService.vPayWebServiceSoapClient.EndpointConfiguration.vPayWebServiceSoap);

            ResponseUserDTO currentUser = null;
            BarCodeDTO barCodeDTO = null;

            try
            {
                currentUser = ContextManager.CurrentUser();

                VPayServiceWS.VpayServiceClient client = new VPayServiceWS.VpayServiceClient();
                barCodeDTO = new BarCodeDTO();

                barCodeDTO.CardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);
                barCodeDTO.CVV = _clubRepo.GetCardCVV(barCodeDTO.CardNumber);

                // לוג לפני הקריאה לוריפון – מה אנחנו מבקשים
                LoggerHelper.InfoSeq(
    "GeneratePayCodeAsync - Request: MemberId={0}, Identity={1}, CardNumber={2}",
    currentUser.Id, currentUser.IdentityNumber, barCodeDTO.CardNumber);

                var generateCodeResult = vps.GenerateCodeAsync(
                    new VPayService.GenerateCodeRequest
                    {
                        codeModel = new VPayService.GenerateCodeModel
                        {
                            CardNumber = barCodeDTO.CardNumber,
                            PaymentType = VPayService.PaymentType.RegularPayment
                        }
                    }
                ).Result;

                barCodeDTO.BarCode = generateCodeResult.GenerateCodeResult.Code;

                // לוג אחרי הקריאה לוריפון – מה קיבלנו
                // לוג אחרי הקריאה לוריפון – מה קיבלנו (רק ל‑SEQ)
                LoggerHelper.InfoSeq(
    "GeneratePayCodeAsync - Response: MemberId={0}, Identity={1}, CardNumber={2}, ShortCode={3}",
    currentUser.Id, currentUser.IdentityNumber, barCodeDTO.CardNumber, barCodeDTO.BarCode);

                var cardBalance = await client.GetCardBalance_DTSAsync(new VPayServiceWS.GetCardBalance_DTSRequest()
                {
                    card = new VPayServiceWS.Card_DTS()
                    {
                        Cvv = barCodeDTO.CVV,
                        CardNumber = barCodeDTO.CardNumber,
                        OrganizationID = ContextManager.CurrentOrganization().OrgId,
                        TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                    }
                });

                if (cardBalance.GetCardBalance_DTSResult.Body != null)
                    barCodeDTO.ExpirationDate = FormatVerifoneValidThruDisplay(cardBalance.GetCardBalance_DTSResult.Body[0]?.ValidThru);

                return barCodeDTO;
            }
            catch (Exception e)
            {
                // לוג שגיאה מקושר לת"ז ולכרטיס
                LoggerHelper.Error(
                    e,
                    $"Error on GeneratePayCodeAsync - MemberId={currentUser?.Id}, Identity={currentUser?.IdentityNumber}, CardNumber={barCodeDTO?.CardNumber}"
                );
                throw;
            }
            finally
            {
                await vps.CloseAsync();
            }
        }

        private static string FormatVerifoneValidThruDisplay(string validThru)
        {
            if (string.IsNullOrWhiteSpace(validThru))
                return null;

            // ISO-8601 כולל Z או אופסט — משתמשים ביום הקלנדר ב-UTC (כמו פורטלים רבים)
            if (DateTimeOffset.TryParse(
                    validThru.Trim(),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out var dto))
            {
                var u = dto.UtcDateTime;
                return u.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            // גיבוי: תאריכים בלי אופסט — ניסיון כ-UTC כדי לא לסובב יום על שרת בישראל
            if (DateTime.TryParse(
                    validThru.Trim(),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var dt))
            {
                return dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return DateTime.Parse(validThru, CultureInfo.CurrentCulture).ToString("dd/MM/yyyy");
        }

        private string GenerateUrl(string request, List<(string name, string value)> ps = null)
        {

            string vals = "";
            ps.ForEach(p => vals += $"&{p.name}=" + p.value);
            return $@"{ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DTSWalletMVC)}{request}{GetUserNamePasswordForUrl()}{vals}";

        }

        private string GetUserNamePasswordForUrl()
        {

            string username = _configuration.GetConfigByValue<string>(ConfigurationKey.Username);
            string password = _configuration.GetConfigByValue<string>(ConfigurationKey.Password);
            return $"?LoginName={username}&Password={password}";
        }

        private bool CheckDiscountCalculation(WalletData wallet, PayerDataDTO payerData)
        {
            if (wallet.DiscountMode == "D")
            {
                decimal t = (decimal)payerData.AmountToLoad * (1 - wallet.DiscountRate / 100m);
                if (payerData.AmountToCharge != (float)Math.Round(t, 2))
                    return false;
            }
            else if (wallet.DiscountMode == "L")
            {
                decimal t = (decimal)payerData.AmountToLoad / (1 + wallet.DiscountRate / 100m);
                if (payerData.AmountToCharge != (float)Math.Round(t, 2))
                    return false;
            }
            else
                return false;
            return true;
        }
        public async Task<CardDetailsDTO> GetCardDetailsByCardNumber(string cardNumber)
        {
            VPayService.vPayWebServiceSoapClient vps =
                  new VPayService.vPayWebServiceSoapClient(VPayService.vPayWebServiceSoapClient.EndpointConfiguration.vPayWebServiceSoap);
            CardDetailsDTO cardDetailsDTO = new CardDetailsDTO();
            cardDetailsDTO.Cvv = _clubRepo.GetCardCVV(cardNumber);
            try
            {
                VPayServiceWS.VpayServiceClient client = new VPayServiceWS.VpayServiceClient();

                var cardBalance = await client.GetCardBalance_DTSAsync(new VPayServiceWS.GetCardBalance_DTSRequest()
                {
                    card = new VPayServiceWS.Card_DTS()
                    {
                        Cvv = cardDetailsDTO.Cvv,
                        CardNumber = cardNumber,
                        OrganizationID = ContextManager.CurrentOrganization().OrgId,
                        TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                    }
                });

                if (cardBalance.GetCardBalance_DTSResult.Body != null)
                    cardDetailsDTO.ExpirationDate = FormatVerifoneValidThruDisplay(cardBalance.GetCardBalance_DTSResult.Body[0]?.ValidThru);
                return cardDetailsDTO;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                await vps.CloseAsync();
            }
        }

        public async Task<List<WalletTagChainsData>> GetWalletChainNearMe(string walletId, decimal lat, decimal lon)
        {
            if (string.IsNullOrWhiteSpace(walletId))
                throw new BusinessException("WalletId is required");

            var result = await _clubRepo.GetWalletChainNearMeFromDb(walletId, lat, lon);

            return result;
        }

        public async Task<List<WalletChainBranches>> GetWalletChainBranchesNearMe(string walletId, string chainId, decimal lat, decimal lon)
        {
            if (string.IsNullOrWhiteSpace(walletId))
                throw new BusinessException("WalletId is required");

            var result = await _clubRepo.GetWalletChainBranchesNearMe(walletId, chainId, lat, lon);

            return result;
        }

        #region Move Balance
        private string GenerateRandomString()
        {
            int maxSize = 10;
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();

        }
        private async Task<MoveBalanceData> CreateNewCardAndSlinkAsync(MoveBalanceData MBdata, bool? isPhysical)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            DtsLoggger.Logger.Info($"CreateNewCardAndSlinkAsync: {currentUser.Id}");

            // מביא את הכרטיס האחרון שנחסם
            MBdata.oldCardNumber = _clubRepo.GetOldCardByIdentity(currentUser.Id);

            if (string.IsNullOrEmpty(MBdata.oldCardNumber))
                throw new BusinessException($"No Old Card : {currentUser.Id}");

            // מכניס שורת בקשה להנפקת כרטיס חדש

            int requestStatus = isPhysical.Value ? 28 : 1;

            MBdata.reqHanpakaId = _clubRepo.AddRequestRow(requestStatus, null, null, "", 56, 0, -1);
            DtsLoggger.Logger.Info($"AddRequestRow {currentUser.Id}");
            if (!_clubRepo.AddCardToMember(currentUser.Id, isPhysical.Value))
            {
                _clubRepo.UpdateRequestRow(2, "", "", 0, "נכשל בהרצת AddCardToMember", 0, MBdata.reqHanpakaId);
                MBdata.reqHanpakaIdUpdated = true;
                throw new BusinessException($"Failed Adding New Card To Member: {currentUser.Id}");
            }

            // שאילתא לקבלת מספר הכרטיס החדש
            MBdata.newCardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);
            MBdata.addedCardToMember = true;
            _clubRepo.UpdateRequestRow(1, MBdata.newCardNumber, "", 0, "", 0, MBdata.reqHanpakaId);

            MBdata.reqHanpakaIdUpdated = true;

            // שליחת SLINK 
            MBdata.reqSlinkId = _clubRepo.AddRequestRow(28, MBdata.newCardNumber, "", "", 26, MBdata.reqHanpakaId);

            var generatedLinkCode = GenerateRandomString();
            if (!await _dtsOnlineRepo.SlinkProcess(generatedLinkCode))
            {
                _clubRepo.UpdateRequestRow(2, MBdata.newCardNumber, "", 0, "", MBdata.reqHanpakaId, MBdata.reqSlinkId);
                MBdata.reqSlinkIdUpdated = true;
                throw new BusinessException($"Slink Proccess Failed: {currentUser.Id}");
            }

            _clubRepo.UpdateRequestRow(1, MBdata.newCardNumber, "", 0, "", MBdata.reqHanpakaId, MBdata.reqSlinkId);
            MBdata.reqSlinkIdUpdated = true;

            // מימוש וריאנט הזמנת כרטיס


            return MBdata;
        }
        private async Task<MoveBalanceData> CheckBalanceAndWallets(MoveBalanceData MBdata)
        {
            MBdata.client = new VPayServiceWS.VpayServiceClient();
            MBdata.oldCardInfoVerifone = _clubRepo.GetCardInfoForVerifoneActions(MBdata.oldCardNumber);

            // איקטוב כרטיס ישן לצורך בדיקת יתרות
            MBdata.singelActivateCardResponse = await MBdata.client.SingelUnBlockCardAsync(new VPayServiceWS.SingelUnBlockCardRequest()
            {
                request = new VPayServiceWS.CardSequentialNum_DTS()
                {
                    VerID = (int)MBdata.oldCardInfoVerifone.VerID,
                    SequentialNum = MBdata.oldCardInfoVerifone.SequentialNum,
                    OrganizationID = ContextManager.CurrentOrganization().OrgId,
                    TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                }
            });

            if (MBdata.singelActivateCardResponse.SingelUnBlockCardResult.ResponseStatus_DTS != VPayServiceWS.ReponseStatuses_DTS.Succeeded)
                throw new BusinessException("Can't Activate Old Card : " + MBdata.singelActivateCardResponse.SingelUnBlockCardResult.Error);


            // בודק יתרות על הכרטיס הישן
            MBdata.oldCardBalance = await MBdata.client.GetCardBalance_DTSAsync(new VPayServiceWS.GetCardBalance_DTSRequest()
            {
                card = new VPayServiceWS.Card_DTS()
                {
                    Cvv = MBdata.oldCardInfoVerifone.CVV,
                    CardNumber = MBdata.oldCardNumber,
                    OrganizationID = ContextManager.CurrentOrganization().OrgId,
                    TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                }
            });
            if (MBdata.oldCardBalance.GetCardBalance_DTSResult.ResponseStatus_DTS != VPayServiceWS.ReponseStatuses_DTS.Succeeded)
                throw new BusinessException("Check Old Card Balance Error - " + MBdata.oldCardBalance.GetCardBalance_DTSResult.ErroreMessage);

            MBdata.balance = MBdata.oldCardBalance.GetCardBalance_DTSResult.Body.Sum(x => x.Balance);
            // עצור כאן אם אין יתרה להעביר
            if (MBdata.balance <= 0)
            {
                MBdata.singelBlockCardResponse = await MBdata.client.SingelBlockCardAsync(new VPayServiceWS.SingelBlockCardRequest()
                {
                    request = new VPayServiceWS.CardSequentialNum_DTS
                    {
                        VerID = (int)MBdata.oldCardInfoVerifone.VerID,
                        SequentialNum = MBdata.oldCardInfoVerifone.SequentialNum,
                        OrganizationID = ContextManager.CurrentOrganization().OrgId,
                        TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                    }
                });
                return MBdata;
            }

            // בודק יתרות על הכרטיס החדש לצורך השוואת ארנקים
            MBdata.newCardInfoVerifone = _clubRepo.GetCardInfoForVerifoneActions(MBdata.newCardNumber);
            MBdata.newCardBalance = await MBdata.client.GetCardBalance_DTSAsync(new VPayServiceWS.GetCardBalance_DTSRequest()
            {
                card = new VPayServiceWS.Card_DTS()
                {
                    Cvv = MBdata.newCardInfoVerifone.CVV,
                    CardNumber = MBdata.newCardNumber,
                    OrganizationID = ContextManager.CurrentOrganization().OrgId,
                    TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                }
            });
            return MBdata;
        }
        private async Task<MoveBalanceData> CheckWalletsListIdentical(MoveBalanceData MBdata)
        {
            List<int> diff = null;
            // בדיקה האם כל הארנקים של הכרטיס הישן נמצאים בכרטיס בחדש
            if (MBdata.oldCardBalance.GetCardBalance_DTSResult.Body != null && MBdata.newCardBalance.GetCardBalance_DTSResult.Body != null)
                diff = MBdata.oldCardBalance.GetCardBalance_DTSResult.Body.Select(x => x.Id).Except(MBdata.newCardBalance.GetCardBalance_DTSResult.Body.Select(x => x.Id)).ToList();

            if (diff != null && diff.Count > 0)
            {
                foreach (int walletId in diff)
                {
                    // שיוך ארנקים לכרטיס חדש
                    await MBdata.client.GenerateAccounts_DTSAsync(new VPayServiceWS.GenerateAccounts_DTSRequest()
                    {
                        request = new VPayServiceWS.GenerateAccountsRequest_DTS()
                        {
                            OrganizationID = ContextManager.CurrentOrganization().OrgId,
                            ClubID = (int)MBdata.newCardInfoVerifone.VerID,
                            DefinitionId = walletId,
                            FromSerialCardNumber = MBdata.newCardInfoVerifone.SequentialNum,
                            ToSerialCardNumber = MBdata.newCardInfoVerifone.SequentialNum + 1,
                            TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                        }
                    });
                }
                diff.Clear();
            }

            List<int> diffSecond = null;
            // בדיקה האם כל הארנקים של הכרטיס החדש נמצאים בכרטיס הישן
            if (MBdata.newCardBalance.GetCardBalance_DTSResult.Body != null && MBdata.oldCardBalance.GetCardBalance_DTSResult.Body != null)
                diffSecond = MBdata.newCardBalance.GetCardBalance_DTSResult.Body.Select(x => x.Id).Except(MBdata.oldCardBalance.GetCardBalance_DTSResult.Body.Select(x => x.Id)).ToList();

            if (diffSecond != null && diffSecond.Count > 0)
            {
                foreach (int walletId in diffSecond)
                {
                    // שיוך ארנקים לכרטיס ישן
                    await MBdata.client.GenerateAccounts_DTSAsync(new VPayServiceWS.GenerateAccounts_DTSRequest()
                    {
                        request = new VPayServiceWS.GenerateAccountsRequest_DTS()
                        {
                            OrganizationID = ContextManager.CurrentOrganization().OrgId,
                            ClubID = (int)MBdata.oldCardInfoVerifone.VerID,
                            DefinitionId = walletId,
                            FromSerialCardNumber = MBdata.oldCardInfoVerifone.SequentialNum,
                            ToSerialCardNumber = MBdata.oldCardInfoVerifone.SequentialNum + 1,
                            TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                        }
                    });
                }
                diffSecond.Clear();
            }
            return MBdata;
        }
        private async Task<MoveBalanceData> SwapCards(MoveBalanceData MBdata)
        {
            MBdata.reqSwapId = _clubRepo.AddRequestRow(28, MBdata.oldCardNumber, MBdata.newCardNumber, "", 43, MBdata.reqHanpakaId);

            // שליחת בקשה להעברת יתרות לוריפון
            MBdata.cardSwapResponse = await MBdata.client.CardsSwap_DTSAsync(new VPayServiceWS.CardsSwap_DTSRequest()
            {
                request = new VPayServiceWS.Swap_Cards()
                {
                    CardNumberFrom = long.Parse(MBdata.oldCardNumber),
                    CardNumberTo = long.Parse(MBdata.newCardNumber),
                    OrganizationID = ContextManager.CurrentOrganization().OrgId,
                    OriginalRequest = MBdata.reqSwapId,
                    TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                }
            });

            if (MBdata.cardSwapResponse.CardsSwap_DTSResult.ResponseStatus_DTS != VPayServiceWS.ReponseStatuses_DTS.Succeeded)
            {
                _clubRepo.UpdateRequestRow(2, MBdata.oldCardNumber, MBdata.newCardNumber, 0, MBdata.cardSwapResponse.CardsSwap_DTSResult.ErroreMessage, MBdata.reqHanpakaId, MBdata.reqSwapId);
                MBdata.reqSwapIdUpdated = true;
                throw new BusinessException("CardSwap Failed From Verifone - " + MBdata.cardSwapResponse.CardsSwap_DTSResult.ErroreMessage);
            }

            _clubRepo.UpdateRequestRow(1, MBdata.oldCardNumber, MBdata.newCardNumber, MBdata.balance, "", MBdata.reqHanpakaId, MBdata.reqSwapId);
            MBdata.reqSwapIdUpdated = true;
            return MBdata;
        }
        private async Task<MoveBalanceData> MoveBalanceRollBack(MoveBalanceData MBdata)
        {
            // ביטול אקטיבציה של כרטיס ישן
            if (MBdata.singelActivateCardResponse != null)
            {
                MBdata.singelBlockCardResponse = await MBdata.client.SingelBlockCardAsync(new VPayServiceWS.SingelBlockCardRequest()
                {
                    request = new VPayServiceWS.CardSequentialNum_DTS
                    {
                        VerID = (int)MBdata.oldCardInfoVerifone.VerID,
                        SequentialNum = MBdata.oldCardInfoVerifone.SequentialNum,
                        OrganizationID = ContextManager.CurrentOrganization().OrgId,
                        TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                    }
                });
            }
            // עדכון בקשת הנפקה בטבלת בקשות
            if (MBdata.reqHanpakaId != 0 && !MBdata.reqHanpakaIdUpdated)
            {
                _clubRepo.UpdateRequestRow(2, "", "", 0, "נכשל בהרצת AddCardToMember", 0, MBdata.reqHanpakaId);
            }
            // עדכון בקשת סלינק בטבלת בקשות
            if (MBdata.reqSlinkId != 0 && !MBdata.reqSlinkIdUpdated)
            {
                _clubRepo.UpdateRequestRow(2, MBdata.newCardNumber, "", 0, "", MBdata.reqHanpakaId, MBdata.reqSlinkId);
            }
            // עדכון בקשת העברת יתרות בטבלת בקשות
            if (MBdata.reqSwapId != 0 && !MBdata.reqSwapIdUpdated)
            {
                _clubRepo.UpdateRequestRow(2, MBdata.oldCardNumber, MBdata.newCardNumber, 0, MBdata.cardSwapResponse != null ? MBdata.cardSwapResponse.CardsSwap_DTSResult.ErroreMessage : "", MBdata.reqHanpakaId, MBdata.reqSwapId);
            }
            return MBdata;
        }
        public async Task MoveBalanceAsync(bool? isPhysical)
        {
            MoveBalanceData MBdata = new MoveBalanceData();

            try
            {
                MBdata = await CreateNewCardAndSlinkAsync(MBdata, isPhysical);

                MBdata = await CheckBalanceAndWallets(MBdata);
                if (MBdata.balance > 0)
                {
                    MBdata = await CheckWalletsListIdentical(MBdata);

                    MBdata = await SwapCards(MBdata);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Error On MoveBalanceAsync");
                _dtsLogsRepo.AddLog(new Common.EF.DTS_Logs.ApiLogs { Body = "MOVE BALANCE EXCEPTION ----- " + ex.Message + " INNER EXP-----" + ex.InnerException + " STACK ----- " + ex.StackTrace, MethodName = "MoveBalanceAsync", QueryParams = "", StartDate = DateTime.Now, EndDate = DateTime.Now, OrganizationId = ContextManager.CurrentOrganization().OrgId, ClientIp = "1", Exception = "", LogType = 1, Response = "", ServerIp = "" });

                MBdata = await MoveBalanceRollBack(MBdata);
                //throw new BusinessException(ex.Message);
            }
        }
        public class MoveBalanceData
        {

            public string newCardNumber = string.Empty, oldCardNumber = string.Empty;
            public long reqHanpakaId = 0, reqSlinkId = 0, reqSwapId = 0;
            public bool reqHanpakaIdUpdated = false, reqSlinkIdUpdated = false, reqSwapIdUpdated = false, addedCardToMember = false;
            public decimal balance = 0;
            public VPayServiceWS.VpayServiceClient client = null;
            public VPayServiceWS.SingelUnBlockCardResponse singelActivateCardResponse = null;
            public VPayServiceWS.SingelBlockCardResponse singelBlockCardResponse = null;
            public VPayServiceWS.CardsSwap_DTSResponse cardSwapResponse = null;
            public VPayServiceWS.GetCardBalance_DTSResponse oldCardBalance = null;
            public VPayServiceWS.GetCardBalance_DTSResponse newCardBalance = null;
            public CardInfoVerifone oldCardInfoVerifone = null;
            public CardInfoVerifone newCardInfoVerifone = null;

        }

        #endregion

        #region Discharge Card

        private decimal CalculateAvailableSelfDischargeAmount(
    decimal walletBalance,
    decimal monthlyLimit,
    decimal monthlyDischargeTotal)
        {
            var availableMonthlyAmount = Math.Max(0, monthlyLimit - monthlyDischargeTotal);
            return Math.Min(walletBalance, availableMonthlyAmount);
        }

        private async Task<(decimal existingAvailable, decimal monthlyAvailable)> GetDischargeAvailabilityContext(int walletId)
        {
            VPayServiceWS.VpayServiceClient client = new VPayServiceWS.VpayServiceClient();

            string cardNumber = _clubRepo.GetActiveCardByIdentity(ContextManager.CurrentUser().Id);

            // קבל יתרת DTS לפני הבדיקה
            CardInfoVerifone cardInfoVerifone = _clubRepo.GetCardInfoForVerifoneActions(cardNumber);
            var cardBalance = await client.GetCardBalance_DTSAsync(new VPayServiceWS.GetCardBalance_DTSRequest()
            {
                card = new VPayServiceWS.Card_DTS()
                {
                    Cvv = cardInfoVerifone.CVV,
                    CardNumber = cardNumber,
                    OrganizationID = ContextManager.CurrentOrganization().OrgId,
                    TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId)
                }
            });

            var walletRow = cardBalance?.GetCardBalance_DTSResult?.Body?.FirstOrDefault(x => x.Id == walletId);
            decimal walletBalance = walletRow?.Balance ?? 0m;

            decimal existingAvailable = _clubRepo.Get14DaysBalance(cardNumber, walletId);

            // Fighter wallet נטעון ישירות ב-DTS — fallback ליתרת DTS
            var fighterWalletId = _configuration.GetConfigByValue<string>(ConfigurationKey.FighterWalletId);
            bool isFighterWallet = walletId.ToString() == fighterWalletId;

            if (existingAvailable <= 0)
            {
                if (isFighterWallet && walletBalance > 0)
                    existingAvailable = walletBalance;
                else
                    throw new BusinessException(GetMessageText(10294));
            }

            if (walletBalance < existingAvailable)
                existingAvailable = walletBalance;

            decimal monthlyLimit = _clubRepo.GetGlobalSelfDischargeLimit();
            decimal monthlyDischargeTotal = await _clubRepo.GetMemberMonthlySelfDischargeTotal(cardNumber);
            decimal monthlyAvailable = CalculateAvailableSelfDischargeAmount(walletBalance, monthlyLimit, monthlyDischargeTotal);

            return (existingAvailable, monthlyAvailable);
        }

        public async Task<decimal> GetAvailableBalanceForDischarge(int walletId)
        {
            var (existingAvailable, monthlyAvailable) = await GetDischargeAvailabilityContext(walletId);
            return Math.Min(existingAvailable, monthlyAvailable);
        }

        public Task<decimal> GetGlobalSelfDischargeLimit()
        {
            return Task.FromResult(_clubRepo.GetGlobalSelfDischargeLimit());
        }

        public async Task Discharge(int walletId, string phoneNumber, string email, string creditCardNumber, string ExpiredDate, int cvv, decimal amountToDischarge)
        {
            DischargeData dischargeObj = new DischargeData();

            var (existingAvailable, monthlyAvailable) = await GetDischargeAvailabilityContext(walletId);
            var maxAllowedAmount = Math.Min(existingAvailable, monthlyAvailable);

            if (amountToDischarge > maxAllowedAmount)
            {
                bool monthlyLimitIsBlocking = monthlyAvailable < existingAvailable;
                if (monthlyLimitIsBlocking)
                {
                    decimal monthlyLimit = _clubRepo.GetGlobalSelfDischargeLimit();
                    string msg = GetMessageText(10012658)
                        .Replace("{X}", monthlyLimit.ToString("0.##"));
                    throw new BusinessException(msg);
                }
                throw new BusinessException(GetMessageText(10295));
            }

            try
            {
                ValidateFighterDischarge(walletId, creditCardNumber);

                //dischargeObj = await GetWalletSpecsAsync(dischargeObj, amountToDischarge, walletId);
                dischargeObj = GetWalletSpecs(dischargeObj, amountToDischarge, walletId);
                dischargeObj = await DischargeVerifoneAsync(dischargeObj, amountToDischarge, walletId);
                RefundCreditCard(ref dischargeObj, creditCardNumber, ExpiredDate, cvv, walletId);

                ResponseUserDTO currentUser = ContextManager.CurrentUser();
                string cardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);
                string dicCardsInLoad = cardNumber + "_" + walletId;

                if (LoadWalletHelper.DicCardsInLoadProcessForCancel.ContainsKey(dicCardsInLoad))
                {
                    LoadWalletHelper.DicCardsInLoadProcessForCancel[dicCardsInLoad] = DateTime.Now;
                }
                else
                {
                    LoadWalletHelper.DicCardsInLoadProcessForCancel.Add(dicCardsInLoad, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Error On Discharge");
                if (dischargeObj.verifoneDischarged && !dischargeObj.creditGuardRefounded)
                {
                    // טעינה חוזרת של הכסף בוריפון
                    dischargeObj.responseDeposit = await dischargeObj.client.SingelDepositAsync(new VPayServiceWS.SingelDepositRequest()
                    {
                        request = new VPayServiceWS.SingelChargeDischargeRequest_DTS()
                        {
                            chargeDischarge = new VPayServiceWS.ChargeDischarge() { Amount = amountToDischarge, WalletID = walletId },
                            CardNumber = dischargeObj.cardNumber,
                            Cvv = dischargeObj.cardInfoVerifone.CVV,
                            OrganizationID = ContextManager.CurrentOrganization().OrgId,
                            TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId),
                            OriginalRequest = dischargeObj.prikaReqId
                        }
                    });
                    // עדכון הבקשה לכישלון כדי שלא תיספר בתקרה החודשית
                    _clubRepo.UpdateRequestRow(2, dischargeObj.cardNumber, "", amountToDischarge,
                        "ביטול פריקה - זיכוי קרדיט קארד נכשל",
                        dischargeObj.prikaReqId, dischargeObj.prikaReqId, "", walletId);
                }
                if (!dischargeObj.creditGuardRefounded || !dischargeObj.verifoneDischarged)
                    throw new BusinessException(GetMessageText(10012645));
                throw new BusinessException(ex.Message);
            }
        }
        private void ValidateFighterDischarge(int walletId, string creditCardNumber)
        {
            var fighterWalletId = _configuration.GetConfigByValue<string>(ConfigurationKey.FighterWalletId);
            var fighterBinsList = fighterBins
                .Split(';')
                .Select(b => b.Trim())
                .Where(b => !string.IsNullOrEmpty(b))
                .ToList();

            bool isFighterWallet = walletId.ToString() == fighterWalletId;
            bool isFighterCreditCard = fighterBinsList.Any(bin => creditCardNumber.StartsWith(bin));

            if (isFighterWallet)
            {
                DtsLoggger.Logger.Info($"ValidateFighterDischarge => Fighter wallet detected (WalletID: {walletId}, Config: {fighterWalletId})");

                if (!isFighterCreditCard)
                {
                    DtsLoggger.Logger.Error($"ValidateFighterDischarge => Fighter wallet but non-fighter credit card ({creditCardNumber})");
                    throw new BusinessException(GetMessageText(51166));
                }

                DtsLoggger.Logger.Info($"ValidateFighterDischarge => Fighter wallet + Fighter card verified — allowed");
            }
        }

        private void RefundCreditCard(ref DischargeData dischargeObj, string creditCardNumber, string expiredDate, int cvv, int walletId)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            LoadWalletToFuncDto loadWalletToFuncDto = _clubRepo.LoadWallet(dischargeObj.cardNumber, walletId.ToString());

            // ביצוע זיכוי בקרדיטקארד
            dischargeObj.refoundReqId = _clubRepo.AddRequestRow(28, dischargeObj.cardNumber, "", "ביצוע זיכוי בקרדיטקארד", 209, 0);
            TransactionResults cgRes = _creditGuard.Payment(new CardNumberPaymentModelView()
            {
                TerminalNumber = loadWalletToFuncDto.LoadMoneyTerminalNumber,
                Expiration = expiredDate,
                CVV = cvv.ToString(),
                CardNumber = creditCardNumber.ToString(),
                UserPersonalID = currentUser.IdentityNumber,
                TotalPrice = double.Parse((dischargeObj.amountToRefound * -1).ToString()),
                NumberOfPayments = 1
            });

            // Payments יצירת רשומה בטבלת 
            var detailsXml = new PaymentsDetailsXml
            {
                source = ContextManager.CurrentOrganization().OrgId.ToString(),
                payments = "1",
                authNumber = currentUser.Id,
                cardId = dischargeObj.cardNumber,
                tranId = cgRes.ServerTransactionID,
                terminalNumber = loadWalletToFuncDto.LoadMoneyTerminalNumber
            };
            var xml = XmlGenerator.ObjectToXml<PaymentsDetailsXml>(detailsXml);

            long paymentId;
            string last4Digit = GetLast4Digit(creditCardNumber);
            if (cgRes.Code != ResponseCode.SUCCESS)
            {
                //paymentId = _clubRepo.InsertPaymentRow(
                //    decimal.Parse((dischargeObj.amountToRefound * -1).ToString()),
                //    dischargeObj.cardNumber,
                //    last4Digit,
                //    cgRes.ServerTransactionID,
                //    currentUser.Id,
                //    currentUser.IdentityNumber,
                //    xml);

                _clubRepo.UpdateRequestRow(10, dischargeObj.cardNumber, "", (decimal)dischargeObj.amountToRefound, dischargeObj.responseDischarge.SingelPartialDischargeResult.ErroreMessage, dischargeObj.prikaReqId, dischargeObj.prikaReqId, "", walletId);
                throw new BusinessException(GetMessageText(10027));
            }
            dischargeObj.serverTransactionId = cgRes.ServerTransactionID;
            dischargeObj.creditGuardRefounded = true;

            paymentId = _clubRepo.InsertPaymentRow(
                decimal.Parse((dischargeObj.amountToRefound * -1).ToString()),
                dischargeObj.cardNumber,
                last4Digit,
                cgRes.ServerTransactionID,
                currentUser.Id,
                currentUser.IdentityNumber,
                xml);
            _clubRepo.UpdateRequestRow(1, dischargeObj.cardNumber, "", (decimal)dischargeObj.amountToRefound, "ביצוע זיכוי בקרדיט קארד", dischargeObj.refoundReqId, dischargeObj.refoundReqId, "<BarCode></BarCode><Asmachta></Asmachta><amount>" + dischargeObj.amountToRefound + "</amount><reason>ביצוע זיכוי בקרדיט קארד כשל</reason><variant>זיכוי לקוח</variant><approved></approved>", walletId, paymentId);
        }


        private async Task<DischargeData> DischargeVerifoneAsync(DischargeData dischargeObj, decimal amountToDischarge, int walletId)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            dischargeObj.prikaReqId = _clubRepo.AddRequestRow(28, dischargeObj.cardNumber, "", "פריקת יתרות", 210, 0, amountToDischarge, walletId);

            // פריקת כרטיס בוריפון
            dischargeObj.responseDischarge = await dischargeObj.client.SingelPartialDischargeAsync(new VPayServiceWS.SingelPartialDischargeRequest()
            {
                request = new VPayServiceWS.SequentialNumChargeDischargeRequest_DTS()
                {
                    chargeDischarge = new VPayServiceWS.ChargeDischarge() { Amount = amountToDischarge, WalletID = walletId },
                    SequentialNum = dischargeObj.cardInfoVerifone.SequentialNum,
                    VerID = (int)dischargeObj.cardInfoVerifone.VerID,
                    OrganizationID = ContextManager.CurrentOrganization().OrgId,
                    TerminalUniqueId = _configuration.GetConfigByValue<string>(ConfigurationKey.TerminalUniqueId),
                    OriginalRequest = dischargeObj.prikaReqId
                }
            });

            if (dischargeObj.responseDischarge.SingelPartialDischargeResult.ResponseStatus_DTS != VPayServiceWS.ReponseStatuses_DTS.Succeeded)
            {
                _clubRepo.UpdateRequestRow(2, dischargeObj.cardNumber, "", amountToDischarge, dischargeObj.responseDischarge.SingelPartialDischargeResult.ErroreMessage, dischargeObj.refoundReqId, dischargeObj.refoundReqId, "", walletId);
                throw new BusinessException(GetMessageText(10038));
            }

            dischargeObj.verifoneDischarged = true;
            _clubRepo.UpdateRequestRow(1, dischargeObj.cardNumber, "", amountToDischarge, "פריקת יתרות", dischargeObj.prikaReqId, dischargeObj.prikaReqId, "", walletId);

            return dischargeObj;
        }
        private async Task<DischargeData> GetWalletSpecsAsync(DischargeData dischargeObj, decimal amountToDischarge, int walletId)
        {
            // משיכת מספר כרטיס פעיל של חבר
            dischargeObj.cardNumber = _clubRepo.GetActiveCardByIdentity(ContextManager.CurrentUser().Id);

            // משיכת נתוני כרטיס לצורך פניות לוריפון
            dischargeObj.cardInfoVerifone = _clubRepo.GetCardInfoForVerifoneActions(dischargeObj.cardNumber);

            // משיכת נתוני ארנק לחישוב החזר
            dischargeObj.walletInfo = await GetWalletInfo(walletId.ToString());
            int moneyMaxCommissionAmoount = GetAppConfigValue<int>(MoneyMaxCommissionAmount);
            decimal moneyPercentageCancellationCommission = GetAppConfigValue<decimal>(MoneyPercentageCancellationCommission);
            // אם ארנק הנחה
            if (dischargeObj.walletInfo.DiscountMode == "D")
            {
                decimal a = ((decimal)dischargeObj.walletInfo.DiscountRate / 100M);
                decimal b = (1 - a);
                dischargeObj.charged = amountToDischarge * b;
                dischargeObj.afterCommisionTaken = dischargeObj.charged - Math.Min((dischargeObj.charged * moneyPercentageCancellationCommission), moneyMaxCommissionAmoount);
            }

            // אם ארנק מינוף
            else if (dischargeObj.walletInfo.DiscountMode == "L")
            {
                decimal a = ((decimal)dischargeObj.walletInfo.DiscountRate / 100M);
                decimal b = (1 + a);
                dischargeObj.charged = amountToDischarge / b;
                dischargeObj.afterCommisionTaken = dischargeObj.charged - Math.Min((dischargeObj.charged * moneyPercentageCancellationCommission), moneyMaxCommissionAmoount);
            }

            // עיגול ללמעלה
            dischargeObj.amountToRefound = dischargeObj.afterCommisionTaken;

            return dischargeObj;
        }
        private DischargeData GetWalletSpecs(DischargeData dischargeObj, decimal amountToDischarge, int walletId)
        {
            // משיכת מספר כרטיס פעיל של חבר
            dischargeObj.cardNumber = _clubRepo.GetActiveCardByIdentity(ContextManager.CurrentUser().Id);

            // משיכת נתוני כרטיס לצורך פניות לוריפון
            dischargeObj.cardInfoVerifone = _clubRepo.GetCardInfoForVerifoneActions(dischargeObj.cardNumber);
            // משיכת נתוני ארנק לחישוב החזר
            var refundInfo = _clubRepo.GetWalletRefundInfo(walletId);
            int moneyMaxCommissionAmoount = GetAppConfigValue<int>(MoneyMaxCommissionAmount);
            decimal moneyPercentageCancellationCommission = GetAppConfigValue<decimal>(MoneyPercentageCancellationCommission);


            //אם היית שגיאה בחיפוש
            if (refundInfo.type == "Err" || refundInfo.discountRate == -1)
            {
                throw new BusinessException("קרתה תקלה, אנא נסה שוב מאוחר יותר.");
            }
            // אם ארנק הנחה
            if (refundInfo.type == "D")
            {
                decimal a = ((decimal)refundInfo.discountRate / 100M);
                decimal b = (1 - a);
                dischargeObj.charged = amountToDischarge * b;
                dischargeObj.afterCommisionTaken = dischargeObj.charged - Math.Min((dischargeObj.charged * moneyPercentageCancellationCommission), moneyMaxCommissionAmoount);
            }
            // אם ארנק מינוף
            else if (refundInfo.type == "L")
            {
                decimal a = ((decimal)refundInfo.discountRate / 100M);
                decimal b = (1 + a);
                dischargeObj.charged = amountToDischarge / b;
                dischargeObj.afterCommisionTaken = dischargeObj.charged - Math.Min((dischargeObj.charged * moneyPercentageCancellationCommission), moneyMaxCommissionAmoount);
            }

            // עיגול ללמעלה
            dischargeObj.amountToRefound = dischargeObj.afterCommisionTaken;

            return dischargeObj;
        }
        public class DischargeData
        {
            public VPayServiceWS.VpayServiceClient client = new VPayServiceWS.VpayServiceClient();
            public VPayServiceWS.SingelPartialDischargeResponse responseDischarge = null;
            public VPayServiceWS.SingelDepositResponse responseDeposit = null;
            public CardInfoVerifone cardInfoVerifone = null;
            public WalletDTO walletInfo = null;
            public string cardNumber = null, serverTransactionId = null;
            public decimal amountToRefound = 0;
            public decimal charged = 0;
            public decimal afterCommisionTaken = 0;
            public long prikaReqId = 0, refoundReqId = 0;
            public bool verifoneDischarged = false, creditGuardRefounded = false;
        }
        private static string GetLast4Digit(string creditCardNumber)
        {
            if (!string.IsNullOrEmpty(creditCardNumber) && creditCardNumber.Length > 4)
            {
                int idx = creditCardNumber.TakeWhile(c => !char.IsWhiteSpace(c)).Count();
                string cardNumber = creditCardNumber.Substring(0, idx);
                return cardNumber.Substring(cardNumber.Length - 4);
            }
            return null;
        }
        public async Task<bool> AllowedToDischarge(string walletId)
        {
            int sec = 0;
            bool isSec = int.TryParse(ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.timeBetweenDischargeAndCharge), out sec);
            if (!isSec)
            {
                return true;
            }
            foreach (var item in LoadWalletHelper.DicCardsInLoadProcessForCancel.Where(kvp => kvp.Value.AddSeconds(sec) <= DateTime.Now).ToList())
            {
                LoadWalletHelper.DicCardsInLoadProcessForCancel.Remove(item.Key);
            }

            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            string cardNumber = _clubRepo.GetActiveCardByIdentity(currentUser.Id);

            string dicCardsInLoad = cardNumber + "_" + walletId;

            if (LoadWalletHelper.DicCardsInLoadProcessForCancel.ContainsKey(dicCardsInLoad))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public T GetAppConfigValue<T>(string key)
        {
            try
            {
                var setting = ContextManager.GetAppConfig().FirstOrDefault(f => f.Key == key);
                return (T)Convert.ChangeType(setting.Value, typeof(T));
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Failed to get value from appConfig  - " + ex.Message);
            }
            return default(T);
        }
        #endregion
    }
}
