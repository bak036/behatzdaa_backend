using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{
    public class ResponseData<T>
    {
        public T data { get; set; }
    }
}
