using Nofshonit.Common.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryHeaderDTO> GetCategoryHeader();
        CategoryDetailsDTO GetCategoryDetails(long categoryId);
        CategoryDetailsDTO GetCategoryProducts(long categoryId);
        
    }
}
