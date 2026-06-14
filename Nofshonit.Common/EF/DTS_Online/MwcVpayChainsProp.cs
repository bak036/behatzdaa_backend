using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayChainsProp
    {
        public int ChainId { get; set; }
        public decimal? PercentReduction { get; set; }
        public int? BankCode { get; set; }
        public int? BankBranchCode { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string EmailContact { get; set; }
        public decimal? PercentReductionHashmal { get; set; }
        public string BuisnessId { get; set; }
        public string LogoUrl { get; set; }
        public string ShufersalLogo { get; set; }
    }
}
