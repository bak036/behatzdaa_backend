using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Nofshonit.Infrastructure.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        public IConfigurationRoot Appsettings { get; set; }

        public ConfigurationManager()
        {
            Appsettings = getAppSettings();
            AddAppSettingsParameters();
        }

        public T GetConfigByValue<T>(string configKey)
        {
            var ret = Appsettings.GetValue<T>($"Configurations:{configKey}");
            return ret;
        }

        public T GetConnectionStringByValue<T>(string configKey)
        {
            var ret = Appsettings.GetValue<T>($"ConnectionStrings:{configKey}");
            return ret;
        }

        public string[] GetCorsOrigins()
        {
            var originsString = Appsettings.GetValue<string>("CorsOrigins:Origins");
            return originsString.Split(';');
        }


        private IConfigurationRoot getAppSettings()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            var builder = new ConfigurationBuilder()
            .SetBasePath(appRoot)
            .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        private void AddAppSettingsParameters()
        {
            var appConfig = System.Configuration.ConfigurationManager.AppSettings;
            var connectionStringsConfig = System.Configuration.ConfigurationManager.ConnectionStrings;

            if (appConfig.AllKeys != null && !(new List<string>(appConfig.AllKeys).Contains("PaymentsAPI")))
                appConfig.Set("PaymentsAPI", GetConfigByValue<string>("PaymentsAPI"));

            if (appConfig.AllKeys != null && !(new List<string>(appConfig.AllKeys).Contains("PaymentApiUserName")))
                appConfig.Set("PaymentApiUserName", GetConfigByValue<string>("PaymentApiUserName"));

            if (appConfig.AllKeys != null && !(new List<string>(appConfig.AllKeys).Contains("PaymentApiPassword")))
                appConfig.Set("PaymentApiPassword", GetConfigByValue<string>("PaymentApiPassword"));

            if (appConfig.AllKeys != null && !(new List<string>(appConfig.AllKeys).Contains("ConStrAll")))
                appConfig.Set("ConStrAll", GetConnectionStringByValue<string>("DTSOnlineContext"));

            if (appConfig.AllKeys != null && !(new List<string>(appConfig.AllKeys).Contains("RESTFulAPI_StockManagement_ApiUrl")))
                appConfig.Set("RESTFulAPI_StockManagement_ApiUrl", GetConfigByValue<string>("RESTFulAPI_StockManagement_ApiUrl"));
        }
    }
}
