using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcSeries
    {
        public MwcSeries()
        {
            MwcRcn = new HashSet<MwcRcn>();
        }

        public int SerieId { get; set; }
        public DateTime DateCreated { get; set; }
        public int OrganizationId { get; set; }
        public short? IssuerId { get; set; }
        public short SerieForIssuerId { get; set; }
        public string CardsPrefix { get; set; }
        public string ConstantTemplate { get; set; }
        public bool MoneySerie { get; set; }
        public int? VerId { get; set; }
        public string SerieName { get; set; }
        public string DescriptionSerie { get; set; }
        public bool Active { get; set; }
        public int? Opid { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public int? AccountId { get; set; }
        public bool IsShowShortNumberOnReport { get; set; }
        public bool? IsShowLongNumberOnReport { get; set; }
        public bool IsPhsyicalMedia { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual ICollection<MwcRcn> MwcRcn { get; set; }
    }
}
