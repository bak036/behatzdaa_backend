using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class TitanCinemas
    {
        public TitanCinemas()
        {
            Business = new HashSet<Business>();
        }

        public string CinemaName { get; set; }
        public byte CinemaId { get; set; }
        public string DtsType { get; set; }
        public string DefaultWeekdayOrallweek { get; set; }
        public string StartHighRatesDay { get; set; }
        public string EndHighRatesDay { get; set; }
        public byte? StartHighRatesHoursTime { get; set; }
        public byte? StartHighRatesMinutesTime { get; set; }
        public byte? EndHighRatesHoursTime { get; set; }
        public byte? EndHighRatesMinutesTime { get; set; }

        public virtual ICollection<Business> Business { get; set; }
    }
}
