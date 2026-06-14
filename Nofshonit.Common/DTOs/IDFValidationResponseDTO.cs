using Nofshonit.Common.DTOs.Enums;

namespace Nofshonit.Common.DTOs
{
    public class IDFValidationResponseDTO
    {
        public string Zehut { get; set; }
        public bool? isValid { get; set; }

        public EIDFPremiumeType premiumeType { get; set; }

        public EDarga darga { get; set; }

        public string cellPhone { get; set; }

        public bool ClubCreditCard { get; set; }

    }

    //bool? isValid,int premiumeType, int darga
}
