using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcHanpakaRequestsPrint
    {
        public int IdIdentity { get; set; }
        public int Hrid { get; set; }
        public int OrgId { get; set; }
        public byte PrintComp { get; set; }
        public DateTime Createdate { get; set; }
        public byte Status { get; set; }
        public string Error { get; set; }
        public string Filename { get; set; }
        public string FileNameSum { get; set; }
        public string FileNameMashov { get; set; }
        public string MashovId { get; set; }
        public DateTime? MashovDate { get; set; }
        public int? MashovCount { get; set; }
    }
}
