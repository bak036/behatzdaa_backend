using Nofshonit.Common;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System.Collections.Generic;

namespace Nofshonit.Services.LimitationsService
{
    public class LimitationsService : BaseService, ILimitationsService
    {
        private ILimitationsBL _limitationsBL;

        public LimitationsService()
        {
            _limitationsBL = Container.Resolve<ILimitationsBL>();
        }

        public List<VariantOrderLimitDTO> ValidatePurchesAllowed()
        {
            return _limitationsBL.ValidatePurchesAllowed();
        }
        public Dictionary<string, bool> GetProductStockStatus(long? categoryId, List<string> barcodes = null, int qty = 1)
        {
            return _limitationsBL.GetProductStockStatus(categoryId, barcodes,qty);
        }
    }
}
