using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.BL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.ScriptsApi
{
    [EnableCors("CorsPolicy")]
    [Route("api/scripts")]
    [ApiController]
    public class ScriptsController: BaseController
    {
        private UserBL  miluim = new UserBL();

        public ScriptsController()
        {
        }

        //[AllowAnonymous]
        [HttpGet("CreateUsers")]
        public bool CreateUsers()
        {
            var userCreated = miluim.CreateUsersScript(true).Result;

            return userCreated;
        }
    }
}
