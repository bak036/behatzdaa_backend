using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class CreateReservationRequest
    {
        public int EventId { get; set; }
        public int JourneyId { get; set; }
        public List<SeatObject> ListOfSeats { get; set; }
    }

    public class CreateReservationRequestDTO: CreateReservationRequest
    {
        public CreateReservationRequestDTO(CreateReservationRequest b)
        {
            this.EventId = b.EventId;
            this.JourneyId = b.JourneyId;
            this.ListOfSeats = b.ListOfSeats;
        }
        public string MemberId { get; set; }
        public int OrganizationId { get; set; }
      
    }

    public class SeatObject
    {
        public string VariantFullBarcode { get; set; }
        public string Area { get; set; }
        public string Row { get; set; }
        public string Seat { get; set; }
        public int SeatID { get; set; }
        public string MetaData { get; set; }
        public string TicketTypeName { get; set; }
        public string PriceLevelName { get; set; }
        public int TicketTypeId { get; set; }
        public int PriceLevelId { get; set; }
        public int PriceID { get; set; }
        public int? Coins { get; set; }
        public int Price { get; set; }
        public string ProductJsonForGA { get; set; }
    }
}
