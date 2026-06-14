using Microsoft.EntityFrameworkCore;
using Nofshonit.BL.Event;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.EF.Club;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Logs;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using Nofshonit.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsHubRepository;
using static Nofshonit.Common.DTOs.PurchaseHistoryResponseDTO;
using static Nofshonit.Repositories.Helpers.ProductFunctions;

namespace Nofshonit.BL.Purchase.PurchaseUtils.PurchaseHandler
{
    public class PurchaseHandler : BaseBL, IPurchaseHandler
    {
        private readonly IClubRepo _clubRepo;
        private readonly IDtsOnlineRepo _dtsOnlineRepo;
        private readonly ITicketsHubRepo _ticketsHubRepo;
        private readonly IConfigurationManager _configuration;
        public PurchaseHandler()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _ticketsHubRepo = Container.Resolve<ITicketsHubRepo>();
            _configuration = Container.Resolve<IConfigurationManager>();
        }

        public List<HistoryVariant> GetHistoryForMember(MemberHistoryDTO memberHistoryDTO)
        {
            List<HistoryVariant> historyVariants = new List<HistoryVariant>();
            try
            {
                List<AllOrdersHistory> allOrdersVariants = _clubRepo.GetHistoryForMember(memberHistoryDTO);
                foreach (AllOrdersHistory variant in allOrdersVariants)
                {
                    if(variant.BusinessSubTypeID != 26)
                        variant.DeliveryAddress = "";
                }

                List<ProductsVars> productsVars = _clubRepo.GetProductsVar(allOrdersVariants.Select(y => y.FullBarCode).ToList());
                List<long> asmachtq = allOrdersVariants.Select(y => y.OrderAsmchta).ToList();

                List<v_wAllOrders> allOrders = _clubRepo.GetAllOrdersForMember(memberHistoryDTO);
                var resVariant = allOrdersVariants.GroupBy(x => new { x.OrderId, x.OrderAsmchta });
                foreach (IEnumerable<AllOrdersHistory> group in resVariant)
                {
                    AllOrdersHistory variant = null;
                    bool isCanceledVariant = group.Any(x => int.Parse(x.TTransactionquantity) < 0);

                    if (isCanceledVariant)
                        variant = group.First(x => int.Parse(x.TTransactionquantity) < 0);
                    else
                        variant = group.Last();

                    ProductsVars productVar = productsVars.FirstOrDefault(x => x.FullBarCode.Equals(variant.FullBarCode));
                    v_wAllOrders allOrder = allOrders.FirstOrDefault(x => x.OrderAsmchta == variant.OrderAsmchta && x.BarCode.Equals(variant.FullBarCode));
                    byte? stockType = 5;
                    if (productVar.CuponStockId != null)
                    {
                        stockType = _clubRepo.GetCouponsStockType(productVar.CuponStockId.Value).StockType;
                    }
                    int benefitTypeId = ProductFunctions.GetBenefitTypeIdByVariantType(productVar.VariantType);
                    EOrderStatus benefitStatus = ProductFunctions.GetOrderStatus(_dtsOnlineRepo, benefitTypeId, allOrder);

                    HistoryVariant newVariant = new HistoryVariant
                    {
                        Name = variant.CategoryName,
                        BenefitId = variant.CategoryNumber.ToString(),
                        BenefitTypeId = benefitTypeId,
                        BusinessId = variant.BusinessId,
                        BusinessName = variant.BusinessName,
                        BusinessSubTypeId = variant.BusinessSubTypeID.ToString(),
                        BusinessSubTypeName = variant.BusinessSubTypeName,
                        ExpiredDate = variant.LastImplementationDate ?? DateTime.MinValue,
                        EndDate = variant.EndDate ?? DateTime.MinValue,
                        VariantBarCode = variant.FullBarCode,
                        VariantName = variant.shortNameVar,
						DigitalCodeType = variant.DigitalCodeType,
						OrderConfirmation = variant.UniqueOrderIdentity.ToString(),
                        CustomerPrice = variant.CustomerPrice ?? -1,
                        CoinsUse = variant.Coins.GetValueOrDefault(0),
                        RedimTypeId = variant.RedimTypeId.GetValueOrDefault(0),
                        RedimTypeName = variant.RedimTypeName,
                        DtsOrderId = variant.OrderId,
                        OrderGuid = variant.ExternalGuid,
                        FriendName = variant.FriendName,
                        FriendMobile = variant.FriendMobile,
                        BenefitStatusId = (int)benefitStatus,
                        BenefitStatusName = benefitStatus.ToString(),
                        GiftCardValue = benefitTypeId == (int)BenefitType.GiftCard ? variant.LoadingAmount : null,
                        GiftCardBalance = benefitTypeId == (int)BenefitType.GiftCard ? this.GetGiftCardBalance(variant.CardNumber) : null,
                        DateOfRedim = benefitTypeId == (int)BenefitType.GiftCard ? this.GetGiftCardRedimDate(variant.CardNumber) : variant.OrderDateExe,
                        IsAlreadySentToFriend = variant.IsSentToFriend ? 1 : 0,
                        CancelDate = variant.TTransactionDateTime.Value,
                        CreditCard16Digits = variant.CreditCard16Digits ?? string.Empty,
                        CreditCardExpirey = variant.CreditCardExpirey ?? string.Empty,
                        DtsRedimCode = variant.CardNumber,
                        MaxCancelDate = variant != null ? variant.TTransactionDateTime.Value.AddDays(14) : null,
                        Slink = variant.Slink,
                        MovieLink = variant.LinkToTheatre,
                        PaymentToken = string.Empty,
                        EventsTicketLink = string.Empty,
                        TTransactionID = variant.TTransactionID,
                        TTransactionOrder = variant.TTransactionOrder.GetValueOrDefault(),
                        BusinessAddress = variant.BusinessAddress,
                        BusinessStreetNumber = variant.BusinessStreetNumber,
                        BusinessCity = variant.BusinessCity,
                        BusinessPhoneNumber = variant.BusinessPhoneNumber,
                        TrackingNumber = variant.TrackingNumber,
                        TrackingWebsite = variant.TrackingWebsite,
                        OrderStatus = variant.OrderStatus,
                        DeliveryModeDescription = variant.DeliveryModeDescription,
                        DeliveryAddress = variant.DeliveryAddress,
                        PremiumType = variant.PremiumType,
						IsDisplayButton = productVar.BusinessSubTypeId != 6 && productVar.BusinessSubTypeId != 20 && productVar.BusinessSubTypeId != 26,
                        IsExternalCoupon = stockType == 0 ? true : false
                    };

                    if (isCanceledVariant)
                    {
                        AllOrdersHistory variantBeforeCancel = group.FirstOrDefault(x => int.Parse(x.TTransactionquantity) > 0);
                        newVariant.OrderDate = variantBeforeCancel == null ? variant.TTransactionDateTime.Value : variantBeforeCancel.TTransactionDateTime.Value;
                        newVariant.IsSendToFriend = 0;
                        newVariant.IsCancelable = 0;
                        newVariant.Quantity = Math.Abs(int.Parse(variant.TTransactionquantity));
                    }
                    else
                    {
                        newVariant.OrderDate = variant.TTransactionDateTime.HasValue ? variant.TTransactionDateTime.Value : DateTime.MinValue;
                        newVariant.IsSendToFriend = !benefitStatus.Equals(EOrderStatus.NotImplemented) ? 0 : variant.IsSendToFriend.HasValue && variant.IsSendToFriend.Value ? 1 : 0;
                        newVariant.IsCancelable = ProductFunctions.IsCancelable(new IsCancelableModel
                        {
                            AutoImplementaionAfterReport = variant.AutoImplementaionAfterReport,
                            BenefitTypeId = benefitTypeId,
                            DaysBeforeShowToAllowCancel = variant.DaysBeforeShowToAllowCancel.GetValueOrDefault(),
                            DaysRangeToCancel = variant.DaysRangeToCancel.GetValueOrDefault(),
                            LastImplementationDate = variant.LastImplementationDate.GetValueOrDefault(),
                            Status = benefitStatus,
                            TTransactionDateTime = variant.TTransactionDateTime.GetValueOrDefault(),
                            ShowDate = productVar.ShowDate.GetValueOrDefault()
                        }) ? 1 : 0;
                        newVariant.Quantity = int.Parse(variant.TTransactionquantity);
                    }

                    if(EventFunctions.IsEventimBenefitByBarcode(_clubRepo, newVariant.VariantBarCode))
                        EventFunctions.AddHistoryEventData(_ticketsHubRepo, _clubRepo, _configuration, ref newVariant);

                    historyVariants.Add(newVariant);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in GetHistoryForMember, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
            return historyVariants;
        }

        public decimal? GetGiftCardBalance(string cardNumber)
        {
            decimal? balance = _dtsOnlineRepo.MwcMediasExecuteQuery(query => query.AsNoTracking().Where(x => x.CardNumber.Equals(cardNumber)).Sum(x => x.Amount));
            if (balance.HasValue && balance > 0)
                return Math.Round(decimal.Parse((balance / 100).ToString()));
            else
                return 0;
        }

        public DateTime? GetGiftCardRedimDate(string cardNumber)
        {
            return _dtsOnlineRepo.MwcMediasExecuteQuery(query => query.Where(x => x.CardNumber.Equals(cardNumber)).OrderBy(x => x.TransactionDateTime).FirstOrDefault(x => x.Amount < 0)?.TransactionDateTime);
        }
    }
}
