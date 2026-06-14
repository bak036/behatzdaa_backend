using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MultipassLastCardIdRealization
    {
        public int Id { get; set; }
        public int? MultipassTransactionsMediaId { get; set; }
        public int? MultipassTransactionsLogId { get; set; }
        public int? OperationType { get; set; }
        public DateTime? OperationDate { get; set; }
        public int CardId { get; set; }
        public long? CouponId { get; set; }
        public DateTime? CouponDateCanceled { get; set; }
        public int? StockId { get; set; }
        public string Dbname { get; set; }
        public long? Asmachta { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? NetId { get; set; }
        public string NetName { get; set; }
        public long? TranId { get; set; }
        public int? Usedbenefit { get; set; }
        public int? UpdateStatus { get; set; }
        public string LockId { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Remark { get; set; }
    }
}
