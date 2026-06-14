using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class MwcHanpakaRequests
    {
        public int Hrid { get; set; }
        public int SerieId { get; set; }
        public int Amount { get; set; }
        public string Hrdescription { get; set; }
        public DateTime CreateDate { get; set; }
        public byte Hrstatus { get; set; }
        public int? StartSequentialNum { get; set; }
        public int? EndSequentialNum { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public string FaildReason { get; set; }
        public string EmailAddress { get; set; }
        public int? Opid { get; set; }
        public int? HanpakaToSerie { get; set; }
        public string HanpakaName { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}
