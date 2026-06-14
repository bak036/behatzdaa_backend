using Microsoft.AspNetCore.Http;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Infrastructure.Utils
{
    public static class IpAddressService
    {
        public static string GetUserIP(this IHeaderDictionary headers)
        {
            string ipList = headers["X-Forwarded-For"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
