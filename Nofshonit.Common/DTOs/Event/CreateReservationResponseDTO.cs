using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class CreateReservationResponseDTO
    {
        public string OrderGuid { get; set; }
        public List<OrderTicketResponse> OrderTickets { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public bool IsSelfPrint { get; set; }
        public CreateReservationResponseDTO()
        { }
    }

    public class OrderTicketResponse
    {
        public int OrderTicketId { get; set; }
        public string TicketTypeName { get; set; }
        public string PriceLevelName { get; set; }
        public int Price { get; set; }
        public int? Coins { get; set; }
        public string Row { get; set; }
        public string Seat { get; set; }
        public string Area { get; set; }

        public string VariantFullBarcode { get; set; }
        public int PriceID { get; set; }
        public int TicketTypeId { get; set; }
        public int PriceLevelId { get; set; }

        public string VenuName { get; set; }
        public int OrderLimit { get; set; }
        public string ProductJsonForGA { get; set; }
    }

	public class OrderTicketResponseDTO : OrderTicketResponse
	{
		public OrderTicketResponseDTO() { }
		public OrderTicketResponseDTO(OrderTicketResponse ticket, string orderGuid,DateTime eventDate, string eventTime,bool isSelfPrint)
		{
			OrderGuid = orderGuid;
			OrderTicketId = ticket.OrderTicketId;
			TicketTypeName = ticket.TicketTypeName;
			Price = ticket.Price;
			Coins = ticket.Coins;
			Row = ticket.Row;
			Seat = ticket.Seat;
            Area = ticket.Area;
            VariantFullBarcode = ticket.VariantFullBarcode;
			PriceID = ticket.PriceID;
            PriceLevelName = ticket.PriceLevelName;
			TicketTypeId = ticket.TicketTypeId;
			PriceLevelId = ticket.PriceLevelId;
            VenuName = ticket.VenuName;
            EventDate = eventDate;
            EventTime = eventTime;
            IsSelfPrint = isSelfPrint;
        }
		public string OrderGuid { get; set; }
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventimTicketTypeName { get; set; } // Get from ProductVars (Variants)
        public bool IsSelfPrint { get; set; }
    }
}
