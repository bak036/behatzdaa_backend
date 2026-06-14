using Nofshonit.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Constants
{
    public static class HttpUrls
    {
        //public static readonly string LinkURL = "http://172.29.90.100:3002/";

        public static readonly string LinkURL = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.LinkURL);
        public static readonly string InvoiceManagementUrl = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.BillingServiceURL);
        public static readonly string MinistryOfDefenceURL = ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.MinistryOfDefenceURL);
        public static readonly string TicketHubURL = Common.ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.TicketHubURL);
        public static readonly string Verifone = Common.ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.Verifone);
        public static readonly string Pulseem = Common.ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.PulseemApiUrl);
        public static readonly string DtsCancellation = Common.ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.DtsCancellation);
        public static readonly string ApigeeTokenURL = Common.ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.ApigeeTokenEndpoint);
        public static readonly string ApigeeValidationRelativeURL = Common.ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.ApigeeValidationRelativePath);
    }
}
