using Nofshonit.Common.DTOs.RequestDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.BL.RestApiGW
{
    public interface IRestApiGW
    {
        T ApiRequest<T>(ApiRequestModel apiRequestModel);
    }
}
