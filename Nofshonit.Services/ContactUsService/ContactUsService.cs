using Nofshonit.Common.DTOs;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nofshonit.Services.ContactUs
{
    public class ContactUsService : BaseService, IContactUsService
    {
        public string PathValueConfig { get; set; }

        public async Task<BaseResponse<object>> OpenServiceCaseRequest(ContactUsDTO contactUsDTO)
        {
           return await Container.Resolve<IContactUsBL>().OpenServiceCaseRequest(contactUsDTO);
        }

        public List<CrmGetTypeDTO> GetCrmTypes()
        {
            return Container.Resolve<IContactUsBL>().GetCrmTypes();
        }

    }
}