using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class DbgenericStructure
    {
        public long Counter { get; set; }
        public string NameItem { get; set; }
        public string TypeItem { get; set; }
        public string NameColumn { get; set; }
        public string TypeColumn { get; set; }
        public int? SizeColumn { get; set; }
        public long? IndentitySeedColumn { get; set; }
        public long? IndentityIncrementColumn { get; set; }
        public string ConstraintColumn { get; set; }
        public byte? AllowNullColumn { get; set; }
        public string DefaultValueColumn { get; set; }
        public string TextViewAndSp { get; set; }
    }
}
