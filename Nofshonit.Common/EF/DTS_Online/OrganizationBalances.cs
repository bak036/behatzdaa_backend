using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrganizationBalances
    {
        public long BalanceId { get; set; }
        public int OrganizationId { get; set; }
        public string BalanceName { get; set; }
        public int BalanceCurrency { get; set; }
        public int FieldNum { get; set; }
        public byte BalanceStatus { get; set; }
    }
}
