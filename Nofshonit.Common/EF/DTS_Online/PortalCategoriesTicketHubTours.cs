using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PortalCategoriesTicketHubTours
    {
        public int Id { get; set; }
        public long CategoryNumber { get; set; }
        public int TicketHubTourNumber { get; set; }
        public string TourTitle { get; set; }
        public string TourComment { get; set; }
    }
}
