using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TypeImplementationDate
    {
        public long Id { get; set; }
        public string ImplementationDesc { get; set; }
        public int NumToImplement { get; set; }
        public string TypeImplement { get; set; }
        public int TypeCalc { get; set; }
    }
}
