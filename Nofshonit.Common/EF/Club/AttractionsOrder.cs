using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.Club
{
    public class AtractionsOrders
    {
        public string MemberID { get; set; }
        public string BarCode { get; set; }
        public DateTime MemberOrderDateEXE { get; set; }
        public short MemberOrderQuntity { get; set; }
        public short MemberOrderBlance { get; set; }
        [Key]
        public long MemberOrderAsmchta { get; set; }
        public string MemberTerminalExe { get; set; }
        public string Hodaa { get; set; }
        public string XmlData { get; set; }
        public string XMLParam { get; set; }
        public string CardNumber { get; set; }
        public decimal? CatalogicPrice { get; set; }
        public decimal? IrgunPrice { get; set; }
        public DateTime UpdatePriceDate { get; set; }
        public DateTime? LastImplementationDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public long? OriginalAsmchta { get; set; }
        public long? CardId { get; set; }
        public string BarCodeEXE { get; set; }
        public int? OrderId { get; set; }
        public string ParentMultiVariant { get; set; }
        public int? CoinsAmount { get; set; }
        public string OriginalMemberId { get; set; }
        public DateTime? TransferToFriendDate { get; set; }
        public bool IsInCancelProcess { get; set; }
        public int? TicketHubTicketId { get; set; }

        public int? ProviderStatusId { get; set; }
        public int? ProviderStatusReason { get; set; }

        public int? CancelledBy { get; set; }
        public int? MemberAddressId { get; set; }
        public long? UniqueOrderIdentity { get; set; }
    }

}
