using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.DtsCancellation
{
    public class CancelOrderRequestModel
    {
        public int OrgId { get; set; }
        public int? TTransactionID { get; set; } 
        public int? ProviderStatusId { get; set; }
        public int? StatusReasonId { get; set; }
        public int? CancelledBy { get; set; }
        public string OrgGuid { get; set; }
        public string OrderGuid { get; set; }
        public bool IgnoreDaysRangeToCancel { get; set; } = false;
        public bool RefundInSuccess { get; set; } = true;
        public List<int> TransactionIdList { get; set; } = new List<int>();
        public bool RefundAfterCancel { get; set; } = false;
    }
}
