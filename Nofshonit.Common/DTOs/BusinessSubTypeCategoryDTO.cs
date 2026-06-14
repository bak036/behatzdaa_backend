using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
   
    public class BusinessSubTypeCategoryDTO
    {
        public int BusinessSubTypeId { get; set; }
        public string BusinessSubTypeName { get; set; }
        public int Limit { get; set; }
        public int? MonthlyLimit { get; set; }
        public int? YearlyLimit { get; set; }
    }
}
