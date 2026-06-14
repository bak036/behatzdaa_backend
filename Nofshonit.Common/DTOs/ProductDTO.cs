using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
	public class ProductDTO
	{
		public string CategoryNumber { get; set; }
        public string ProductJsonForGA { get; set; }
        public string CategoryName { get; set; }

		public List<ShoppingBasketVariantDTO> Variants { get; set; }

		//public short SeatsStatus { get; set; }

		// Eventim
		public CreateReservationRequest CreateReservation { get; set; }
	}
}
