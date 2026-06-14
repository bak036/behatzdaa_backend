using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MwcMultiRequest
    {
        public int MwcRequestId { get; set; }
        public int Hrid { get; set; }
        public DateTime MwcRequestDate { get; set; }
        public byte MwcRequestTypeId { get; set; }
        public string Barcode { get; set; }
        public decimal? Amount { get; set; }
        public int? StartSequentialNum { get; set; }
        public int? EndSequentialNum { get; set; }
        public int? PopulationTypeId { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public string FaildReason { get; set; }
        public int? ExecuteAmount { get; set; }
        public int? Opid { get; set; }
        public byte? MwcRequestStatus { get; set; }
        public byte? RechargeResonCode { get; set; }
        public long? WalletId { get; set; }
        public string DumpUrl { get; set; }
        public string RequestDescription { get; set; }
        public int? VariantQuantity { get; set; }
    }
}
