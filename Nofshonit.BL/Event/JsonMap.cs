using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.BL.Event
{



    public class JsonMap
    {
        public int SystemID { get; set; }
        public int EventID { get; set; }
        public string Currency { get; set; }
        public Customprices CustomPrices { get; set; }
    }

    public class Customprices
    {
        public Price[] Prices { get; set; }
        public Pricelevel[] PriceLevels { get; set; }
        public Creditlimit[] CreditLimits { get; set; }
        public Clienteventactivation[] ClientEventActivations { get; set; }
        public object[] MemberCardLinks { get; set; }
    }

    public class Price
    {
        public int PriceID { get; set; }
        public int OutletFee { get; set; }
        public int BasicPrice { get; set; }
        public int EndPrice { get; set; }
        public int BasicPriceVATKey { get; set; }
        public string PriceType { get; set; }
        public int SystemFeeVATKey { get; set; }
        public int ForeignID { get; set; }
    }

    public class Pricelevel
    {
        public int PriceLevelID { get; set; }
        public int Number { get; set; }
        public int ForeignID { get; set; }
        public string Text { get; set; }
        public string Currency { get; set; }
        public int PriceLevelPrice { get; set; }
        public Promotion[] Promotions { get; set; }
    }

    public class Promotion
    {
        public int PromotionID { get; set; }
        public int PriceLevelPriceMin { get; set; }
        public int PriceLevelPriceMax { get; set; }
        public Hold[] Holds { get; set; }
    }

    public class Hold
    {
        public int Number { get; set; }
        public Freeseats FreeSeats { get; set; }
        public Tickettype[] TicketTypes { get; set; }
    }

    public class Freeseats
    {
        public int PriceLevelID { get; set; }
        public int TotalSeats { get; set; }
        public int TotalFreeSeats { get; set; }
        public int MaxFreeConnectedSeats { get; set; }
        public string SeatMapSeatType { get; set; }
    }

    public class Tickettype
    {
        public int TicketTypeID { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
        public int ForeignID { get; set; }
        public int MinTicketsPerOrder { get; set; }
        public int MaxTicketsPerOrder { get; set; }
        public string Code { get; set; }
        public string ItalyFiscalCode { get; set; }
        public int OnlyInGroupsOfPerOrder { get; set; }
        public Flags Flags { get; set; }
        public int PriceID { get; set; }
    }

    public class Flags
    {
    }

    public class Creditlimit
    {
        public string Currency { get; set; }
        public bool NoCreditLimitCheck { get; set; }
    }

    public class Clienteventactivation
    {
        public int ClientID { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string SalesType { get; set; }
        public int MaxTicketsPerOrder { get; set; }
        public bool ShowHolds { get; set; }
        public string ActivatedHolds { get; set; }
        public int MaxReservationMinutes { get; set; }
        public Flags1 Flags { get; set; }
    }

    public class Flags1
    {
        public bool WillCallCreate { get; set; }
        public bool WillCallPrint { get; set; }
        public bool WillCallHijack { get; set; }
        public bool TicketTypeChangeable { get; set; }
        public bool ReseatAllowed { get; set; }
        public bool BestSeatAvoidGapSeats { get; set; }
        public bool BestSeatArmageddon { get; set; }
    }
}