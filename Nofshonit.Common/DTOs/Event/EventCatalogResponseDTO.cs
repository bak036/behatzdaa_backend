using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class EventCatalogResponseDTO
    {
        public string FullBarCode { get; set; }
        public string VarName { get; set; }
        public bool IsSendToFriend { get; set; }
        public int OrderLimit { get; set; }
        public decimal FinalPrice { get; set; }
        public int? Coins { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? RedimTypeId { get; set; }
        public string RedimTypeName { get; set; }
        public int? TicketTypeId { get; set; }
        public int? PriceLevelId { get; set; }
        public int? EventimPriceId { get; set; }
        public int EventId { get; set; }
        public int TourId { get; set; }
        public string MapJson { get; set; }
        public DateTime? EventDate { get; set; }
        public string EventTime { get; set; }
        public int EventSeriesId { get; set; }
        public int VenueId { get; set; }
        public string VeneueName { get; set; }
        public bool IsSeatMap { get; set; }
        public string EventimPriceLevelName { get; set; }
        public string EventimTicketTypeName { get; set; }
        public string Title { get; set; }
        public bool IsSelfPrint { get; set; }
        public bool IsCampaign { get; set; }
        public string CategoryDescription { get; set; }
        public decimal CupaPrice { get; set; }
    }
}
