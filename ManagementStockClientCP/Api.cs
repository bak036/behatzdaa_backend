using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagementStockClient.Client;
using ManagementStockClient.Models;
using Newtonsoft.Json;
using Nofshonit.Logs;

namespace ManagementStockClient
{
    public class Api
    {

        public void SetEndPointURL(string apiLink)
        {
            RESTFulAPI<BaseApiResult>._apiURL = apiLink;
        }
        public void InsertLog(string function, CheckStockRequestModel model, BaseApiResult result)
        {
             try
            {
                string requestJson = JsonConvert.SerializeObject(model.VariantData);
                string responseJson = JsonConvert.SerializeObject(result);
                string log = $"{function}:{model.OrgID},  Request: {requestJson} | Response: {responseJson}";
                DtsLogggerCP.Logger.Info(log);
            }
            catch { }
        }
        public BaseApiResult CheckStock(CheckStockRequestModel model)
        {
            try
            {
                var result = RESTFulAPI<BaseApiResult>.GetObject("/checkstock", requesteObject: model);
                InsertLog("CheckStock", model, result);
                return result;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("CheckStock, Exception :" + ex.Message);

                var errorResult = new BaseApiResult();
                errorResult.Code = BaseApiResult.HTTPResponseCode.InternalError;
                return errorResult;
            }

       
        }

        public BaseApiResult GetStockLimit(CheckStockRequestModel model)
        {
            try
            {
                var result = RESTFulAPI<BaseApiResult>.GetObject("/getstocklimit", requesteObject: model);
                InsertLog("GetStockLimit", model, result);
                return result;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("CheckStock, Exception :" + ex.Message);

                var errorResult = new BaseApiResult();
                errorResult.Code = BaseApiResult.HTTPResponseCode.InternalError;
                return errorResult;
            }
        }
        public BaseApiResult UpdateStock(CheckStockRequestModel model)
        {
            try
            {
                var result = RESTFulAPI<BaseApiResult>.GetObject("/updatestock", requesteObject: model);
                InsertLog("UpdateStock", model, result);
                return result;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Updatestock, Exception :" + ex.Message);

                var errorResult = new BaseApiResult();
                errorResult.Code = BaseApiResult.HTTPResponseCode.InternalError;
                return errorResult;
            }
        }

    }
}
