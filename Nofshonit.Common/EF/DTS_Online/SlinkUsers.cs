using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SlinkUsers
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public DateTime InsertDate { get; set; }
        public string RestrictionPopulation { get; set; }
        public bool IsAdmin { get; set; }
    }
}
