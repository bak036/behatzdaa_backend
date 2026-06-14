using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PurchaseRestriction
    {
        public int TableUiId { get; set; }
        public int RowGuid { get; set; }
        public int OrgId { get; set; }
        public int CustomersTypeid { get; set; }
        public int Moneytypeid { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Restrictionid { get; set; }
    }
}
