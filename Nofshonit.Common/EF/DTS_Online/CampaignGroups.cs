using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CampaignGroups
    {
        public long GroupId { get; set; }
        public long CampainId { get; set; }
        public byte GroupType { get; set; }
        public byte GroupSubType { get; set; }
        public byte GroupStatus { get; set; }
        public string GroupDescription { get; set; }
        public string Xmlparam { get; set; }
        public bool? AllMembers { get; set; }
        public string TerminalForAddMembers { get; set; }
        public byte? GroupPriority { get; set; }
    }
}
