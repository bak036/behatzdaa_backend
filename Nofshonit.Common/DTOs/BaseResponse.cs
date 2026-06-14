using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class BaseResponse<T>
    {      
        public bool Status { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public int? ErrorId { get; set; }

        public string ErrorDescription { get; set; }
        public string ErrorHTML { get; set; }
    }
}
