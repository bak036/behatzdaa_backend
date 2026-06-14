using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
	public class ShoppingBasketVariantDTO
	{
		public string Barcode { get; set; }

		public byte Quantity { get; set; }

		public int Price { get; set; }

		public byte BusinessSubTypeId { get; set; }
        public string ProductJsonForGA { get; set; }

        public ShoppingBasketVariantDTO() { }
	}
}
