using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public string MemberId { get; set; }

        public string Type { get; set; }

        public DateTime OrderDate { get; set; }

    }
}
