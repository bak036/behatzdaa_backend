using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class BenefitFileLog
    {
        public int BenefitFileLogId { get; set; }
        public DateTime InsertDate { get; set; }
        public long CategoryNumber { get; set; }
    }
}
