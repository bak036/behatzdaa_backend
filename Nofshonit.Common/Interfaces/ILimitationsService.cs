using Nofshonit.Common.DTOs.Limitations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
    public interface ILimitationsService
    {
        List<VariantOrderLimitDTO> ValidatePurchesAllowed();
        Dictionary<string, bool> GetProductStockStatus(long? categoryId, List<string> barcodes = null, int qty = 1);
    }
}
