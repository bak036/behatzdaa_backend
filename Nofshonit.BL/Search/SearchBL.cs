using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Nofshonit.BL.Search
{
    public class SearchBL : BaseBL, ISearchBL
    {
        private ICategoryBL _categoryBL;
        private IClubRepo _clubRepo;
        private IDtsOnlineRepo _dtsOnlineRepo;
        private const string SEARCH_SCRIPT = "SearchByNameAndSuperCategory";
        public SearchBL() : base()
        {
            _categoryBL = Container.Resolve<ICategoryBL>();
            _clubRepo = Container.Resolve<IClubRepo>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
        }


        public List<GetAutoCompleteResultsDTO> GetAutoCompleteResults(string text, int selectTop)
        {
            return _dtsOnlineRepo.GetAutoCompleteData(text, selectTop);
        }

        public List<CategoryDetailsDTO> GetSearchData(string text, int selectTop, long superCategory, string region)
        {
            List<CategoryDetailsDTO> categoryDetailsList = new List<CategoryDetailsDTO>();
            try
            {
                categoryDetailsList = _dtsOnlineRepo.GetSearchData(text, selectTop, superCategory,region);
                if (categoryDetailsList.Count == 0)
                    return null;
                return categoryDetailsList;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Failed to Search Data for {text}, {superCategory}, {region}", ex);
            }
        }



    }
}
