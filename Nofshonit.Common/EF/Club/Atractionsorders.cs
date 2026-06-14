using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Atractionsorders
    {
        public string MemberId { get; set; }
        public string BarCode { get; set; }
        public DateTime MemberOrderDateExe { get; set; }
        public short MemberOrderQuntity { get; set; }
        public short MemberOrderBlance { get; set; }
        public long MemberOrderAsmchta { get; set; }
        public string MemberTerminalExe { get; set; }
        public string Hodaa { get; set; }
        public string XmlData { get; set; }
        public string Xmlparam { get; set; }
        public string CardNumber { get; set; }
        public decimal? CatalogicPrice { get; set; }
        public decimal? IrgunPrice { get; set; }
        public DateTime UpdatePriceDate { get; set; }
        public DateTime? LastImplementationDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public long? OriginalAsmchta { get; set; }
        public long? CardId { get; set; }
        public string BarCodeExe { get; set; }
        public int? OrderId { get; set; }
        public string ParentMultiVariant { get; set; }
        public int? CoinsAmount { get; set; }
        public string OriginalMemberId { get; set; }
        public DateTime? TransferToFriendDate { get; set; }
        public int? TicketHubTicketId { get; set; }
        public bool? IsInCancelProcess { get; set; }
        public long? UniqueOrderIdentity { get; set; }
    }
}
