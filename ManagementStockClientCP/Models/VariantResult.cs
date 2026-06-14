using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagementStockClient.Models
{
    public class VariantResult
    {
        public enum ResultCodeEnum
        {
            SUCCESS=1,
            VariantNotExists=2,
            CouponStockIsEmpty = 3,
            DailyStockIsEmpty = 4,
            VariantStockIsEmpty = 5,
        }

        public ResultCodeEnum ResultCode { get; set; }
        public string FullBarcode { get; set; }
        public int? Limit { get; set; }
    }
}