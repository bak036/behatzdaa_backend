using System;
using System.Collections.Generic;
using System.Text;
using Nofshonit.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.DTOs.ResponseDTOs;

namespace Nofshonit.Services.OrderService
{
    public class OrderService : Base.BaseService, IOrderService
    {
        private readonly IOrderBL _orderBl;

        public OrderService()
        {
            _orderBl = Container.Resolve<IOrderBL>();
        }
        public async Task<PurchaseResponseDTO> Purchase(PurchaseRequestDTO request)
        {
            DtsLoggger.Logger.Info("Entering Purchase OrderService");

            return await _orderBl.PurchaseRequest(request);
        }

        public async Task<CancelResponseDTO> Cancel(CancelRequestDTO request)
        {
            return await _orderBl.Cancel(request);
        }

        public bool IsCancelRequriedAproove(string barCode)
        {
            return _orderBl.IsCancelRequriedAproove(barCode);
        }
        public async Task<string> GetConfirmationPage(ConfirmationPageDTO request)
        {
            return await _orderBl.GetConfirmationPage(request);

        }
		public async Task<BarcodePopupDetails> GetPopupBarcodeDetails(string asmachta)
		{
			return await _orderBl.GetPopupBarcodeDetails(asmachta);

		}
	}
}
