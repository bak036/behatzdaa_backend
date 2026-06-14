using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class VariantVerifone
    {
        public int Id { get; set; }
        public int SerieId { get; set; }
        public int WalletId { get; set; }
        public int LoadingAmount { get; set; }
        public string Barcode { get; set; }
    }
}
