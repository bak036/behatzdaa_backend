using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class RptParams
    {
        public int ParamId { get; set; }
        public int JobId { get; set; }
        public string ParamType { get; set; }
        public string ParamName { get; set; }
        public string ParamDescription { get; set; }
    }
}
