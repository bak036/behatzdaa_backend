using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class ShopingBasket
    {
        public long Id { get; set; }
        public string MemberId { get; set; }
        public string ProductBarcode { get; set; }
        public string CategoryNumber { get; set; }
        public byte ProductSubType { get; set; }
        public byte Quantity { get; set; }
        public DateTime Expired { get; set; }
        public string XmlParams { get; set; }
        public short? SeatsStatus { get; set; }
        public int? FinalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ProductJsonForGA { get; set; }
    }
}
