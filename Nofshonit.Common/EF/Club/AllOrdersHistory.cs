using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.Club
{
    public class AllOrdersHistory
    {
        public string CategoryName { get; set; }

        public long CategoryNumber { get; set; }

        public short? BusinessSubTypeID { get; set; }

        public string BusinessId { get; set; }

        public DateTime? LastImplementationDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? TTransactionDateTime { get; set; }

        public string FullBarCode { get; set; }

        public string shortNameVar { get; set; }
		public int DigitalCodeType { get; set; }

		public long? TTransactionOrder { get; set; }

        public decimal? CustomerPrice { get; set; }

        public int? Coins { get; set; }

        public string TTransactionquantity { get; set; }

        public int OrderId { get; set; }

        public int? RedimTypeId { get; set; }

        public string ExternalGuid { get; set; }

        public bool? IsSendToFriend { get; set; }

        public string FriendName { get; set; }

        public string FriendMobile { get; set; }

        public int? LoadingAmount { get; set; }

        public DateTime OrderDateExe { get; set; }

        public DateTime OrderDate { get; set; }

        public bool IsSentToFriend { get; set; }

        public string CreditCard16Digits { get; set; }

        public string CreditCardExpirey { get; set; }

        public string CardNumber { get; set; }

        public string MemberID { get; set; }

        public string OriginalMemberId { get; set; }

        public string DeliveryAddress { get; set; }

        [Key]
        public long OrderAsmchta { get; set; }

        public string BusinessName { get; set; }

        public string BusinessSubTypeName { get; set; }

        public string RedimTypeName { get; set; }

        public bool AutoImplementaionAfterReport { get; set; }

        public int? DaysBeforeShowToAllowCancel { get; set; }

        public int? DaysRangeToCancel { get; set; }

        public string Slink { get; set; }

        public string LinkToTheatre { get; set; }

        public long TTransactionID { get; set; }

        #region TrackingRegion
        public string BusinessAddress { get; set; }
        public string BusinessStreetNumber { get; set; }
        public string BusinessCity { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingWebsite { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryModeDescription { get; set; }
        #endregion
        public int? PremiumType { get; set; }

        public long? UniqueOrderIdentity { get; set; }

    }

}
