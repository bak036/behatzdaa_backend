using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Constants
{
    public static class TicketHubClientKeys
    {
        public static string GetActiveEvents = "api/TicketsHub/GetActiveEvents";
        public static string EventCatalog = "api/TicketsHub/GetCatalogForMember";
        public static string CreateReservation = "api/TicketsHub/CreateReservation";
        public static string GetMapHtml = "api/TicketsHub/GetMapHtml";
        public static string BookOrder = "api/TicketsHub/BookOrder";
        public static string CancelOrder = "api/TicketsHub/CancelOrder";
    }
}
