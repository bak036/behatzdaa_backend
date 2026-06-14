using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nofshonit.Common.EF.Club;
using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;
using Nofshonit.Common.DTOs.DtsCancellation;

namespace Nofshonit.Common.Interfaces
{
    public interface IOrderBL
    {
        Task<PurchaseResponseDTO> PurchaseRequest(PurchaseRequestDTO request);
        Task<CancelResponseDTO> Cancel(CancelRequestDTO request);
        Task<string> GetConfirmationPage(ConfirmationPageDTO _request);
        bool IsCancelRequriedAproove(string barCode);
        List<AllMembers> Test();
        void Test2();
        Task<BarcodePopupDetails> GetPopupBarcodeDetails(string asmachta);

	}
}
