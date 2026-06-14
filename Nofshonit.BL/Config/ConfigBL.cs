using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.ClubModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.BL.Config
{
    public class ConfigBL : BaseBL,IConfigBL
    {
        private IClubRepo _clubRepo;

        public ConfigBL()
        {
            _clubRepo = Container.Resolve<IClubRepo>();
        }

        public async Task<List<AppConfig>> GetValueByKey(string request)
        {            
            return await _clubRepo.getValueByKeyList(new List<string>() { request });
        }

        public async Task<List<AppConfig>> GetValueByKeyList(List<string> request)
        {
            return await _clubRepo.getValueByKeyList(request);
        }
    }
}
