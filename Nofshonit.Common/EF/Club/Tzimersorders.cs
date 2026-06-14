using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class Tzimersorders
    {
        public string MemberId { get; set; }
        public string BarCode { get; set; }
        public DateTime OrderDateExe { get; set; }
        public short OrderQuantity { get; set; }
        public short OrderBalance { get; set; }
        public long OrderAsmchta { get; set; }
        public string TerminalExe { get; set; }
        public string Hodaa { get; set; }
        public long? ProviderId { get; set; }
        public string Xmlparam { get; set; }
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
        public string CardNumber { get; set; }
        public bool? IsInCancelProcess { get; set; }
    }
}
