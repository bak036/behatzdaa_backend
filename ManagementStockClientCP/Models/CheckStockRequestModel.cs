using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementStockClient.Models
{
    public class CheckStockRequestModel
    {
        public int OrgID { get; set; }
        public  List<VariantData> VariantData { get; set; }
    }
}