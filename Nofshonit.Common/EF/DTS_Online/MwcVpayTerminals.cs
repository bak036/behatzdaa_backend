using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayTerminals
    {
        public int TerminalId { get; set; }
        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string TerminalDescription { get; set; }
        public int Owner { get; set; }
        public int TerminalType { get; set; }
        public byte StateId { get; set; }
        public string CreateDate { get; set; }
        public int ParentBranch { get; set; }
        public string Authorizations { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
