using Nofshonit.Common.DTOs.Tags;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Services.TagsService
{
    public class TagsService : BaseService, ITagsService
    {
        private ITagsBL _tagsBl;
       private ICacheManager _cacheManager ;
        private readonly IContextManager _contextManager;
        public TagsService() : base()
        {
            _tagsBl = Container.Resolve<ITagsBL>();
            _cacheManager = Container.Resolve<ICacheManager>();
            _contextManager = Container.Resolve<IContextManager>();
        }

        public Task<List<TagsCategoriesDTO>> GetCategoryByTags(int selectTop,int skipTags = 0)
        {
            Task<List<TagsCategoriesDTO>> tcdList = null;

            int key = _cacheManager.GenerateKey(new object[] { Container.Resolve<IContextManager>().CurrentOrganization().OrgId, selectTop, _contextManager.CurrentUser().PremiumType });
            if (_cacheManager.Exists(key))
                return (Task<List<TagsCategoriesDTO>>)_cacheManager.Get(key);
            else
            {
                tcdList = _tagsBl.GetCategoryByTags(selectTop,skipTags);
                _cacheManager.Set(key, tcdList);
            }

            return tcdList;
            
        }

        public TagsCategoriesDTO GetCategorysByTagID(int tagId)
        {
            TagsCategoriesDTO tcd = null;
           
            int key = _cacheManager.GenerateKey(new object[] { Container.Resolve<IContextManager>().CurrentOrganization().OrgId, tagId, _contextManager.CurrentUser().PremiumType });
            if (_cacheManager.Exists(key))
                return (TagsCategoriesDTO)_cacheManager.Get(key);
            else
            {
                tcd = _tagsBl.GetCategorysByTagID(tagId);
                _cacheManager.Set(key, tcd);
            }

            return tcd;
           
        }
    }
}
