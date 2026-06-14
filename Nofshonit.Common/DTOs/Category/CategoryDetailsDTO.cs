using Nofshonit.Common.DTOs.Business;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Category
{
  
    public class CategoryDetailsDTO
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool isSelfPrint { get; set; }
        public long ParentId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public int CategoryDesign { get; set; }
        public int CategoryType { get; set; }
        public int? SortOrder { get; set; }

        public long BusinessId { get; set; }
        public string SupplierName { get; set; }
        public string CategoryHTML { get; set; }
        public string CategoryUrl { get; set; }
        public string TermsOfUse { get; set; }

        public string RedimType { get; set; }

        public string CampaignDetails { get; set; }

        public string Remarks { get; set; }
        public string MinimumInventoryForSale { get; set; }
        public string MustKnow { get; set; }
        
        public List<ImageDTO> Images { get; set; }

        public List<VariantDTO> Variants { get; set; }
        public List<EventDTO> Events { get; set; }

        public bool IsLeaf { get; set; }

        public bool? IsAllGrandchildren { get; set; }

        public bool IsEvents { get; set; }

        public List<CategoryDetailsDTO> SubCategories { get; set; }

        public List<LocationDto> Locations { get; set; }

        public BusinessDTO Business { get; set; }

        public List<SimpleLong> Breadcrumbs { get; set; }
        public List<SimpleLong> SameLevelCategories { get; set; }

		public bool IsConsumption { get; set; }
        public string ConcatForSearch { get; set; }
        public long SuperCategoryId { get; set; }
        public string Region { get; set; }
        public bool HaveSubBranch { get; set; }
        public long FirstFatherId { get; set; }
        public bool IsFilterEnabled { get; set; }   
        public HashSet<int> FilterParameters { get; set; }
        public HashSet<decimal> Prices { get; set; }
        public HashSet<int> CityIds { get; set; }
        public HashSet<int> RegionIds { get; set; }
        public HashSet<DateRange> DateRanges { get; set; }

    }   
    public class DateRange 
    { 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    } 

}
