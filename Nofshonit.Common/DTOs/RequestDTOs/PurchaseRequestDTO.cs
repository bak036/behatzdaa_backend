using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.RequestDTOs
{
    public class PurchaseRequestDTO
    {
        public string UniqueId { get; set; }
        public string MemberId { get; set; }
        public string OrderGuid { get; set; }
        public string EventsGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string identity { get; set; }

        public int? City { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber {get;set;}
        public string ZipCode { get; set; }
        public string Entrance { get; set; }
        public string Mailbox { get; set; }

        public string CreditCardExpirey { get; set; }
        public string CreditCard16Digits { get; set; }
        public string Cvv { get; set; }
        public string PinCode { get; set; }
        public decimal TotalPayment { get; set; }
        public int NumOfPayments { get; set; }
        public List<CartItem> Cart { get; set; }
        public List<CartVarsDTO> ShoppingBasketCart { get; set; }
        public string CreditCardClub { get; set; }
        public int CreditCardStatus { get; set; }
        public bool PayWithRegularCard { get; set; }

        public class CartItem
        {
            public long CategoryId { get; set; }
            public string CategoryName { get; set; }
            public List<Variant> Variants { get; set; }
        }
        public class Variant {
            public int Quantity { get; set; }
            public string Barcode { get; set; }
            public int Price { get; set; }
            public int RegularPrice { get; set; }

        }
    }
}
