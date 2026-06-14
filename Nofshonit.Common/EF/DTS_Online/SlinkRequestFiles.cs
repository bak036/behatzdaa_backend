using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SlinkRequestFiles
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public DateTime InsertDate { get; set; }
        public bool FileStatus { get; set; }
        public int? FileSlinkId { get; set; }
    }
}
