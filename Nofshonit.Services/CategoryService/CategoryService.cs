using Nofshonit.Common;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Nofshonit.Services.CategoryService
{
    public class CategoryService : BaseService, ICategoryService
    {

        private ICategoryBL _categoryBL;
		private ICacheManager _cacheManager;
        private readonly IContextManager _contextManager;

        public CategoryService()
        {
            _categoryBL = Container.Resolve<ICategoryBL>();
			_cacheManager = Container.Resolve<ICacheManager>();
            _contextManager = Container.Resolve<IContextManager>();
        }

        public List<CategoryHeaderDTO> GetCategoryHeader()
        {
			List<CategoryHeaderDTO> chdList = null;
			int key = _cacheManager.GenerateKey(new object[] { Container.Resolve<IContextManager>().CurrentOrganization().OrgId, _contextManager.CurrentUser().PremiumType});
			if (_cacheManager.Exists(key))
				return (List<CategoryHeaderDTO>)_cacheManager.Get(key);
			else
			{
				chdList = _categoryBL.GetCategoryHeader();
				_cacheManager.Set(key, chdList, new TimeSpan(0, Container.Resolve<IConfigurationManager>().GetConfigByValue<int>(ConfigurationKey.TimeSaveCache), 0));
			}
			return chdList;
        }

        public CategoryDetailsDTO GetCategoryDetails(long categoryId)
        {           
            CategoryDetailsDTO cdd = null;
            int key = _cacheManager.GenerateKey(new object[] { Container.Resolve<IContextManager>().CurrentOrganization().OrgId, categoryId,  _contextManager.CurrentUser().PremiumType });
			if (_cacheManager.Exists(key))
				return (CategoryDetailsDTO)_cacheManager.Get(key);
			else
			{
				cdd = _categoryBL.GetCategoryDetails(categoryId);
				_cacheManager.Set(key, cdd, new TimeSpan(0, Container.Resolve<IConfigurationManager>().GetConfigByValue<int>(ConfigurationKey.TimeSaveCache), 0));
			}
			return cdd;
        }

        public CategoryDetailsDTO GetCategoryProducts(long categoryId)
        {
			return _categoryBL.GetCategoryProducts(categoryId);
        }
    }
}
