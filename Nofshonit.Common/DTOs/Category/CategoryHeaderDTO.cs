using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Category
{

    public class CategoryHeaderDTO
    {
        public long CategoryId { get; set; }      
        public string CategoryName { get; set; }
        public int? SortOrder { get; set; }
        public List<CategoryHeaderItemDTO> Children { get; set; }
        public List<ImageDTO> Image { get; set; }
        public bool? IsAllGrandchildren { get; set; }
        public int CategoryType { get; set; }
        public bool IsLeaf { get; set; }
        public string CategoryUrl { get; set; }
    }

    public class CategoryHeaderItemDTO
    {       
        public long CategoryId { get; set; }
        public long ParentId { get; set; }
        public string CategoryName { get; set; }
        public int? SortOrder { get; set; }
        public bool HasChildren { get; set; }
        public bool IsAllGrandchildren { get; set; }
        public int CategoryType { get; set; }
        public string CategoryUrl { get; set; }
    }
}
