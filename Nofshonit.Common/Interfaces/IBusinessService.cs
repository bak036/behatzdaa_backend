using Nofshonit.Common.DTOs.Business;
using Nofshonit.Common.EF.DTS_Online;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
    public interface IBusinessService
    {
        List<BusinessDTO> GetBusinessByIds(List<long> businessIds);
        List<BusinessSubBranchDTO> GetBusinessSubBranches(long businessId);
    }
}
