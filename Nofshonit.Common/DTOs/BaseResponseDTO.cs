using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class BaseResponseDTO
    {
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorId { get; set; }
    }
}
