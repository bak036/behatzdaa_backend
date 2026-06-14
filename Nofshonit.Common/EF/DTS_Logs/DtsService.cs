using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class DtsService
    {
        public DtsService()
        {
            DataCenter = new HashSet<DataCenter>();
        }

        public int DtsServiceId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<DataCenter> DataCenter { get; set; }
    }
}
