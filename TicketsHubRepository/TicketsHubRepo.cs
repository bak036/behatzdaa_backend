

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TicketsHubRepository.EF.TicketsHub;

namespace TicketsHubRepository
{
    public class TicketsHubRepo : ITicketsHubRepo
    {
        public List<OrderTickets> GetOrdersByEventGuid(string eventGuid, out int orderId)
        {
            List<OrderTickets> orderTickets = new List<OrderTickets>();
            orderId = 0;
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                Orders orderTicketHub;

                if (Guid.TryParse(eventGuid, out var eventGuidParsed))
                    orderTicketHub = thc.Orders.FirstOrDefault(x => x.OrderGuiud == eventGuidParsed);
                else
                    orderTicketHub = thc.Orders.FirstOrDefault(x => x.OrderGuiud.ToString() == eventGuid);

                if (orderTicketHub != null)
                {
                    orderTickets = thc.OrderTickets.Where(x => x.OrderId == orderTicketHub.OrderId).ToList();
                    orderId = orderTicketHub.OrderId;
                    return orderTickets;
                }
                return null;
            }
        }
        public TicketsHubRepository.EF.TicketsHub.Orders GetOrderByOrderGuiud(string orderGuiud)
        {
            try
            {
                using (TicketsHubContext thc = new TicketsHubContext())
                {
                    return thc.Orders.FirstOrDefault(x => x.OrderGuiud.ToString() == orderGuiud);
                }
            }
            catch
            {
                return null;
            }
        }
        public bool ValidateTicketHubEventBooked(string eventGuid)
        { 
            try
            {
                using (TicketsHubContext thc = new TicketsHubContext())
                {
                    Orders orderTicketHub;
                    if (Guid.TryParse(eventGuid, out var eventGuidParsed))
                        orderTicketHub = thc.Orders.FirstOrDefault(x => x.OrderGuiud == eventGuidParsed);
                    else
                        orderTicketHub = thc.Orders.FirstOrDefault(x => x.OrderGuiud.ToString() == eventGuid);
                    var isValid = orderTicketHub != null && orderTicketHub.IsBooked && orderTicketHub.OrderStatusId == 2;
                    return isValid;
                }
            }
            catch
            {
                return false;
            }
        }
        public List<OrderTickets> GetOrderTicketsByOrderId(int orderId)
        {
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                try
                {
                    return thc.OrderTickets.Where(x => x.OrderId == orderId).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
        public List<OrderTickets> GetOrderTickets(List<int> tickethubTicketIds)
        {
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                try
                {
                    return thc.OrderTickets.Where(x => tickethubTicketIds.Contains(x.OrderTicketId)).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

        public Events GetEvents(int eventId)
        {
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                return thc.Events.FirstOrDefault(x => x.EventId == eventId);
            }

        }

        public Orders GetOrders(int orderId)
        {
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                return thc.Orders.FirstOrDefault(x => x.OrderId == orderId);
            }
        }

        public Orders GetOrdersByEventGuid(string eventGuid)
        {
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                if (Guid.TryParse(eventGuid, out var eventGuidParsed))
                    return thc.Orders.FirstOrDefault(x => x.OrderGuiud == eventGuidParsed);

                return thc.Orders.FirstOrDefault(x => x.OrderGuiud.ToString() == eventGuid);
            }
        }

        public bool IsTicketshubOrderBooked(string eventGuid)
        {
            using (TicketsHubContext thc = new TicketsHubContext())
            {
                Orders orderTicketHub;

                if (Guid.TryParse(eventGuid, out var eventGuidParsed))
                    orderTicketHub = thc.Orders.FirstOrDefault(x => x.OrderGuiud == eventGuidParsed);
                else
                    orderTicketHub = thc.Orders.FirstOrDefault(x => x.OrderGuiud.ToString() == eventGuid);
                return orderTicketHub != null && orderTicketHub.IsBooked;
            }
        }

    }
}

