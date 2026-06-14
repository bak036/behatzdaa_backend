using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofshonit.BL.Addresses
{
    public class AddressesBL : BaseBL, IAddressesBL
    {
        private IDtsOnlineRepo _dtsOnlineRepo;
        private ICacheManager Cache;

        public AddressesBL():base()
        {
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            Cache=Container.Resolve<ICacheManager>();
        }

      
        public BaseResponse<List<CityDTO>> GetCities()
        {
            var Cities =(List<CityDTO>) Cache.Get(CacheKeys.CitiesList);

            if (Cities == null)
            {
                Cities= _dtsOnlineRepo.GetCitiesList();
                Cache.Set(CacheKeys.CitiesList, Cities, TimeSpan.FromDays(1));              
            }
            return new BaseResponse<List<CityDTO>>
            {
                Status = true,
                Data = Cities.ToList()
            };
            
        }

		/// <summary>
		///  The method returns a list of all the streets of a city by the city id
		/// </summary>
		/// <param name="cityId">The City id number</param>
		/// <returns>
		///  Returns list of all the streets of a city by the city id
		/// </returns>
		public BaseResponse<List<StreetDTO>> GetCityStreets(int cityId)
        {
            var streets = (List<StreetDTO>)Cache.Get(string.Format( CacheKeys.CityStreetsList,cityId));
            if (streets == null)
            {
                streets = _dtsOnlineRepo.GetStreetsListById(cityId);
                Cache.Set(string.Format(CacheKeys.CityStreetsList, cityId), streets, TimeSpan.FromDays(1));
            }

            return new BaseResponse<List<StreetDTO>>
            {
                Status = true,
                Data = streets.ToList()
            };
        }
        public BaseResponse<RegionDTO> GetRegionByCityId(int? cityId)
        {
            
                if (cityId != null)
                {
                    var retRegion = new RegionDTO();

                    var regions = GetRegions();
                    var cities = GetCities();
                    var city = cities.Data.FirstOrDefault(c => c.CityId == cityId);
                    if (city != null)
                    {
                        retRegion = regions.Data.FirstOrDefault(r => r.RegionId == city.RegionID);
                        if (retRegion != null)
                        {

                            return new BaseResponse<RegionDTO>
                            {
                                Status = true,
                                Data = retRegion
                            };
                        }
                    }
                }
                return new BaseResponse<RegionDTO>
                {
                    Status = false,
                    Data = new RegionDTO()
                };
            
        }
        public BaseResponse<List<RegionDTO>> GetRegions()
        {
            var regions = (List<RegionDTO>)Cache.Get(CacheKeys.RegionsList);

            if (regions == null)
            {
                regions = _dtsOnlineRepo.GetRegionsList();
                Cache.Set(CacheKeys.RegionsList, regions, TimeSpan.FromDays(1));
            }
            return new BaseResponse<List<RegionDTO>>
            {
                Status = true,
                Data = regions.ToList()
            };
            
        }
    }
}
