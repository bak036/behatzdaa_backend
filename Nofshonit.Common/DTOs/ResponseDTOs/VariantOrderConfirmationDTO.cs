using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.ResponseDTOs
{
    public class VariantOrderConfirmationDTO
    {
        public string Barcode { get; set; }
        public int Qty { get; set; }
        public string Confirmation { get; set; }
    }
}
