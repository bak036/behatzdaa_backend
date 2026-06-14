using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.Interfaces
{
    public interface ILimitationsBL
    {

        List<VariantOrderLimitDTO> ValidatePurchesAllowed();

        List<VariantOrderLimitDTO> ValidatePurchesAllowed(List<CartVarsDTO> cartVarList);
        VariantOrderLimitDTO ValidatePurchesAllowed(CartVarsDTO cartVar);
        int GetVariantOrderLimit(string productVarBarcode, int cartQtyBySubtype = 0, bool isPuchase = false, bool CheckOrderLimitEqualsZero = true);
        Dictionary<string, bool> GetProductStockStatus(long? categoryId, List<string> barcodes = null, int qty = 1);
        void ValidateCategoryForMemberLimitations(long categoryNumber, bool isCategoryProductPage = false);
    }
}
