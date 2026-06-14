using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MultipassTransactionsMedia
    {
        public int Id { get; set; }
        public int? MultipassTransactionsLogId { get; set; }
        public int? OperationType { get; set; }
        public DateTime? OperationDate { get; set; }
        public int? CardId { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? NetId { get; set; }
        public string NetName { get; set; }
        public long? TranId { get; set; }
        public int? Usedbenefit { get; set; }
        public int? UpdateStatus { get; set; }
        public string LockId { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
