using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
    public class BarCodeDTO
    {
        public string BarCode { get; set; }
        public string CardNumber { get; set; }
        public short CVV { get; set; }
        public string ExpirationDate { get; set; }
    }
}
