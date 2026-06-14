using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Stock
{
    public class CheckStockRequestModel
    {
        public int OrgID { get; set; }
        public List<VariantData> VariantData { get; set; }
    }
}
