using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.BL.Order.OrderUtils.OrderHandler
{
    public interface IOrderHandler
    {
        Task<PurchaseResponseDTO> Purchase(PurchaseRequestDTO purchaseRequestDTO);
    }
}
