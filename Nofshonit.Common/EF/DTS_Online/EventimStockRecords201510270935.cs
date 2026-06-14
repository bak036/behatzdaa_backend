using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class EventimStockRecords201510270935
    {
        public int StockId { get; set; }
        public int EventimBusinessId { get; set; }
        public string EventimBuisnessName { get; set; }
        public int EventNo { get; set; }
        public int? EventTdlid { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? EventSalesStartDate { get; set; }
        public DateTime? EventSalesEndDate { get; set; }
        public int EventStatus { get; set; }
        public int? EventSeriesId { get; set; }
        public string EventSeriesTitle { get; set; }
        public int TicketPriceLevelCode { get; set; }
        public string TicketPriceLevelName { get; set; }
        public int NetCapacity { get; set; }
        public int Sold { get; set; }
        public int Remain { get; set; }
        public string Affiliate { get; set; }
        public string K4aeventNo { get; set; }
        public int EventimStockFileId { get; set; }
        public string BusinessId { get; set; }
        public DateTime? SendReportDate { get; set; }
        public bool? DisabledToOrder { get; set; }
    }
}
