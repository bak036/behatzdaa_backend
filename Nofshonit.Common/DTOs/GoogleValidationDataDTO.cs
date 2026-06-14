using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class GoogleValidationDataDTO
    {
        public string AccessId { get; set; }

        public string Email { get; set; }

        public GoogleValidationDataDTO(dynamic data)
        {
            AccessId = data.id;
            Email = data.emails[0].value;
            var a = 1;
        }
    }
}
