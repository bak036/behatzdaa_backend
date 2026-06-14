using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nofshonit.Common
{
    public static class ConfigurationKey
    {
        public const string minuteToAddShopingBasketToCach = "minuteToAddShopingBasketToCach";
        public const string ContactUsPath = "configCRM";
        public const string Username = "Username";
        public const string Password = "Password";
        public const string DTSWalletMVC = "DTSWalletMVC";
        public const string VPayService = "VPayService";
        public const string VPayServiceWS = "VPayServiceWS";
        public const string RecoverPasswordURL = "recoverPasswordURL";
        public const string UpdateDetailsMonthExpiry = "updateDetailsMonthExpiry";
        public const string UpdatePasswordMonthExpiry = "PasswordExpiryInMonths";
        public const string LastLoginAttemptsMinExpiry = "lastLoginAttemptsMinExpiry";
        public const string LastLoginAttemptsExpiry = "lastLoginAttemptsExpiry";
        public const string lastRegisterAttemptsExpiry = "lastRegisterAttemptsExpiry";
        public const string LastJoinAttemptsMinExpiry = "LastJoinAttemptsMinExpiry";
        public const string LastJoinAttemptsExpiry = "LastJoinAttemptsExpiry";
        public const string DefaultWallet = "defaultWallet";
		public const string ShoppingBasket_EventimExpireDate = "MinutesToExpired_ShoppinBasketEventim";
		public const string ShoppingBasket_ExpireDate = "MinutesToExpired_ShoppinBasket";
        public const string MinutesToExpired_RecoverPassword = "MinutesToExpired_RecoverPassword";
        public const string MinutesToExpired_UserToken = "MinutesToExpired_UserToken";
        public const string TerminalUniqueId = "TerminalUniqueId";
        public const string PreventDuplicateLoadMoneyInPeriodOfSeconeds = "PreventDuplicateLoadMoneyInPeriodOfSeconeds";
        public const string Slink_Link = "Slink_Link";
        public const string ClubCradSlinkImage = "ClubCradSlinkImage";
        public const string LinkURL = "LinkURL";
        public const string InvoiceManagementURL = "InvoiceManagementURL";
        public const string TicketHubURL = "TicketHubURL";
        public const string Verifone = "Verifone";
        public const string LeumiCard = "LeumiCard";
        public const string DBLogLevel = "DBLogLevel";
        public const string DBLogLevelMethods = "DBLogLevelMethods";
        public const string LocalMembersAllowedLogin = "LocalMembersAllowedLogin";
        public const string IsUnitTest = "IsUnitTest";
        public const string SkipMinistryOfDefenceValidation = "SkipMinistryOfDefenceValidation";
        public const string SkipMaxCardValidation = "SkipMaxCardValidation";
        public const string AllowedLoginPeriod = "AllowedLoginPeriod";
        public const string DaysToIgnoreHistadrutValidationWhenServiceIsDown = "DaysToIgnoreHistadrutValidationWhenServiceIsDown";
        public const string Status5MonthlyLimit = "Status5MonthlyLimit";
        public const string MaxAmountOfSmsIn2Minutes = "MaxAmountOfSmsIn2Minutes";
        public const string SmsLoginCodeLength = "SmsLoginCodeLength";
        public const string SmsLoginCodeMaxAgeInMinutes = "SmsLoginCodeMaxAgeInMinutes";
        public const string NewCardVariant = "NewCardVariant";
        public const string NewDigitalCardVariant = "NewDigitalCardVariant";
        public const string TerminalNumber = "TerminalNumber";
        //public const string TerminalNumberDischarge = "TerminalNumberDischarge";
        public const string PaymentsAPI = "PaymentsAPI";
        public const string RESTFulAPI_StockManagement_ApiUrl = "RESTFulAPI_StockManagement_ApiUrl";
        public const string BillingServiceURL = "BillingServiceURL";
        public const string MinistryOfDefenceURL = "MinistryOfDefenceURL";
        public const string HttpClientMessageKeys = "HttpClientMessageKeys";
        public const string PulseemGroupIDs = "PulseemGroupIDs";
        public const string PulseemOrganizationNames = "PulseemOrganizationNames";
        public const string WSZakautHeaders = "WSZakautHeaderToken";
        public const string MinistryOfDefenceTimeOut = "MinistryOfDefenceTimeOut";
        public const string OldMiluimOrdersUrl = "OldMiluimOrdersUrl";
        public const string ShortUrlBase = "ShortUrl_Base";
        public const string Dts_Logs_DB = "Dts_Logs";
        public const string SendSMSIIS_Url = "SendSMSIIS_Url";
        public const string TimeSaveCache = "TimeSaveCache";
        public const string RecaptchaSecretKey = "RecaptchaSecretKey";
        public const string RecaptchaGoogleApi = "RecaptchaGoogleApi";
        public const string ReCaptchaScoreNeeded = "ReCaptchaScoreNeeded";
        public const string AllowCancelExternalCoupon = "AllowCancelExternalCoupon";
        public const string walletPictureUrl = "walletPictureUrl";
        public const string timeBetweenDischargeAndCharge = "timeBetweenDischargeAndCharge";
        public const string MemberInactivityInYears = "MemberInactivityInYears";
		public const string Domain = "Domain";
		public const string CustomerSupportEmail = "CustomerSupportEmail";
        public const string JoinMaxAttempts = "JoinMaxAttempts";
        public const string JwtKey = "Jwt:Key";
        public const string JwtIssuer = "Jwt:Issuer";
        public const string JwtAudience = "Jwt:Audience";
        public const string JwtExpireMinutes = "Jwt:expireMinutes";
        public const string PulssemApiKey = "Pulseem:ApiKey";
        public const string PulseemApiUrl = "Pulseem:ApiUrl";
        public const string PulseemOrgName = "Pulseem:OrgName";
        public const string EventTicketView = "EventTicketView";
        public const string DtsCancellation = "DtsCancellation";
        public const string EncryptionKey = "EncryptionKey";
        public const string EncryptionKeyNewOrder = "EncryptionKeyNewOrder";
        public const string SaltKey = "SaltKey";
        public const string VIBaseKey = "VIBaseKey";
        public const string ConfirmationSmsSenderName = "ConfirmationSmsSenderName";
        public const string SubTypeTzarchanut = "SubTypeTzarchanut";
		public const string LongUrl = "LongUrl";
		public const string SendSmsByBl2 = "SendSmsByBl2";
        public const string MinAmountToSendSmsAfterLoad = "MinAmountToSendSmsAfterLoad";
        public const string AllowedFullLoginIPs = "AllowedFullLoginIPs";
        public const string PincodeLength = "PincodeLength";
        public const string ClubNameForSubject = "ClubNameForSubject";
        public const string FighterBins = "FighterBins";
        public const string FighterWalletId = "FighterWalletId";
        public const string ApigeeTokenEndpoint = "ApigeeTokenEndpoint";
        public const string ApigeeValidationRelativePath = "ApigeeValidationRelativePath";
        public const string ApigeeConsumerKey = "ApigeeConsumerKey";
        // Hold for backward compatibility only. The plain consumer_secret is no longer used.
        // Replaced by BehatzdaaApigeeEncryptedSecret (web.config / appsettings.json) +
        // BehatzdaaApigeeSecretSalt (dbo.AppConfig) + BEHATZDAA_APIGEE_ENCRYPTION_KEY (env var).
        public const string ApigeeConsumerSecret = "ApigeeConsumerSecret";

        // Encrypted consumer_secret (Base64 of IV + AES-CBC ciphertext) stored in appSettings / web.config.
        public const string BehatzdaaApigeeEncryptedSecret = "BehatzdaaApigeeEncryptedSecret";

        // Key under dbo.AppConfig that holds the random Base64 salt used for PBKDF2 key derivation.
        public const string BehatzdaaApigeeSecretSalt = "BehatzdaaApigeeSecretSalt";

        // Configuration key (under appSettings / Configurations) holding the encryption key
        // used as PBKDF2 password. Stored per-environment in appsettings.{Environment}.json.
        // SECURITY NOTE: this value MUST NOT be committed to source control together with
        // BehatzdaaApigeeEncryptedSecret. Use environment-specific config transforms / secrets.
        public const string BehatzdaaApigeeEncryptionKey = "BehatzdaaApigeeEncryptionKey";

        public static string GetValueFromConfigurations(string key)
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            var builder = new ConfigurationBuilder()
            .SetBasePath(appRoot)
            .AddJsonFile("appsettings.json");

            var config = builder.Build();
            return config.GetValue<string>($"Configurations:{key}");
        }
    }
}
