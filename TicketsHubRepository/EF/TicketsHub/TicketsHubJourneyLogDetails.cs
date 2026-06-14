using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class TicketsHubJourneyLogDetails
    {
        public int TicketsHubJourneyLogDetailsId { get; set; }
        public string Comment { get; set; }
        public DateTime? Timestamp { get; set; }
        public bool IsSuccess { get; set; }
        public string ExceptionInfo { get; set; }
        public string StackTrace { get; set; }
        public int? TicketsHubJourneyLogId { get; set; }
    }
}
