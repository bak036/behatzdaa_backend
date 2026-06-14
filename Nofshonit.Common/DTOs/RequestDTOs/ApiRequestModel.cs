using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.RequestDTOs
{
    public class ApiRequestModel
    {
        public string baseUrl { get; set; }
        public string relativeUrl { get; set; }
        public EHttpRequestType method { get; set; }
        public object data { get; set; } = default;
        public Dictionary<string, string> headers { get; set; } = null;
    }
}
