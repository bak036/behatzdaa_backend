using Nofshonit.Common.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Tags
{
    public class TagsCategoriesDTO
    {
        public string TagName { get; set; }

        public int TagId { get; set; }
        public List<CategotyTagInfoDTO> TagCategoryInfo { get; set; }
        
        public bool IsFilterEnabled { get; set; }
        public HashSet<int> FilterParameters { get; set; }
        public HashSet<decimal> Prices { get; set; }
        public HashSet<int> CityIds { get; set; }
        public HashSet<int> RegionIds { get; set; }
        public HashSet<DateRange> DateRanges { get; set; }
    }
}
