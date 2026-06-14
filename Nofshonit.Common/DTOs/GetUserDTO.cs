using Nofshonit.Common.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
	public class GetUserDTO
	{
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public DateTime BirthDate { get; set; }

		public string PhoneNumber { get; set; }

		public string City { get; set; }

		public string Street { get; set; }

		public string HouseNumber { get; set; }

		public string ApartmentNumber { get; set; }

		public string PostalCode { get; set; }

		public string WorkPlaceCity { get; set; }

		public int NumberOfChildren { get; set; }

		public string PartnerEmail { get; set; }

		public EGender Gender { get; set; }
	}
}
