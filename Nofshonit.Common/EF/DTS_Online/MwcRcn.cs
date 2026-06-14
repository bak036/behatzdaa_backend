using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcRcn
    {
        public int RcnId { get; set; }
        public DateTime DateCreated { get; set; }
        public int SerieId { get; set; }
        public int Amount { get; set; }
        public string DescriptionRcn { get; set; }
        public string FileFtpUrl { get; set; }
        public byte? RcnStatus { get; set; }
        public int? StartSequentialNum { get; set; }
        public int? EndSequentialNum { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public int? Opid { get; set; }
        public string ErrorMessage { get; set; }
        public string DumpUrlApproveCards { get; set; }

        public virtual MwcSeries Serie { get; set; }
    }
}
