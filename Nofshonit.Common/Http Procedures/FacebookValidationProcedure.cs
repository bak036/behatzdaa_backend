using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Common.DTOs;

namespace Nofshonit.Common.Http_Procedures
{
    public static class FacebookValidationProcedure
    {
        public static string Token { get; set; }

        public static async Task<BaseResponse<FacebookValidationDataDTO>> IsValid(string token, string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"https://graph.facebook.com/{id}?access_token={token}"),
                        Method = HttpMethod.Get,
                    };
                    dynamic responseString = JObject.Parse(await client.SendAsync(request).Result.Content.ReadAsStringAsync());
                    var facebookDto = new FacebookValidationDataDTO(responseString);
                    if (responseString != null)
                    {
                        if (responseString.id == id)
                        {
                            Token = token;
                            return new BaseResponse<FacebookValidationDataDTO>
                            {
                                Status = true,
                                Data = facebookDto,
                                Message = "The Validation with Facebook succeeded"
                            };
                        }
                        throw new Exception("There is a problem with Facebook Token or Facebook ID");
                    }
                    throw new Exception("There is a problem with Facebook Token or Facebook ID");
                }
            }
            catch
            {
                return new BaseResponse<FacebookValidationDataDTO>
                {
                    Status = false,
                    Message = "The Validation with Facebook failed"
                };
            }
        }
    }
}


//https://graph.facebook.com/me?access_token=...