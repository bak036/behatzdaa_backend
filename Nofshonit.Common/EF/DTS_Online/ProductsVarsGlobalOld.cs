using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ProductsVarsGlobalOld
    {
        public int Id { get; set; }
        public string FullBarCode { get; set; }
        public string BusinessId { get; set; }
        public string VarNameGlobal { get; set; }
        public DateTime DtCreatedDate { get; set; }
        public string BusinessList { get; set; }
        public decimal? CupaPrice { get; set; }
        public DateTime? CupaPriceUpdateDate { get; set; }
        public string ShortNameVar { get; set; }
        public string VarName { get; set; }
    }
}
