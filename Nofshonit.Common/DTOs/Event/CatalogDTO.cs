using Nofshonit.Common.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Event
{
    public class CatalogDTO
    {
        public CategoryDetailsDTO Category {get;set;}

        public string MapHTML { get; set; }
        public List<EventCatalogResponseDTO> EventCatalogList { get; set; }

        public int JourneyId { get; set; }
    }
}
