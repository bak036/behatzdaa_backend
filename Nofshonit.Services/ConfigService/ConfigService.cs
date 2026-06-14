using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;

namespace Nofshonit.Services.ConfigService
{
    public class ConfigService : Base.BaseService,IConfigService
    {
        public ConfigService() { }
        public async Task<List<AppConfig>> GetValueByKeyList(List<string> request)
        {
            return await Container.Resolve<IConfigBL>().GetValueByKeyList(request);
        }

        public async Task<List<AppConfig>> GetValueByKey(string request)
        {
            return await Container.Resolve<IConfigBL>().GetValueByKey(request);
        }
    }
}
