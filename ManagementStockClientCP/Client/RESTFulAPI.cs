using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ManagementStockClient.Client
{
    public static class RESTFulAPI<T>
    {
        public static string _apiURL = null;

        public static string API_URL
        {
            get
            {
                if (_apiURL == null)
                {
                    if (ConfigurationManager.AppSettings["RESTFulAPI_StockManagement_ApiUrl"] == null)
                        throw new Exception("RESTFulAPI_StockManagement_ApiUrl Is Missing in Webconfig");

                    _apiURL = ConfigurationManager.AppSettings["RESTFulAPI_StockManagement_ApiUrl"].ToString();
                }

                return _apiURL;
            }
        }

       
        //public static int DTS_SERVICE_ID = int.Parse(ConfigurationManager.AppSettings["RESTFulAPI_DtsServiceID"].ToString());

           

        public static T GetObject(string endpoint, EnumRestFulMethod method = EnumRestFulMethod.POST, object requesteObject = null)
        {
            var openRequestTime = DateTime.Now;
            var postRespones = SendPacket(endpoint, method, requesteObject);
            var jsonString = postRespones.JsonResult;
            DtsLogggerCP.Logger.Info("result: " + jsonString.Replace("{", "(").Replace("}", ")"));
            var obj = default(T);
            //If Exception exists
            if (postRespones.IsException)
            {
                jsonString = "{ }";
            }

            if (typeof(T) != typeof(string)) obj = JsonConvert.DeserializeObject<T>(jsonString);
            else obj = (T)(object)jsonString;

            //TODO: Add log
            //InsertLog(endpoint, openRequestTime, requesteObject, postRespones, DTS_SERVICE_ID);

            return obj;
        }

        //Doing POST/GET/PUT/DELETE Request return json string
        private static PostResponse SendPacket(string endpoint, EnumRestFulMethod method, object requestObject)
        {
            var result = new PostResponse
            {
                JsonResult = string.Empty
            };
            string targetURL = "";
            try
            {
                targetURL = API_URL; //.AppSettings["RESTFulAPI_ApiUrl"].ToString();
                targetURL += endpoint;
            }
            catch (Exception ex)
            {

                string errorException = ex.Message;

            }



            //GET Parameters
            if (method == EnumRestFulMethod.GET && requestObject != null)
            {
                var dic = (Dictionary<string, string>)requestObject;
                string queryString = string.Join("&", dic.Select(x => x.Key + "=" + x.Value));
                if (!targetURL.Contains("?")) targetURL += "?";
                targetURL += queryString;
                DtsLogggerCP.Logger.Info("Get params:" + queryString);
            }
            DtsLogggerCP.Logger.Info("targetURL:" + targetURL);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(targetURL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method.ToString();

            try
            {
                //POST/PUT/DELETE Parameters
                if (requestObject != null && method != EnumRestFulMethod.GET)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        //POST JSON Data
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestObject);
                        DtsLogggerCP.Logger.Info(json.Replace("{", "(").Replace("}", ")"));
                        DtsLogggerCP.Logger.Info("-------------------");
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
            }
            catch (Exception e)
            {
                DtsLogggerCP.Logger.Info("//POST/PUT/DELETE Parameters error:" + e);
                throw e;
            }

            try
            {
                //Execute Request network
                //POST/GET/PUT/DELETE 
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result.JsonResult = streamReader.ReadToEnd();
                    DtsLogggerCP.Logger.Info("Post JsonResult:" + result.JsonResult.Replace("{", "(").Replace("}", ")"));
                }
            }
            catch (WebException wex)
            {
                //Case return 505 error code we still want to read result
                DtsLogggerCP.Logger.Info("Post WebException:" + wex.ToString());

                try
                {
                    result.JsonResult = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                }
                catch
                {
                    result.IsException = true;
                    result.Exception = wex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                DtsLogggerCP.Logger.Info("//Execute Request network ex:" + ex);

                //Error Log
                result.IsException = true;
                result.Exception = ex.Message.ToString();
            }

            return result;
        }
    }
}
