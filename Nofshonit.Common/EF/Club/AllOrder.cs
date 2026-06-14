using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.Club
{
    public class v_wAllOrders
    {
        public string MemberId { get; set; }
        public string BarCode { get; set; }
        public DateTime OrderDateExe { get; set; }
        public short OrderQuantity { get; set; }
        public short OrderBalance { get; set; }
        //public Guid ExternalGuid { get; set; }
        [Key]
        public long OrderAsmchta { get; set; }
        public DateTime? LastImplementationDate { get; set; }
        public long? CardId { get; set; }
        public int? OrderId { get; set; }
        public string OriginalMemberId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? TransferToFriendDate { get; set; }
        public bool IsInCancelProcess { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool IMPLEMENTATION_IN_PROCESS { get; set; }
    }

}
