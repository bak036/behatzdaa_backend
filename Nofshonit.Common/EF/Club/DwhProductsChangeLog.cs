using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class DwhProductsChangeLog
    {
        public long RowId { get; set; }
        public string DataBaseName { get; set; }
        public string ProductId { get; set; }
        public int? ProductProviderId { get; set; }
        public double? CostPrice { get; set; }
        public double? TransferPrice { get; set; }
        public int? Inactive { get; set; }
        public DateTime? Created { get; set; }
    }
}
