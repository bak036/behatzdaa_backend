using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class OrderTickets
    {
        public int OrderTicketId { get; set; }
        public int OrderId { get; set; }
        public Guid TicketGuid { get; set; }
        public int ExternalSystemTicketId { get; set; }
        public string Barcode { get; set; }
        public string Headlilne { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string VenueName { get; set; }
        public string RegionName { get; set; }
        public string Row { get; set; }
        public string Seat { get; set; }
        public string ShortMessager { get; set; }
        public int PriceLevelId { get; set; }
        public string PriceLlevelName { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }
        public string Area { get; set; }
        public string TourComment { get; set; }
        public bool? IsValid { get; set; }
        public DateTime? CancelTimeStamp { get; set; }
        public int? CancelRefId { get; set; }
        public int? Coins { get; set; }
        public int Price { get; set; }
        public int PriceId { get; set; }
        public string VariantFullBarcode { get; set; }

        public virtual Orders Order { get; set; }
    }
}
