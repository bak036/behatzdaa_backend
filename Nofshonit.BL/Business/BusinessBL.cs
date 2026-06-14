using Nofshonit.Common.DTOs.Business;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Interfaces;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.BL.Business
{
    public class BusinessBL: BaseBL, IBusinessBL
    {
        private IMapperManager _mapper;
        private IDtsOnlineRepo _dtsOnlineRepo;
        public BusinessBL()
        {
            _mapper = Container.Resolve<IMapperManager>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
        }
        public List<BusinessDTO> GetBusinessByIds(List<long> businessIds)
        {
            return _dtsOnlineRepo.GetBusinessByIds(businessIds);
        }

        public List<BusinessSubBranchDTO> GetBusinessSubBranches(long businessId)
        {
            var result = _dtsOnlineRepo.BusinessSubBranches(new List<long> { businessId });
            List<BusinessSubBranch> model = new List<BusinessSubBranch>();
            result.TryGetValue(businessId, out model);
            return model.ConvertAll(x=>  (_mapper.Map(x, typeof(BusinessSubBranchDTO))) as BusinessSubBranchDTO);
        }
    }
}
