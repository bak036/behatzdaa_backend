using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class AllMembersFieldMap
    {
        public int IdCounter { get; set; }
        public byte? Status { get; set; }
        public int? ShowOrder { get; set; }
        public string NameColumn { get; set; }
        public string Name { get; set; }
        public string IdTable { get; set; }
        public string TypeColumn { get; set; }
        public int SizeColumn { get; set; }
        public string ContactTable { get; set; }
        public string ContactTableColumnId { get; set; }
        public string ContactTableColumnDescription { get; set; }
        public string PlaceContactTable { get; set; }
        public byte? Vad { get; set; }
        public bool? AllowNull { get; set; }
    }
}
