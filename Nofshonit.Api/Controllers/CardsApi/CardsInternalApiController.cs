using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace Nofshonit.Api.Controllers.CardsApi
{
    [EnableCors("CorsPolicy")]
    [Authorize]
    [Route("api/contact")]
    [ApiController]
    public class CardsInternalApiController : Controller
    {
    
    }
}
