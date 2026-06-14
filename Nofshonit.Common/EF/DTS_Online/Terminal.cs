using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Terminal
    {
        public string TerminalNo { get; set; }
        public string BuisnessId { get; set; }
        public string Type { get; set; }
        public string Rem { get; set; }
        public DateTime? InstallDate { get; set; }
        public DateTime? UnInstallDate { get; set; }
        public bool? TerminalActive { get; set; }
    }
}
