using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nofshonit.Services.AddressesService
{
    public class AddressesService : BaseService,IAddressesService
    {
        private readonly IAddressesBL _addressBL;

        public AddressesService()
        {
            _addressBL = Container.Resolve<IAddressesBL>();
        }

        public BaseResponse<List<CityDTO>> GetCities()
        {
			var cities = _addressBL.GetCities();
			cities.Data = cities.Data.Where(c => c.GovId != 0).ToList();
			return cities;
		}

        public BaseResponse<List<StreetDTO>> GetCityStreets(int cityId)
        {
            return _addressBL.GetCityStreets(cityId);
        }

        public BaseResponse<List<RegionDTO>> GetRegions()
        {
            return _addressBL.GetRegions();
        }
    }
}
