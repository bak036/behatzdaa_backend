using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Histadrut
{
    public partial class DtsService
    {
        public DtsService()
        {
            DataCenter = new HashSet<DataCenter>();
            DataCenterArchaive = new HashSet<DataCenterArchaive>();
        }

        public int DtsServiceId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public virtual ICollection<DataCenter> DataCenter { get; set; }
        public virtual ICollection<DataCenterArchaive> DataCenterArchaive { get; set; }
    }
}
