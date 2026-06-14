using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Services.Base;
using System.Collections.Generic;

namespace Nofshonit.Services.SearchService
{
    public class SearchService : BaseService, ISearchService
    {

        private ISearchBL _searchBL;
        public SearchService() : base()
        {
            _searchBL = Container.Resolve<ISearchBL>();
        }

        public List<GetAutoCompleteResultsDTO> GetAutoCompleteResults(string text, int selectTop)
        {


            return _searchBL.GetAutoCompleteResults(text, selectTop);
        }
        public List<CategoryDetailsDTO> GetSearchData(string text, int selectTop, long superCategory, string region)
        {
            return _searchBL.GetSearchData(text, selectTop, superCategory, region);
        }

    }
}
