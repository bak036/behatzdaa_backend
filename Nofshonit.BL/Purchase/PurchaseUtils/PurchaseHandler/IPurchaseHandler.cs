using Nofshonit.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nofshonit.Common.DTOs.PurchaseHistoryResponseDTO;

namespace Nofshonit.BL.Purchase.PurchaseUtils.PurchaseHandler
{
    public interface IPurchaseHandler
    {
        public List<HistoryVariant> GetHistoryForMember(MemberHistoryDTO memberHistoryDTO);
    }
}
