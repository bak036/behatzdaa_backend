using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LoadFilesFields
    {
        public int Id { get; set; }
        public int? LoadFileId { get; set; }
        public string FieldName { get; set; }
        public int? FieldType { get; set; }
        public bool? IsPrimaryKey { get; set; }
        public int? OrderBy { get; set; }
    }
}
