using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ShortCardNumbersBank
    {
        public int ShortNumber { get; set; }
        public DateTime InsertDate { get; set; }
        public Guid? BulkIdentifier { get; set; }
        public Guid ShortNumberOrderGuid { get; set; }
    }
}
