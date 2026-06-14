using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{

    public class GetEventsByBenefitIdResponse
    {

        public int Status { get; set; }

        public string ErrorMessage { get; set; }


        public int? ErrorId { get; set; }

        public string MemberId { get; set; }


        public string BenefitId { get; set; }

        public int DtsId { get; set; }

        public List<EventByBenefitIdResponse> Events { get; set; } = new List<EventByBenefitIdResponse>();

    }

    public class EventByBenefitIdResponse
    {
        public string Name { get; set; }

        public string VenueName { get; set; }

        public int VenueId { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public int EventId { get; set; }
        public DateTime ExpireDate { get; set; }
        public int IsSendToFriend { get; set; }
        public int OrderLimit { get; set; }
        public int BenefitTypeId { get; set; }
        public string IframeUrl { get; set; }

        public bool IsCampaign { get; set; }
    }
}
