using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class CloseDaysorders
    {
        public string MemberId { get; set; }
        public string BarCode { get; set; }
        public DateTime MemberOrderDateExe { get; set; }
        public short? MemberOrderQuntity { get; set; }
        public short? MemberOrderBlance { get; set; }
        public long? MemberOrderAsmchta { get; set; }
        public short? MimushCharig { get; set; }
        public string Hodaa { get; set; }
        public string Xmlparam { get; set; }
    }
}
