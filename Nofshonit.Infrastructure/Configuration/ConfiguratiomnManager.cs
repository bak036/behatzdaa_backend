using Microsoft.Extensions.Configuration;
using System;

namespace Nofshonit.Infrastructure.Configuration
{
    public class ConfigurationManager
    {

        IConfiguration _configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetConfigByValue<T>(string configurationKey)
        {
            object value = _configuration.GetSection($"Configurations:{configurationKey}").Value;
            if (value == null)
                return default(T);
            
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
