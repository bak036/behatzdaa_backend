using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Product
{
    public class IsCancelableModel
    {
        public EOrderStatus Status { get; set; }
        public bool AutoImplementaionAfterReport { get; set; }
        public int DaysBeforeShowToAllowCancel { get; set; }
        public int DaysRangeToCancel { get; set; }
        public int BenefitTypeId { get; set; }
        public DateTime LastImplementationDate { get; set; }
        public DateTime TTransactionDateTime { get; set; }
        public DateTime ShowDate { get; set; }
    }
}
