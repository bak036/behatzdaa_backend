using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.DTS_Online;
using System.Collections.Generic;

namespace Nofshonit.Common.Interfaces
{
    public interface IAddressesService
    {
        BaseResponse<List<CityDTO>> GetCities();

        BaseResponse<List<StreetDTO>> GetCityStreets(int cityId);

        BaseResponse<List<RegionDTO>> GetRegions();
    }
}
