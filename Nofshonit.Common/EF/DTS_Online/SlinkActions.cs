using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SlinkActions
    {
        public int Id { get; set; }
        public int ActionId { get; set; }
        public int RequestId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string ActionValue { get; set; }
    }
}
