using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Limitations
{
    public class VariantStockRequestDTO
    {
        public long? CategoryId { get; set; }
        public List<string> Barcodes { get; set; }
        public int? Qty  { get; set; }

    }
}
