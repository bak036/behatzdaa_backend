using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TerminalSettings
    {
        public int Id { get; set; }
        public string TerminalNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
