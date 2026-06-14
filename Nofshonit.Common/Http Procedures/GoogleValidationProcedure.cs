using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using System.Threading;
using Google.Apis.ServiceUser.v1;
using Google.Apis.Plus.v1;
using Google.Apis.Util.Store;
using Nofshonit.Infrastructure.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nofshonit.Common.DTOs;

namespace Nofshonit.Common.Http_Procedures
{
    public static class GoogleValidationProcedure
    {
        public static string Token { get; set; }

        public static async Task<BaseResponse<GoogleValidationDataDTO>> IsValid(string token, string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"https://www.googleapis.com/plus/v1/people/{id}?access_token={token}"),
                        Method = HttpMethod.Get,
                    };
                    dynamic responseString = JObject.Parse(await client.SendAsync(request).Result.Content.ReadAsStringAsync());
                    var googleDto = new GoogleValidationDataDTO(responseString);
                    if (responseString != null)
                    {
                        if (responseString.id == id)
                        {
                            Token = token;
                            return new BaseResponse<GoogleValidationDataDTO>
                            {
                                Status = true,
                                Data = googleDto,
                                Message = "The Validation with Google succeeded"
                            };
                        }
                        throw new Exception("There is a problem with Google Token or Google ID");
                    }
                    throw new Exception("There is a problem with Google Token or Google ID");
                }
            }
            catch
            {
                return new BaseResponse<GoogleValidationDataDTO>
                {
                    Status = false,
                    Message = "The Validation with Google failed"
                };
            }
        }
    }
}



//https://developers.google.com/oauthplayground/

