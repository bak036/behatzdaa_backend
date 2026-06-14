using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.Product;
using System;
using System.Collections.Generic;

namespace Nofshonit.Common
{
	public class CartVarsDTO
	{
		public string CategoryId { get; set; }

		public string CategoryName { get; set; }
        public string ProductJsonForGA { get; set; }

        public string SupplierName { get; set; }

        public bool? AllowPriceZero { get; set; }

        public byte Quantity { get; set; }

		public int? FinalPrice { get; set; }

		public DateTime? EventDate { get; set; }

        public DateTime ExpireDate { get; set; }

		public string ShortDescription { get; set; }

		public VariantDTO Variant { get; set; }

        public List<OrderTicketResponseDTO> Tickets { get; set; }
    }

    class EventsTicketsDTO
    {
        public int OrderTicketId { get; set; }
        public string TicketTypeName { get; set; }
        public string PriceLevelName { get; set; }
        public int Price { get; set; }
        public int? Coins { get; set; }
        public string Row { get; set; }
        public string Seat { get; set; }
        public string VariantFullBarcode { get; set; }
        public int PriceID { get; set; }
        public int TicketTypeId { get; set; }
        public int PriceLevelId { get; set; }
        public int OrderLimit { get; set; }
        public int VenuName { get; set; }
        public int EventDate { get; set; }
        public int EventTime { get; set; }
        public int EventimTicketTypeName { get; set; } // Get from ProductVars (Variants)
        public string OrderGuid { get; set; }
    }
}
