using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Tags
    {
        public Tags()
        {
            TagsBusiness = new HashSet<TagsBusiness>();
            TagsCategory = new HashSet<TagsCategory>();
            TagsPremium = new HashSet<TagsPremium>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }
        public int TagType { get; set; }
        public bool TagEnable { get; set; }
        public DateTime? TagOpenDate { get; set; }
        public DateTime? TagCloseDate { get; set; }
        public int TagSort { get; set; }
        public int OrganizationId { get; set; }
        public string Description { get; set; }
        public string TagContext { get; set; }
        public int PremiumTypeId { get; set; }
        public bool? IsFilterEnabled { get; set; }
        public string FilterParameters { get; set; }

        public virtual Organizations Organization { get; set; }
        public virtual ICollection<TagsBusiness> TagsBusiness { get; set; }
        public virtual ICollection<TagsCategory> TagsCategory { get; set; }
        public virtual ICollection<TagsPremium> TagsPremium { get; set; }
    }
}
