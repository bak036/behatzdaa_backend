using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class UpdateSubscriptionDTO
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public int OrganizationId { get; set; }

        public bool Subscribed { get; set; }

        public UpdateSubscriptionDTO()
        { }

    }
}
