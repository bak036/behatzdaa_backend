using System;
using System.Collections.Generic;
using TicketsHubRepository.EF.TicketsHub;

namespace TicketsHubRepository
{
    public interface ITicketsHubRepo
    {
        List<OrderTickets> GetOrdersByEventGuid(string eventGuid, out int orderId);
        List<OrderTickets> GetOrderTickets(List<int> tickethubTicketIds);
        bool ValidateTicketHubEventBooked(string eventGuid);
        Orders GetOrders(int orderId);
        Events GetEvents(int eventId);
        Orders GetOrdersByEventGuid(string eventGuid);
        bool IsTicketshubOrderBooked(string eventGuid);
        TicketsHubRepository.EF.TicketsHub.Orders GetOrderByOrderGuiud(string orderGuiud);
        List<OrderTickets> GetOrderTicketsByOrderId(int orderId);
    }
}
