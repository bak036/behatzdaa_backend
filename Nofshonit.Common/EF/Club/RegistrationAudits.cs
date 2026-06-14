using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class RegistrationAudits
    {
        public RegistrationAudits()
        {
        }

        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IdentityNumber { get; set; }
        public int? AttemptsCount { get; set; }
        public DateTime? LastAttemptDate { get; set; }
        public int? IsMember { get; set; }
        public int? Darga { get; set; }
        public int? PremiumType { get; set; }
        public string Cellphone { get; set; }
        public int ClubCreditCard { get; set; }
       
    }
}
