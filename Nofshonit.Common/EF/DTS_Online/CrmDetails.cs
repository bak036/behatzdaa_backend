using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CrmDetails
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int CrmId { get; set; }
        public int OpId { get; set; }
        public DateTime Date { get; set; }
    }
}
