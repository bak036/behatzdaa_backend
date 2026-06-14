using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface IPurchaseService
    {
        Task<PurchaseHistoryResponseDTO> PurchaseHistory(MemberHistoryDTO memberHistoryDTO);
        string PurchaseHistoryOld();
        Task<string> SendOldOrdersLinkToUnauthorizedUser(string memberId);
    }
}
