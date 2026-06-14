using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Cards
{


    
    public class CardsDTO
    {
        public ActivitiesProperties properties { get; set; }
        public List<Row> rows { get; set; }
    }

    public class ActivitiesProperties
    {
        public List<ActivitiesData> Data { get; set; }
    }

    public class ActivitiesData
    {
        public string ErrorID { get; set; }
        public string TZ { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string SeriesPrefix { get; set; }
        public string SeriesName { get; set; }
        public string OrganizationName { get; set; }
    }

    public class Row
    {
        public string ErrorID { get; set; }
    }


}
