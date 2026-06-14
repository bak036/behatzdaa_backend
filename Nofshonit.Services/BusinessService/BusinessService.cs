using Nofshonit.Common.DTOs.Business;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Services.BusinessService
{
    public class BusinessService: BaseService, IBusinessService
    {
        private IBusinessBL _businessBL;
        public BusinessService()
        {
            _businessBL = Container.Resolve<IBusinessBL>();
        }
        public List<BusinessDTO> GetBusinessByIds(List<long> businessIds)
        {
            return _businessBL.GetBusinessByIds(businessIds);
        }
        public List<BusinessSubBranchDTO> GetBusinessSubBranches(long businessId)
        {
            return _businessBL.GetBusinessSubBranches(businessId);
        }
    }
}
