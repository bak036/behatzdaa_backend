using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SoGood
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string CityName { get; set; }
        public string Area { get; set; }
        public string StoreAddress { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public long? Posid { get; set; }
    }
}
