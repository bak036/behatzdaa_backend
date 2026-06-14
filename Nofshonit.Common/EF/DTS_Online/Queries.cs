using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Queries
    {
        public long QueryId { get; set; }
        public int? QueryOrganizationId { get; set; }
        public byte? QuerySatus { get; set; }
        public int? QueryOrder { get; set; }
        public DateTime? QueryDefineDate { get; set; }
        public int? QueryDefineOp { get; set; }
        public string QueryName { get; set; }
        public string QuerySql { get; set; }
        public string QueryXml { get; set; }
        public string QueryObject { get; set; }
    }
}
