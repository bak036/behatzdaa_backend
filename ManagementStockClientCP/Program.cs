using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagementStockClient.Models;

namespace ManagementStockClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Api api = new Api();

            var res = api.UpdateStock(new CheckStockRequestModel()
            {
                OrgID = 102,
                VariantData = new List<VariantData>()
                {
                    new VariantData() {BenefitID = 227, Quantity = 6, FullBarcode = "100157-2"},
                    new VariantData() {BenefitID = 227, Quantity = 6, FullBarcode = "100157-1"},
                    new VariantData() {BenefitID = 227, Quantity = 6, FullBarcode = "100157-0"},

                }
            });


            string t = "";
        }
    }
}
