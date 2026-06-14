using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
    public interface IAddressesBL
    {

        BaseResponse<List<CityDTO>> GetCities();

        BaseResponse<List<StreetDTO>> GetCityStreets(int cityId);

        BaseResponse<List<RegionDTO>> GetRegions();
        BaseResponse<RegionDTO> GetRegionByCityId(int? cityId);
    }
}
