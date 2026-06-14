using Nofshonit.Common.DTOs;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System.Threading.Tasks;

namespace Nofshonit.Services.PurchaseService
{
    public class PurchaseService: BaseService, IPurchaseService
    {
        private IPurchaseBL _purchaseBL;
        public PurchaseService():base()
        {
            _purchaseBL  = Container.Resolve<IPurchaseBL>();
        }

        public Task<PurchaseHistoryResponseDTO> PurchaseHistory(MemberHistoryDTO memberHistoryDTO)
        {
            return _purchaseBL.PurchaseHistoryAsync(memberHistoryDTO);
        }

        public string PurchaseHistoryOld()
        {
            return _purchaseBL.PurchaseHistoryOld();
        }

        public Task<string> SendOldOrdersLinkToUnauthorizedUser(string memberId)
        {
            return _purchaseBL.SendOldOrdersLinkToUnauthorizedUser(memberId);
        }
    }
}
