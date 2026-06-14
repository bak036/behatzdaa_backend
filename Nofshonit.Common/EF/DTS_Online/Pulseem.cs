using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Pulseem
    {
        public int PulseemId { get; set; }
        public string MemberId { get; set; }
        public int OrganizationId { get; set; }
        public string Email { get; set; }
        public string PartnerEmail { get; set; }
        public string Telephone { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public DateTime? DateAdded { get; set; }
        public bool? SentToPulseem { get; set; }
        public DateTime? SentDate { get; set; }
        public string Source { get; set; }

    }
}
