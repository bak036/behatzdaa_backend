using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class ContactUsDTO
    {
        public string Id { get; set; }

        public string IdentityNumber { get; set; }

        public string FullName { get; set; }

        public string InputEmail { get; set; }

        public string MobilePhone { get; set; }

        public int? CrmType{ get; set; }

        public string Subject { get; set; }

        public int? CrmSubjectId { get; set; }

        public string Description { get; set; }

        public int OrganizationId { get; set; }

        public int? PremiumType { get; set; }
        public int? ClubCreditCard { get; set; }
    }
}
