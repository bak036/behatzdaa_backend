using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface IEventBL
    {
        List<EventDTO> GetEventsByCategoryId(long categoryId);
        CatalogDTO EventCatalog(int eventId, long categoryId);

        CreateReservationResponseDTO CreateReservation(CreateReservationRequest request);
        ShellTicketHub<object> BookOrder(string eventGuid);
        ShellTicketHub<object> CanselOrder(string eventGuid);
    }
}
