using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.GeneralDTOs
{
    public class OrganizationDetailsDTO
    {
        public int OrgId { get; set; }

        public string DBName { get; set; }

        public string OrgName { get; set; }

        public string OrganizationGuid { get; set; }

        public string Password { get; set; }

        public bool IsNewSubsidy { get; set; }

        public string ServiceMail { get; set; }
    }
}
