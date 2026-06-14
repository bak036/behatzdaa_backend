using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface IConfigService
    {
        Task<List<AppConfig>> GetValueByKeyList(List<string> request);
        Task<List<AppConfig>> GetValueByKey(string request);
    }
}
