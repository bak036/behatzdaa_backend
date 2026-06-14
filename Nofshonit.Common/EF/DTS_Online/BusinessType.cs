using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class BusinessType
    {
        public short TypeId { get; set; }
        public string TypeName { get; set; }
        public string WebTypeName { get; set; }
        public string TypeNameExact { get; set; }
        public bool EnableAdd { get; set; }
    }
}
