using Nofshonit.Common.DTOs.Tags;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nofshonit.BL.Tags
{
    public class TagsBL : BaseBL, ITagsBL
    {
        private IDtsOnlineRepo _dtsOnlineRepo;
        private IClubRepo _clubRepo;

        public TagsBL()
        {
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _clubRepo = Container.Resolve<IClubRepo>();
        }

        public async Task<List<TagsCategoriesDTO>> GetCategoryByTags(int selectTop,int skipTags = 0)
        {
           
            return _dtsOnlineRepo.GetTagsByTop(selectTop,skipTags);
        }

        public TagsCategoriesDTO GetCategorysByTagID(int tagId)
        {
            return _dtsOnlineRepo.GetTagsById(tagId);
        }
    }
}
