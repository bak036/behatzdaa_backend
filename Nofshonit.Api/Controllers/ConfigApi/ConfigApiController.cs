using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.ConfigApi
{
    [Route("api/config")]
    [ApiController]
    public class ConfigApiController : BaseController
    {
        private readonly IConfigService _configService;

        public ConfigApiController()
        {
            _configService = Container.Resolve<IConfigService>();
        }
        [HttpPost("getValueByKeyList")]
        public async Task<BaseResponse<List<AppConfig>>> Purchase(List<string> request)
        {
            var response = new BaseResponse<List<AppConfig>>();
            var logItem = new LogDTO();

            try
            {
                response.Data = await _configService.GetValueByKeyList(request);                
            }

            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }

            return response;
        }
    }
}
