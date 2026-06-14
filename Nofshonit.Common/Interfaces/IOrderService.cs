using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.DtsCancellation;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;

namespace Nofshonit.Common.Interfaces
{
    public interface IOrderService
    {
        Task<PurchaseResponseDTO> Purchase(PurchaseRequestDTO request);
        Task<CancelResponseDTO> Cancel(CancelRequestDTO request);
        Task<string> GetConfirmationPage(ConfirmationPageDTO request);
        bool IsCancelRequriedAproove(string barCode);
        Task<BarcodePopupDetails> GetPopupBarcodeDetails(string asmachta);

	}
}
