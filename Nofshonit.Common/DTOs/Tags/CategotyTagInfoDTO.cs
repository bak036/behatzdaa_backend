using Nofshonit.Common.DTOs.Category;
using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Tags
{
    public class CategotyTagInfoDTO
    {
        public int CategoryId { get; set; }
        public int CategoryTagSort { get; set; }
        public CategoryDetailsDTO Categories { get; set; }
    }
}