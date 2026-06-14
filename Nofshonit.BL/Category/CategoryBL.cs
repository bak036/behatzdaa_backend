using Microsoft.AspNetCore.Http;
using Nofshonit.Common;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Nofshonit.BL.Category
{
    public class CategoryBL: BaseBL, ICategoryBL
    {
        IDtsOnlineRepo _dtsOnlineRepo;
        private IClubRepo _clubRepo;
        private readonly ICacheManager _cacheManager;
        private int organizationId;
        private ICardsBL _cardsBL;
        private IConfigurationManager _configuration;
        private readonly IContextManager _contextManager;
        public CategoryBL()
        {
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _clubRepo = Container.Resolve<IClubRepo>();
            _cacheManager = Container.Resolve<ICacheManager>();
            organizationId = ContextManager.CurrentOrganization().OrgId;
            _cardsBL = Container.Resolve<ICardsBL>();
            _configuration = Container.Resolve<IConfigurationManager>();
            _contextManager = Container.Resolve<IContextManager>();
        }

		public List<CategoryHeaderDTO> GetCategoryHeader()
		{
			return _dtsOnlineRepo.GetCategoryHeader(organizationId);
		}
        /*
        public List<CategoryDetailsDTO> GetCategoryDetails(long categoryId)
        {
            var level = _dtsOnlineRepo.CategoryLevel(categoryId, organizationId);
            
            var result = _dtsOnlineRepo.GetCategoryDetails(categoryId, level, organizationId);
            if(result != null && level == 2)
            {
                var current = result.FirstOrDefault(x => x.CategoryNumber == categoryId);
                current.Variants = Container.Resolve<IProductService>().GetVariantsByCategoryNumber(categoryId);
            }
            return result;
        }*/

        public CategoryDetailsDTO GetCategoryDetails(long categoryId)
        {
            var level = 1;// _dtsOnlineRepo.CategoryLevel(categoryId, organizationId);

            var result = _dtsOnlineRepo.GetCategoryDetails(categoryId, level, organizationId);
           
            return result;
        }
        public CategoryDetailsDTO GetCategoryProducts(long categoryId)
        {
            Container.Resolve<ILimitationsBL>().ValidateCategoryForMemberLimitations(categoryId, true);
            //var level = _dtsOnlineRepo.CategoryLevel(categoryId, organizationId);
            var level = 2;
            CategoryDetailsDTO result;
            int key = _cacheManager.GenerateKey(new object[] { organizationId, categoryId, _contextManager.CurrentUser().PremiumType });
            if (_cacheManager.Exists(key))
                result = (CategoryDetailsDTO)_cacheManager.Get(key);
            else
            {
                result = _dtsOnlineRepo.GetCategoryDetails(categoryId, level, organizationId);
                _cacheManager.Set(key, result, new TimeSpan(0, _configuration.GetConfigByValue<int>(ConfigurationKey.TimeSaveCache), 0));
            }
            
            if (result != null)
            {
                //List<CartVarsDTO> varList = Container.Resolve<IShopingBasketBL>().GetCartVars(Container.Resolve<IShopingBasketBL>().GetCart().Result);
                //var cardLimits = Container.Resolve<ILimitationsBL>().ValidatePurchesAllowed(varList).ToDictionary(x=>x.Barcode,x=>x.OrderLimit);
                if (!result.IsEvents)
                {
                    var variants = Container.Resolve<IProductService>().GetVariantsByCategoryId(categoryId);
                    //var varWithCards = variants.Where(x => cardLimits.ContainsKey(x.BarCode)).ToList();
                    //foreach(var varWithCard in varWithCards)
                    //{
                    //    var limit = varWithCard.OrderLimit;
                    //    cardLimits.TryGetValue(varWithCard.BarCode, out limit);
                    //    varWithCard.OrderLimit = limit;
                    //}
                    result.Variants = variants;
                }
                else
                {
                    result.Events = Container.Resolve<IEventService>().GetEventsByCategoryId(categoryId);
                }
                                    
            }
            return result;
        }
    }
}
