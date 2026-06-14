using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LcmediaRecords
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int MerchantNumber { get; set; }
        public string BussinessName { get; set; }
        public string Branch { get; set; }
        public int NumbersOfVoucher { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountOther { get; set; }
        public decimal CommissionPercentage { get; set; }
        public decimal CommissionAfterVat { get; set; }
        public decimal CommissionBeforeVat { get; set; }
        public decimal Vat { get; set; }
        public DateTime InsertTime { get; set; }
    }
}
