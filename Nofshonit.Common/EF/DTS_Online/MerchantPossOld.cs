using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MerchantPossOld
    {
        public long Posid { get; set; }
        public long? MerchantId { get; set; }
        public string PosName { get; set; }
        public long? PosCity { get; set; }
        public string PosAddress { get; set; }
        public string PosPhone { get; set; }
        public string PosTerminalId { get; set; }
        public string Xmlparam { get; set; }
        public bool? Active { get; set; }
        public string MerchantUser { get; set; }
        public string MerchantPassword { get; set; }
        public int? PosType { get; set; }
        public string CityName { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; }
        public bool? ShowInWeb { get; set; }
        public string OpeningSchedule { get; set; }
        public string PrinterNum { get; set; }
    }
}
