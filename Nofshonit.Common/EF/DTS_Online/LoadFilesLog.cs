using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LoadFilesLog
    {
        public int Id { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? LoadFileId { get; set; }
        public string FileName { get; set; }
        public int? NumOfRecords { get; set; }
    }
}
