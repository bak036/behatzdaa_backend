using Nofshonit.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
   public interface IContactUsBL
    {
        Task<BaseResponse<object>> OpenServiceCaseRequest(ContactUsDTO contactUsDTO);

        List<CrmGetTypeDTO> GetCrmTypes();
    }
}
