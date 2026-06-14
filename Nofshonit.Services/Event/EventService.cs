using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Services.Event
{
    public class EventService: BaseService, IEventService
    {
        public EventService() : base()
        {

        }

        public List<EventDTO> GetEventsByCategoryId(long categoryId)
        {
            return Container.Resolve<IEventBL>().GetEventsByCategoryId(categoryId);
        }

        public CatalogDTO EventCatalog(int eventId, long categoryId)
        {
            return Container.Resolve<IEventBL>().EventCatalog(eventId, categoryId);
        }

        public CreateReservationResponseDTO CreateReservation(CreateReservationRequest request)
        {
            return Container.Resolve<IEventBL>().CreateReservation(request);
        }
    }
}
