using Google.Apis.ServiceUser.v1.Data;
using Google.Apis.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nofshonit.BL.BLHelper;
using Nofshonit.BL.Event;
using Nofshonit.BL.Limitations;
using Nofshonit.BL.Order.OrderUtils.OrderHandler;
using Nofshonit.BL.RestApiGW;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.DtsCancellation;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Common.Utils;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsLogsModel;
using Nofshonit.Repositories.DtsOnlineModel;
using Nofshonit.Repositories.Helpers;
using OrderDll.TelemesserSms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Nofshonit.Common.DTOs.RequestDTOs.PurchaseRequestDTO;
using Nofshonit.Logs;

namespace Nofshonit.BL.Order
{
    public class OrderBL : BaseBL, IOrderBL
    {
        private readonly IDtsOnlineRepo _dtsOnlineRepo;
        private readonly ICardsBL _cardsBL;
        private readonly IDtsLogsRepo _dtsLogsRepo;
        private readonly IClubRepo _clubRepo;
        private readonly IConfigBL _configRepo;
        private readonly ILimitationsBL _limitationsBL;
        private readonly IConfigurationManager _configuration;
        private readonly IRestApiGW _restApiGW;
        private readonly IOrderHandler _orderHandler;

        public OrderBL()
        {
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _cardsBL = Container.Resolve<ICardsBL>();
            _dtsLogsRepo = Container.Resolve<IDtsLogsRepo>();
            _clubRepo = Container.Resolve<IClubRepo>();
            _configRepo = Container.Resolve<IConfigBL>();
            _limitationsBL = Container.Resolve<ILimitationsBL>();
            _configuration = Container.Resolve<IConfigurationManager>();
            _restApiGW = Container.Resolve<IRestApiGW>();
            _orderHandler = Container.Resolve<IOrderHandler>();
        }

        public List<Common.EF.Club.AllMembers> Test()
        {
            return null;
        }

        public void Test2()
        {
        }

        /// <summary>  
        /// Purchase API  
        /// </summary>  
        /// <param name="_request"></param>  
        /// <returns></returns>  
        public async Task<PurchaseResponseDTO> PurchaseRequest(PurchaseRequestDTO _request)
        {
            ApiLoggerBL.LogConnectorData(_request.identity);
            if (!utils.ValidateCharactersOnly(_request.FirstName))
                throw new Exception();
            if (!utils.ValidateCharactersOnly(_request.LastName))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.City.ToString()))
                throw new Exception();
            if (!utils.ValidateCharactersAndNumbersOnly(_request.StreetName))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.CreditCard16Digits))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.Mobile))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.Mailbox))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.CreditCardExpirey))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.Cvv))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.TotalPayment.ToString()))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.NumOfPayments.ToString()))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.PinCode))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(_request.ZipCode))
                throw new Exception();
           

            DtsLoggger.Logger.Info(" PurchaseRequest:");
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            _request.EventsGuid = string.Empty;
            string ClubKey = string.Empty;
            PurchaseResponseDTO result;
            _request.Cart = new List<CartItem>();
            ECheckCreditCard creditCardState = _cardsBL.CheckMaxClubCreditCard(currentUser,_request.CreditCard16Digits, _request.PinCode,(_request.PinCode!=null && _request.CreditCard16Digits== null), out ClubKey);//CheckCreditCardStatus(_request);

            //TODO להוסיף לטבלתצ הודעות
            if (creditCardState != ECheckCreditCard.WithClubCreditCard || currentUser.ClubCreditCard <= 0)
                throw new BusinessException(@"כרטיס לא שיך למועדון לא ניתן להמשיך ברכישה  ");


            
            _request.CreditCardClub = ClubKey;

            if (_request.ShoppingBasketCart != null)
            {
                List<CartVarsDTO> ShoppingBasketCartCach = ContextManager.GetShopingBasketPurchaseByMember(currentUser.Id.Trim());
                if (ShoppingBasketCartCach != null && ShoppingBasketCartCach.Count == _request.ShoppingBasketCart.Count)
                {
                    if (IsShopingBasketExistInCach(_request.ShoppingBasketCart, ShoppingBasketCartCach))
                    {
                        DtsLoggger.Logger.Info($"ShoppingBasketCart exist in cach for member: {currentUser.Id.Trim()}");
                        throw new BusinessException(MessagesUtil.GetMessagesByKey(new List<int> { 10012607 })?[0]?.MessageText);
                    }
                }
                // Make sure the shopping basket items are valid ExpireDate.
                if (_request.ShoppingBasketCart.Any(s => s.ExpireDate < DateTime.Now))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(new List<int> { 10282 })?[0]?.MessageText);
                using (var context = ContextManager.ClubContext())
                {

                    var orderCategories = _request.ShoppingBasketCart.Select(y => y.CategoryId).Select(long.Parse);
                    var productVars = context.CategoryVariants.AsNoTracking().Where(x => orderCategories.Contains(x.CategoryNumber))
                                       .Join(context.ProductsVars.AsNoTracking().Where(y => !y.DisabledToOrder)
                                             , cv => cv.Barcode, pv => pv.FullBarCode, (cv, pv) => pv)
                                        .ToList();

                    productVars = productVars.Where(y => ((y.LastImplementationDate.HasValue && y.LastImplementationDate.Value.Date >= DateTime.Now.Date) || !y.LastImplementationDate.HasValue)).ToList();

                    var businessSubTypeList = productVars.Select(x => (int)x.BusinessSubTypeId.GetValueOrDefault()).Distinct().ToList();

                    var codeList = _request.ShoppingBasketCart.Select(y => y.Variant != null ? y.Variant.BarCode : y.Tickets[0].VariantFullBarcode).ToList();
                    var specsByVarsList = context.BusinessSubTypeSpecificationByVariants.AsNoTracking().Where(x => codeList.Contains(x.BarCode)).ToList();
                    var specsCurrentList = context.BusinessSubTypeSpecificationCurrent.AsNoTracking().Where(x => businessSubTypeList.Contains(x.BusinessSubTypeId)).ToList();


                    foreach (var shoppingBasketCartItem in _request.ShoppingBasketCart)
                    {
                        if (shoppingBasketCartItem.Tickets != null)
                        {

                            var ticketVariantList = new List<Variant>();
                            foreach (var ticket in shoppingBasketCartItem.Tickets)
                            {
                                Common.EF.Club.ProductsVars productVar = productVars.FirstOrDefault(f => f.FullBarCode == ticket.VariantFullBarcode);
                                ticketVariantList.Add(new Variant()
                                {
                                    Barcode = ticket.VariantFullBarcode,
                                    Price = ticket.Price,
                                    Quantity = 1,
                                    RegularPrice = Convert.ToInt32(ProductFunctions.GetVariantPrice(ContextManager.CurrentUser().PremiumType,productVar,
                                    specsByVarsList.FirstOrDefault(y => y.BarCode == ticket.VariantFullBarcode),
                                    specsCurrentList.FirstOrDefault(y => y.BusinessSubTypeId == productVar.BusinessSubTypeId),
                                false, true) ?? -1)
                                });
                                _request.EventsGuid = ticket.OrderGuid;
                            }
                            _request.Cart.Add(new CartItem()
                            {
                                CategoryId = long.Parse(shoppingBasketCartItem.CategoryId),
                                CategoryName = shoppingBasketCartItem.CategoryName,
                                Variants = ticketVariantList,
                            }); 
                        }
                        else
                        {

                            Common.EF.Club.ProductsVars productVar = productVars.FirstOrDefault(f => f.FullBarCode == shoppingBasketCartItem.Variant.BarCode);

                            var variantList = new List<Variant>();
                            variantList.Add(new Variant()
                            {

                                Barcode = shoppingBasketCartItem.Variant.BarCode,
                                Price = (int)shoppingBasketCartItem.Variant.Price,
                                Quantity = shoppingBasketCartItem.Quantity,
                                RegularPrice = Convert.ToInt32(ProductFunctions.GetVariantPrice(ContextManager.CurrentUser().PremiumType, productVar,
                                    specsByVarsList.FirstOrDefault(y => y.BarCode == shoppingBasketCartItem.Variant.BarCode),
                                    specsCurrentList.FirstOrDefault(y => y.BusinessSubTypeId == productVar.BusinessSubTypeId),
                                false, true) ?? -1)
                            });




                            _request.Cart.Add(new CartItem()
                            {
                                CategoryId = long.Parse(shoppingBasketCartItem.CategoryId),
                                CategoryName = shoppingBasketCartItem.CategoryName,
                                Variants = variantList,
                            });
                        }
                    }
                }
                //_request.ShoppingBasketCart = null;
            }
            DtsLoggger.Logger.Info("member id: Before CheckCart:");

            //ECheckCreditCard creditCardStatus = checkCart(_request);
            //if (creditCardStatus == ECheckCreditCard.WithClubCreditCard)
            //    _request.TotalPayment = _request.ShoppingBasketCart.Select(v => v.Variant.Price).FirstOrDefault().GetValueOrDefault();
            //DtsLoggger.Logger.Info("member id:{0} check creditCardStatus:{1}", currentUser.Id, creditCardStatus);
            _request.UniqueId = ContextManager.CurrentOrganization().OrganizationGuid.Trim();
            _request.MemberId = currentUser.MemberGuid.Trim();

            _request.OrderGuid = GenerateNewOrderGuid();
            _request.CreditCardStatus = currentUser.ClubCreditCard;
            var limitsList = new List<VariantOrderLimitDTO>();
            //Check if there are variants with limit 0 and if YES dont execute API but create directly the response with the proper error message. 
            if (_request.ShoppingBasketCart != null) // TEMP CODE - Remove after adding support to eventim.
            {
                limitsList = _limitationsBL.ValidatePurchesAllowed(_request.ShoppingBasketCart);

                _request.ShoppingBasketCart.ForEach(x => _limitationsBL.ValidateCategoryForMemberLimitations(long.Parse(x.CategoryId)));
            }

             if (!limitsList.Any(x => x.OrderLimit == 0))
            {
                var resultSw = new Stopwatch();
                resultSw.Start();

                result = await _orderHandler.Purchase(_request);

                //result = await HttpRequestManager.HttpRequest<PurchaseResponseDTO>(HttpUrls.LinkURL + DtsEcommerceRoutesKeys.Purchase, _request, HttpMethod.Post);
                ContextManager.SetCurrentUserCache(currentUser.Id);
                resultSw.Stop();

                if (result.Status == 1)
                {
                    // add current shopinBasket to cach
                    ContextManager.SaveShopingBasketPurchase(_request.ShoppingBasketCart, currentUser.Id);
                    // purchase process succeeded clear the shopping basket.
                    if (!Container.Resolve<IShopingBasketBL>().RemoveAllProducts())
                        throw new BusinessException("Failed to remove products from shopping cart");
                }
                if (result.Status == 0)
                {
                    return result;
                }
                ContextManager.SetCurrentUserCache(currentUser.Id);


                // Check if purchase had new card variant and call to MoveBalance
                string newDigitalCardBarcode = _configuration.GetConfigByValue<string>(ConfigurationKey.NewDigitalCardVariant);
                if (_request.ShoppingBasketCart.Any(x =>
                                                    x.Variant != null &&
                                                    ( x.Variant.BarCode.Trim() == newDigitalCardBarcode)))
                {
                
                    await _cardsBL.MoveBalanceAsync(false);
                    // מימוש וריאנט הזמנת כרטיס דיגיטלי
                    if (!_clubRepo.ImplementNewCardVariant(true))
                    {
                        throw new BusinessException("Implement New Digital Card Variant Failed");
                    }
                }



                return result;
            }
            else
            {
                var messageSet = new HashSet<string>();
                foreach (var item in limitsList.Where(x => x.OrderLimit == 0))
                {
                    //int limit = _request.ShoppingBasketCart?.Where(x => item.Barcode.Equals(x.Variant.BarCode))?.FirstOrDefault()?.Variant.MonthlyLimit ?? 0;
                    string varName = _request.ShoppingBasketCart?.Where(x => item.Barcode.Equals(x.Variant != null ? x.Variant.BarCode : ""))?.FirstOrDefault()?.CategoryName;
                    messageSet.Add(string.Format(MessagesUtil.GetMessagesByKey(new List<int> { 10011 })?[0]?.MessageText, varName));
                }

                string message = string.Join("\r\n ", messageSet);
                throw new BusinessException(message);
            }

        }
      
        private bool IsShopingBasketExistInCach(List<CartVarsDTO> ShopingBasket, List<CartVarsDTO> ShopingBasketCach)
        {
            foreach (var item in ShopingBasket)
            {
                if (item.Variant != null)
                {
                    var rowExist = ShopingBasketCach.FirstOrDefault(r => r.Variant.BarCode == item.Variant.BarCode && r.CategoryId == item.CategoryId && r.Quantity == item.Quantity);
                    if (rowExist == null)
                        return false;
                }
                if (item.Tickets != null)
                {
                    var rowExist = ShopingBasketCach.FirstOrDefault(r => r.Tickets == item.Tickets && r.CategoryId == item.CategoryId && r.Quantity == item.Quantity);
                    if (rowExist == null)
                        return false;
                }

            }
            return true;
        }
        /// <summary>  
        /// Cancelation API  
        /// </summary>  
        /// <param name="_request"></param>  
        /// <returns></returns>  
        public async Task<CancelResponseDTO> Cancel(CancelRequestDTO _request)
        {
            List<WebServiceTransactionDTO> wstList = _clubRepo.GetOrderTransactionsByOrderGuid(_request.OrderGuid);
            WebServiceTransactionDTO wst = wstList.FirstOrDefault(x => x.OrderAsmachta == long.Parse(_request.OrderConfirmation));

            // _request.OrderConfirmation = MemberOrderAsmchta, TTransactionOrder
            AtractionsOrders atractionsOrders = _clubRepo.GetAttractionsOrderByAsmachta(long.Parse(_request.OrderConfirmation));
			bool isEevent = atractionsOrders != null && atractionsOrders.TicketHubTicketId != null;
            if (isEevent)
            {
                Orders order = _clubRepo.GetOrderByGuid(_request.OrderGuid);
                List<AtractionsOrders> atractionsOrdersList = _clubRepo.GetAttractionsOrderByOrderId(order.OrderId).Where(x => x.TicketHubTicketId != null).ToList();
                wstList = wstList.Join(atractionsOrdersList, (w => w.OrderAsmachta), (o => o.MemberOrderAsmchta), (w, o) => (w)).ToList(); 
            }
            else
                wstList = new List<WebServiceTransactionDTO> { wst };

            Dictionary<string, string> dtsCancelationHeaders = new Dictionary<string, string>()
                {
                    { HeadersKeys.OrganizationId, ContextManager.CurrentOrganization().OrgId.ToString() },
                    { HeadersKeys.OrganizationGuid, ContextManager.CurrentOrganization().OrganizationGuid }
                };
            
            CancelResponseModel result = this.CancelOrder(dtsCancelationHeaders, _request, atractionsOrders, wstList);

            return new CancelResponseDTO
            {
                Status = result.Success ? 1 : 0,
                ErrorDescription = result.CancelText,
                Data = new CancelResponseDTO.CancelDataDTO
                {
                    MemberId = _request.MemberId,
                    OrderConfirmation = _request.OrderConfirmation,
                    OrderGuid = _request.OrderGuid,
                    VariantBarCode = _request.VariantBarCode,
                    CancelStatusId = result.Success ? (int)ECancelStatus.CancelledNowSuccessfully : (int)ECancelStatus.InCancelProcess,
                    CancelStatusName = result.Success ? ECancelStatus.CancelledNowSuccessfully.ToString() : ECancelStatus.InCancelProcess.ToString(),
                },
            };
        }

        private CancelResponseModel CancelOrder(Dictionary<string, string> dtsCancelationHeaders, CancelRequestDTO _request, AtractionsOrders atractionsOrders, List<WebServiceTransactionDTO> transactionsList)
        {
            ApiRequestModel apiRequestModel = new ApiRequestModel
            {
                baseUrl = HttpUrls.DtsCancellation,
                relativeUrl = DtsCancellationKeys.CancelOrder,
                data = new CancelOrderRequestModel
                {
                    OrgId = ContextManager.CurrentOrganization().OrgId,
                    OrgGuid = ContextManager.CurrentOrganization().OrganizationGuid.TrimEnd(),
                    TTransactionID = (int)transactionsList[0].ID,
                    TransactionIdList = transactionsList.Select(x => (int)x.ID).ToList(),
                    ProviderStatusId = 3, // בוטל
                    CancelledBy = 0, // לקוח
                    StatusReasonId = 1, // לקוח התחרט
                    RefundInSuccess = true,
                    RefundAfterCancel = true,
                },
                method = EHttpRequestType.POST,
                headers = dtsCancelationHeaders,
            };
            DtsLoggger.Logger.Info($"Start CancelOrder for memberId: {_request.MemberId}, TTransactionID: {transactionsList[0].ID}");
            CancelResponseModel cancelResponseModel = _restApiGW.ApiRequest<CancelResponseModel>(apiRequestModel);
            DtsLoggger.Logger.Info($"Finish CancelOrder for memberId: {_request.MemberId}, TTransactionID: {transactionsList[0].ID}," +
                $" status: {cancelResponseModel.Success}");
            return cancelResponseModel;
        }

        private CancelResponseModel RefundOrder(Dictionary<string, string> dtsCancelationHeaders, List<WebServiceTransactionDTO> transactionsList, bool isEvent)
        {
            ApiRequestModel apiRequestModel = new ApiRequestModel
            {
                baseUrl = HttpUrls.DtsCancellation,
                relativeUrl = DtsCancellationKeys.RefundOrder,
                data = new RefundOrderRequestModel
                {
                    OrgId = ContextManager.CurrentOrganization().OrgId,
                    OrgGuid = ContextManager.CurrentOrganization().OrganizationGuid.TrimEnd(),
                    TransactionIdList = transactionsList.Select(x => (int)x.ID).ToList(),
                    PaymentId = (int)transactionsList[0].PaymentId,
                    CancelCommission = 0, //אחוז עמלת ביטול
                    isShow = isEvent,
                },
                method = EHttpRequestType.POST,
                headers = dtsCancelationHeaders,
            };
            return _restApiGW.ApiRequest<CancelResponseModel>(apiRequestModel);
        }

        public bool IsCancelRequriedAproove(string barCode)
        {
            return _clubRepo.IsCancelRequriedAproove(barCode);
        }
        /// <summary>  
        /// Generate new and uniqueue OrderGuid for Purchase API  
        /// </summary>  
        /// <returns></returns>  
        private string GenerateNewOrderGuid()
        {

            var command = ContextManager.ClubContext().Database.GetDbConnection().CreateCommand();
            command.CommandText = "EXEC  GenerateOrderGuid ;";
            try
            {
                command.Connection.Open();
                var result = command.ExecuteScalar();

                return result.ToString();
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, "Error On GenerateNewOrderGuid");
                throw new BusinessException($"Error On GenerateNewOrderGuid : {e.Message}");
            }
            finally
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Dispose();
            }

        }

        public async Task<string> GetConfirmationPage(ConfirmationPageDTO _request)
        {
            _request.UniqueId = ContextManager.CurrentOrganization().OrganizationGuid.Trim();
            string memberId = ContextManager.CurrentUser().Id.Trim();
            string errors = string.Empty;
            //var clubRepo = Container.Resolve<IClubRepo>();
            var orgId = ContextManager.CurrentOrganization().OrgId;
            var template = _dtsOnlineRepo.GetConfirmationTemplate(orgId);
            var wst = _clubRepo.GetOrderTransactionsByOrderGuid(_request.OrderGuid);
            int i = 0;
            int trys = 0;
            if (wst.Any())
            {
                var paymentId = wst.First().PaymentId;
                do
                {
                    try
                    {
                        var result = await HttpRequestManager.HttpRequest<BaseResponse<OrderConfirmationResponse>>($@"{ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.BillingServiceURL)}{DtsEcommerceRoutesKeys.CreateHtmlConfirmation}{ string.Format(template.Template, paymentId, orgId, memberId)}", _request, HttpMethod.Get);
                        // = await HttpRequestManager.HttpRequest<<ConfirmationResponse>>(HttpUrls.InvoiceManagementUrl + string.Format(DtsEcommerceRoutesKeys.CreateHtmlConfirmation, paymentId, orgId, memberId), null, HttpMethod.Get);
                        if (result.Status)
                            return result.Data.Html;
                        else
                            errors += result.ErrorDescription + Environment.NewLine;

                    }
                    catch (Exception e)
                    {
                        LoggerHelper.Error(e, "Error On GetConfirmationPage PaymentId =>" + paymentId);
                    }
                } while (i++ < trys);
            }
            LoggerHelper.Error("Unable to get ConfirmationPage" + Environment.NewLine + errors);

            throw new BusinessException("תוכן דף האישור חסר");

        }
		public async Task<BarcodePopupDetails> GetPopupBarcodeDetails(string asmachta)
		{
			BarcodePopupDetails barcodePopupDetails = await _clubRepo.GetPopupBarcodeDetails(asmachta);
			return barcodePopupDetails;
		}
		//
	}
}
