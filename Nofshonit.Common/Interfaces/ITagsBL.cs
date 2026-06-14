using Nofshonit.Common.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface ITagsBL
    {
        Task<List<TagsCategoriesDTO>> GetCategoryByTags(int selectTop, int skipTags = 0);
        TagsCategoriesDTO GetCategorysByTagID(int tagId);
    }
}
