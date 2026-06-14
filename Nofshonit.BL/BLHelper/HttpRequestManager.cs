using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Repositories.ClubModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Logs;
using Azure.Core;
using System.Security.Policy;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using OrderDll.TelemesserSms;

namespace Nofshonit.BL.BLHelper
{

    public static class HttpRequestManager
    {
        static Dictionary<int, int> _mappingErrorMessage = JsonConvert.DeserializeObject<Dictionary<int, int>>(ConfigurationKey.GetValueFromConfigurations(ConfigurationKey.HttpClientMessageKeys));

        public static async Task<JObject> HttpRequest(string urlRequest, object requestDTO, HttpMethod requestMethod, [CallerMemberName] string methodName = "")
        {
            var logDto = new LogDTO();
            string responseString = string.Empty;
            bool? isUnitTest = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.IsUnitTest);
            isUnitTest = (isUnitTest == null || (bool)!isUnitTest); //true if not unit tests
            if ((bool)isUnitTest)
                ApiLoggerBL.OnStart(logDto, new object[] { urlRequest, requestDTO }, methodName);
            try
            {
                using (var client = new HttpClient())
                {
                    string URL = urlRequest;
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(URL),
                        Method = requestMethod,
                    };
                    JsonResult memberHistoryDtoAsJson = new JsonResult(requestDTO);
                    request.Content = new StringContent(JsonConvert.SerializeObject(memberHistoryDtoAsJson.Value));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var watch = Stopwatch.StartNew();
                    responseString = await client.SendAsync(request).Result.Content.ReadAsStringAsync();
                    watch.Stop();
                    var a = watch.ElapsedMilliseconds;
                    if (responseString != null)
                    {
                        return JObject.Parse(responseString);
                    }
                    return null;
                }
            }
            finally
            {
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnEnd(logDto, responseString);
            }
        }



        public static async Task<T> HttpRequest<T>(string urlRequest, object requestDTO, HttpMethod requestMethod, [CallerMemberName] string methodName = "")
        {
            var logDto = new LogDTO();
            bool? isUnitTest = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.IsUnitTest);
            isUnitTest = (isUnitTest == null || (bool)!isUnitTest); //true if not unit tests
            if ((bool)isUnitTest)
                ApiLoggerBL.OnStart(logDto, new object[] { urlRequest, requestDTO }, methodName);
            string responseString = string.Empty;
            try
            {

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(300);
                    string URL = urlRequest;
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(URL),
                        Method = requestMethod,
                    };
                    JsonResult memberHistoryDtoAsJson = new JsonResult(requestDTO);
                    request.Content = new StringContent(JsonConvert.SerializeObject(memberHistoryDtoAsJson.Value));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var watch = Stopwatch.StartNew();
                    responseString = await client.SendAsync(request).Result.Content.ReadAsStringAsync();
                    watch.Stop();
                    var a = watch.ElapsedMilliseconds;
                    if (responseString != null)
                    {
                        return JsonConvert.DeserializeObject<T>(responseString);
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, " Error on HttpRequest");
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnException(logDto, ex.Message);
                throw new BusinessException(ex.Message);
            }
            finally
            {
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnEnd(logDto, responseString);
            }
        }

        public static async Task<JObject> HttpHistadrutRequest(object requestDTO, HttpMethod requestMethod, [CallerMemberName] string methodName = "")
        {
            using (var client = new HttpClient())
            {
                //string URL = HttpUrls.LinkURL + urlRequest;

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://212.25.107.230/together_tests/index.php?wsdl"),
                    Method = requestMethod,
                };
                JsonResult memberHistoryDtoAsJson = new JsonResult(requestDTO);
                request.Content = new StringContent(JsonConvert.SerializeObject(memberHistoryDtoAsJson.Value));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var watch = Stopwatch.StartNew();
                var responseString = await client.SendAsync(request).Result.Content.ReadAsStringAsync();
                watch.Stop();
                var a = watch.ElapsedMilliseconds;
                if (responseString != null)
                {
                    return JObject.Parse(responseString);
                }
                return null;
            }
        }
        public static async Task<T> HttpClientGet<T>(string path, [CallerMemberName] string methodName = "")
        {
            var logDto = new LogDTO();
            bool? isUnitTest = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.IsUnitTest);
            isUnitTest = (isUnitTest == null || (bool)!isUnitTest); //true if not unit tests
            if ((bool)isUnitTest)
                ApiLoggerBL.OnStart(logDto, new object[] { path }, methodName);
            string result = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(path))
                using (HttpContent content = response.Content)
                {
                    ErrorDTO err = null;
                    result = await content.ReadAsStringAsync();
                    result = result.Substring(1, result.Length - 2);
                    if (result.Contains("Error:"))
                    {
                        err = JsonConvert.DeserializeObject<ErrorDTO>(result);
                        int errorId = 0;
                        int messageId = 0;
                        var message = err.rows[0].Result;
                        if (int.TryParse(err.rows[0].ErrorID, out errorId) && _mappingErrorMessage.TryGetValue(errorId, out messageId))
                        {
                            var msg = MessagesUtil.GetMessagesByKey(new List<int> { messageId }).FirstOrDefault(m => m.MessageKey == messageId);
                            if (msg != null)
                            {
                                message = msg.MessageText;
                            }
                        }

                        throw new BusinessException(message);
                    }
                    return JsonConvert.DeserializeObject<T>(result);
                }

            }
            catch (HttpRequestException hex)
            {
                string message = MessagesUtil.GetMessagesByKey(new List<int> { 10236 }).FirstOrDefault(m => m.MessageKey == 10236).MessageText;
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnException(logDto, message);
                throw new BusinessException(message, hex);
            }
            catch (BusinessException bex)
            {
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnException(logDto, bex.Message);
                throw;
            }
            finally
            {
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnEnd(logDto, result);
            }
        }


        public static async Task<JObject> HttpRequestGet(string urlRequest, object requestDTO, [CallerMemberName] string methodName = "")
        {
            HttpMethod requestMethod = HttpMethod.Get;
            var logDto = new LogDTO();
            bool? isUnitTest = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.IsUnitTest);
            isUnitTest = (isUnitTest == null || (bool)!isUnitTest); //true if not unit tests
            if ((bool)isUnitTest)
                ApiLoggerBL.OnStart(logDto, new object[] { urlRequest, requestDTO }, methodName);
            string responseString = string.Empty;
            string queryString = "";
            try
            {
                if (requestDTO != null)
                {
                    var dic = requestDTO.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(requestDTO, null));
                    queryString = string.Join("&", dic.Select(x => x.Key + "=" + x.Value));
                }
                using (var client = new HttpClient())
                {
                    string URL = urlRequest;
                    if (!URL.Contains("?")) URL += "?";
                    URL += queryString;

                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(URL),
                        Method = requestMethod,
                    };

                    var watch = Stopwatch.StartNew();
                    responseString = await client.SendAsync(request).Result.Content.ReadAsStringAsync();
                    watch.Stop();
                    var a = watch.ElapsedMilliseconds;
                    if (responseString != null)
                        return JObject.Parse(responseString);

                    return null;
                }
            }
            finally
            {
                if ((bool)isUnitTest)
                    ApiLoggerBL.OnEnd(logDto, responseString);
            }
        }
        public static async Task<IDFValidationResponseDTO> GetIDFValidationForMember(string tz)
        {
            HttpMethod requestMethod = HttpMethod.Post;
            IDFValidationResponseDTO result = new IDFValidationResponseDTO() { Zehut = tz, isValid = null };
            var logDto = new LogDTO();
            bool? isUnitTest = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.IsUnitTest);
            bool? skipValidation = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.SkipMinistryOfDefenceValidation);
            isUnitTest = (isUnitTest == null || (bool)!isUnitTest); //true if not unit tests
            //if ((bool)isUnitTest)
            //    ApiLoggerBL.OnStart(logDto, new object[] { urlRequest, requestDTO }, methodName);
            if (skipValidation.HasValue && skipValidation.Value) return new IDFValidationResponseDTO() { Zehut = tz, premiumeType = EIDFPremiumeType.Approved, darga = EDarga.Miluim, isValid = true };

            string responseString = string.Empty;

            string accessToken = GetApigeeAccessToken();
            result = await GetIDFValidationFromApigee(tz, accessToken);
            return result;
        }


        private static string GetApigeeAccessToken()
        {
            string url = HttpUrls.ApigeeTokenURL;
            string consumerKey = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.ApigeeConsumerKey);
            string consumerSecret = GetDecryptedApigeeConsumerSecret();

            using (var client = new HttpClient())
            {
                var ministryOfDefenceTimeOut = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.MinistryOfDefenceTimeOut);
                if (ministryOfDefenceTimeOut != null)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(int.Parse(ministryOfDefenceTimeOut));
                }
                string credentials = string.Format("{0}:{1}", consumerKey, consumerSecret);
                string base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));


                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", base64Credentials);

                var content = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        string.Format("Failed to get Apigee access token. StatusCode: {0}", response.StatusCode));
                }

                string responseContent = response.Content.ReadAsStringAsync().Result;

                dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);

                return tokenResponse.access_token;
            }
        }
        private static async Task<IDFValidationResponseDTO> GetIDFValidationFromApigee(string tz, string accessToken)
        {
            string url = $"{HttpUrls.ApigeeValidationRelativeURL}?id={tz}";
            string consumerKey = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.ApigeeConsumerKey);
            IDFValidationResponseDTO result = new IDFValidationResponseDTO() { Zehut = tz, isValid = null };
            try
            {
                using (var client = new HttpClient())
                {
                    var ministryOfDefenceTimeOut = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.MinistryOfDefenceTimeOut);
                    if (ministryOfDefenceTimeOut != null)
                    {
                        client.Timeout = TimeSpan.FromMilliseconds(int.Parse(ministryOfDefenceTimeOut));
                    }
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get
                    };
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    request.Headers.Add("x-apikey", consumerKey);

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(
                            $"Apigee validation request failed. StatusCode: {response.StatusCode}");
                    }

                    string responseString = await response.Content.ReadAsStringAsync();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responseString);
                    int miluim = 0;
                    int honor = 0;

                    var root = xmlDocument.DocumentElement;
                    // זכאי מילואים
                    if (int.TryParse(root?["PremiumTypeForMiluimnikPail"]?.InnerText, out miluim)
                        && miluim > 0)
                    {
                        result.premiumeType = EIDFPremiumeType.Approved;
                        result.darga = EDarga.Miluim;
                        result.isValid = true;
                        result.cellPhone = root?["CELLPHONE"]?.InnerText;
                    }
                    // זכאי משוחררים בכבוד
                    else if (int.TryParse(root?["PremiumTypeForReleasedWithHonor"]?.InnerText, out honor)
                             && honor > 0)
                    {
                        result.premiumeType = EIDFPremiumeType.Approved;
                        result.darga = EDarga.Miluim;
                        result.isValid = true;
                        result.cellPhone = root?["CELLPHONE"]?.InnerText;
                    }
                    // לא זכאי
                    else
                    {
                        result.premiumeType = EIDFPremiumeType.NotApproved;
                        result.darga = EDarga.Unkown;
                        result.isValid = false;
                        result.cellPhone = root?["CELLPHONE"]?.InnerText;
                    }

                    DtsLoggger.Logger.Info($"Response from IDF: {responseString}");

                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Error on GetIDFValidationForMember:");
            }
            return result;
        }

        // Process-local cache for the decrypted Apigee consumer_secret.
        // The decrypted value is held in memory only; it is NEVER persisted to disk,
        // DB, logs or the shared ICacheManager (to avoid accidental serialization).
        private static string _cachedDecryptedApigeeConsumerSecret;
        private static readonly object _apigeeConsumerSecretLock = new object();

        /// <summary>
        /// Returns the decrypted Apigee / IDF consumer_secret.
        /// The value is decrypted on the first call and cached in memory for subsequent calls.
        /// On any failure a technical exception is thrown and an error is logged (without exposing
        /// the secret, the encryption key, the derived key, the salt or the full ciphertext).
        /// </summary>
        private static string GetDecryptedApigeeConsumerSecret()
        {
            if (_cachedDecryptedApigeeConsumerSecret != null)
                return _cachedDecryptedApigeeConsumerSecret;

            lock (_apigeeConsumerSecretLock)
            {
                if (_cachedDecryptedApigeeConsumerSecret != null)
                    return _cachedDecryptedApigeeConsumerSecret;

                try
                {
                    // 1. Encrypted secret from web.config / appsettings.json.
                    string encryptedSecret = ContainerManager.Container.Resolve<IConfigurationManager>()
                        .GetConfigByValue<string>(ConfigurationKey.BehatzdaaApigeeEncryptedSecret);

                    if (string.IsNullOrWhiteSpace(encryptedSecret))
                        throw new InvalidOperationException(
                            $"Configuration value '{ConfigurationKey.BehatzdaaApigeeEncryptedSecret}' is missing.");

                    // 2. Salt from dbo.AppConfig.
                    var clubRepo = ContainerManager.Container.Resolve<IClubRepo>();
                    var appConfigs = clubRepo
                        .getValueByKeyList(new List<string> { ConfigurationKey.BehatzdaaApigeeSecretSalt })
                        .GetAwaiter().GetResult();

                    string saltBase64 = appConfigs?
                        .FirstOrDefault(c => c.Key == ConfigurationKey.BehatzdaaApigeeSecretSalt)?
                        .Value;

                    if (string.IsNullOrWhiteSpace(saltBase64))
                        throw new InvalidOperationException(
                            $"AppConfig entry '{ConfigurationKey.BehatzdaaApigeeSecretSalt}' is missing from dbo.AppConfig.");

                    // 3. Encryption key from the secure configuration mechanism (appsettings, per environment).
                    string encryptionKey = ContainerManager.Container.Resolve<IConfigurationManager>()
                        .GetConfigByValue<string>(ConfigurationKey.BehatzdaaApigeeEncryptionKey);

                    if (string.IsNullOrWhiteSpace(encryptionKey))
                        throw new InvalidOperationException(
                            $"Configuration value '{ConfigurationKey.BehatzdaaApigeeEncryptionKey}' is missing.");

                    // 4-5. Derive key with PBKDF2 and AES-decrypt.
                    string decrypted = DecryptionHelper.DecryptSecret(encryptedSecret, encryptionKey, saltBase64);

                    if (string.IsNullOrEmpty(decrypted))
                        throw new InvalidOperationException("Decrypted Apigee consumer_secret is empty.");

                    // 6. Cache in memory only.
                    _cachedDecryptedApigeeConsumerSecret = decrypted;
                    return _cachedDecryptedApigeeConsumerSecret;
                }
                catch (Exception ex)
                {
                    // Log a generic error. Do NOT include the secret, the encryption key,
                    // the salt, the derived AES key or the ciphertext.
                    LoggerHelper.Error(ex, "Failed to load and decrypt Apigee consumer_secret.");
                    throw;
                }
            }
        }

    }
}
