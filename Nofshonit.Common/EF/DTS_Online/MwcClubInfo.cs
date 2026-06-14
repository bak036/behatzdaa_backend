using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcClubInfo
    {
        public long Id { get; set; }
        public long? ClubId { get; set; }
        public long? TransactionIdentifier { get; set; }
        public string FtpHost { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPass { get; set; }
        public string FtpPort { get; set; }
        public string Url { get; set; }
        public string Mails { get; set; }
    }
}
