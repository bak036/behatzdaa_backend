using Dynamitey.DynamicObjects;
using Nofshonit.Common;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nofshonit.BL.RestApiGW
{
    public class RestApiGW : IRestApiGW
    {
        public RestApiGW()
        {
        }

        public T ApiRequest<T>(ApiRequestModel apiRequestModel)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            using (HttpClient client = new HttpClient())
            {
                string stringJson;
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.RequestUri = new Uri(apiRequestModel.baseUrl + apiRequestModel.relativeUrl);
                if(apiRequestModel.headers != null)
                    foreach (KeyValuePair<string, string> header in apiRequestModel.headers)
                    {
                        httpRequestMessage.Headers.Add(header.Key, header.Value);
                    }
                switch (apiRequestModel.method)
                {
                    case EHttpRequestType.POST:
                        httpRequestMessage.Method = HttpMethod.Post;
                        JsonSerializerOptions jso = new JsonSerializerOptions();
                        jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                        stringJson = JsonSerializer.Serialize(apiRequestModel.data, jso);
                        httpRequestMessage.Content = new StringContent(stringJson, Encoding.UTF8, "application/json");
                        break;

                    case EHttpRequestType.GET:
                        httpRequestMessage.Method = HttpMethod.Get;
                        break;
                }
                response = client.SendAsync(httpRequestMessage).Result;
                return GetModelFromResponse<T>(response, apiRequestModel.baseUrl, apiRequestModel.relativeUrl);
            }
        }

        private T GetModelFromResponse<T>(HttpResponseMessage responseMessage, string baseUrl, string relativeUrl)
        {
            string responseBody = responseMessage.Content.ReadAsStringAsync().Result;
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                T jsonData = JsonSerializer.Deserialize<T>(responseBody);
                if (jsonData == null)
                    LoggerHelper.Error($"Failed on response from GppRestApi service in url: {baseUrl + relativeUrl}, Body is null FullResponseBody: {responseBody}");
                return jsonData;
            }
            catch (HttpRequestException e)
            {
                LoggerHelper.Error($"Failed on HttpClient in url: {baseUrl + relativeUrl}, to connect Vpay service, Message: {e.Message}, FullResponseBody: {responseBody} StackTrace: {e.StackTrace}, ex: {e}");
                return default;
            }
        }
    }

}
