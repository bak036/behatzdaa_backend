using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class RechargeResones
    {
        public short Code { get; set; }
        public short? CodeType { get; set; }
        public bool? IsActive { get; set; }
        public string ShortDescription { get; set; }
        public string Rem { get; set; }
    }
}
