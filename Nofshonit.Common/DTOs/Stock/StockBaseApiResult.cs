using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Stock
{
    public class StockBaseApiResult
    {
        public enum HTTPResponseCode
        {
            Success = 200,
            BadRequest = 400,
            NotAuthorized = 401,
            InternalError = 500
        }
        public HTTPResponseCode Code { get; set; }
        public List<VariantResult> VariantResult { get; set; }

        //Default Result
        public StockBaseApiResult()
        {
            Code = HTTPResponseCode.Success;
            VariantResult = new List<VariantResult>();
        }

        public void SetCode(HTTPResponseCode code)
        {
            Code = code;
        }
    }
}
