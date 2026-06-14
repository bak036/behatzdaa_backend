using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
   public interface ISearchBL
    {

        List<GetAutoCompleteResultsDTO> GetAutoCompleteResults(string text, int selectTop);
        List<CategoryDetailsDTO> GetSearchData(string text, int selectTop, long superCategory, string region);
    }
}
