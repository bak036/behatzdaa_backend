using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class FacebookValidationDataDTO
    {
        public string AccessId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public FacebookValidationDataDTO(dynamic data)
        {
            AccessId = data.id;
            Name = data.name;
            Email = data.Email;
        }
    }
}
