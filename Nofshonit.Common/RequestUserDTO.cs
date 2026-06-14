using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common
{
    public class RequestUserDTO
    {
        public string Id { get; set; }

        public ELoginType LoginType { get; set; }

        public string AccessID { get; set; }

        public string Email { get; set; }

        public string IdentityNumber { get; set; }
    }
}
