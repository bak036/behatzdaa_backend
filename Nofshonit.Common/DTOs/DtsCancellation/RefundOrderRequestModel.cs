using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.DtsCancellation
{
    public class RefundOrderRequestModel
    {
        public int OrgId { get; set; }
        public string OrgGuid { get; set; }
        public int PaymentId { get; set; }
        public float? CancelCommission { get; set; }
        public int? TTransactionID { get; set; }
        public bool isShow { get; set; }
        public List<int> TransactionIdList { get; set; } = new List<int>();
    }
}
