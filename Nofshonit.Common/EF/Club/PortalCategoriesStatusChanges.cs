using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class PortalCategoriesStatusChanges
    {
        public long Id { get; set; }
        public long CategoryNumber { get; set; }
        public int StatusId { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime InsertDate { get; set; }
        public int? ReturnCode { get; set; }
        public string ReturnCodeDesc { get; set; }
        public string ReturnErrorName { get; set; }
    }
}
