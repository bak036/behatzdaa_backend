﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public class ReferralHistory
    {
        public int Id { get; set; }

        public string MemberId { get; set; }

        public short? OrgId { get; set; }

        public string ExternalLlink { get; set; }

        public string IneerLlink { get; set; }

        public DateTime? LastUpdateMember { get; set; }
    }
}
