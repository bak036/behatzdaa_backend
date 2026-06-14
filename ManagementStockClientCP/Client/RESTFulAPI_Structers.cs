using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementStockClient.Client
{
    public class PostResponse
    {
        public string JsonResult { get; set; }
        public string Exception { get; set; }
        public bool IsException { get; set; }
    }

    public enum EnumRestFulMethod
    {
        POST = 1,
        GET = 2,
        PUT = 3,
        DELETE = 4
    }

}
