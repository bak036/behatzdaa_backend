using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LoadFiles
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string SplitChar { get; set; }
        public string Path { get; set; }
        public string Filter { get; set; }
        public string TableName { get; set; }
        public bool? CheckFieldsCount { get; set; }
        public bool? LoadAllOnly { get; set; }
        public bool? Header { get; set; }
        public int? CheckByPrimaryKey { get; set; }
        public string RunSpWhenFinished { get; set; }
        public string HistoryFolder { get; set; }
        public string UpdateInsertDate { get; set; }
        public bool? BackupErrorFiles { get; set; }
    }
}
