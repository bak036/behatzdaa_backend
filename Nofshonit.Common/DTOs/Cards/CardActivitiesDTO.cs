using System;
using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Cards
{

    public class CardActivitiesDTO
    {
        public List<CardActivitiesData> CardActivitiesList{ get; set; }
    }

    public class CardActivities
    {
        public CardActivitiesProperties properties { get; set; }
        public CardActivitiesRow[] rows { get; set; }
    }

    public class CardActivitiesProperties
    {
        public CardActivitiesData[] Data { get; set; }
    }

    public class CardActivitiesData
    {
        public DateTime DateTime { get; set; }
        public string ActivityTypeName { get; set; }
        public string Amount { get; set; }
        public string BusinessName { get; set; }
        public string ChainName { get; set; }
        public string ActivityID { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class CardActivitiesRow
    {
        public string ErrorID { get; set; }
    }


}
