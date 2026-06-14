using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class KeyValueList
    {
        public string ListId { get; set; }
        public string ListText { get; set; }
        public string ListValue { get; set; }
        public bool? IsArchived { get; set; }
        public int Id { get; set; }
    }
}
