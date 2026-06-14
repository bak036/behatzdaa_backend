using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class AuthenticateUserRequestDTO
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public string AccessID { get; set; }

        public string IdentityNumber { get; set; }
        public string Email { get; set; }
        public string PrivateBiometricToken { get; set; }
        public string PaylodBiometricToken { get; set; }

        public ELoginType LoginType { get; set; }

        public EAuthenticateOrJoin AuthenticateOrJoin { get; set; }

        public short? Active { get; set; }
        public bool? ConfirmPrivacyPolicy { get; set; }
    }
}
