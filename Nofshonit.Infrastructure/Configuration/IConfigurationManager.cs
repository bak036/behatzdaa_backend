using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Infrastructure.Configuration
{
    public interface IConfigurationManager
    {
        T GetConfigByValue<T>(string configKey);

        T GetConnectionStringByValue<T>(string configKey);

        string[] GetCorsOrigins();
    }
}
