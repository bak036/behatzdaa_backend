using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Nofshonit.BL;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Logs;
using Nofshonit.Repositories.ClubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nofshonit.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RecaptchaValidationAttribute : ActionFilterAttribute
    {
        private IConfigurationManager _configuration;
        private IClubRepo _clubRepo;
        public RecaptchaValidationAttribute()
        {
            BaseBL baseBL = new BaseBL();
            _configuration = baseBL.Container.Resolve<IConfigurationManager>();
            _clubRepo = baseBL.Container.Resolve<IClubRepo>();
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext _context, ActionExecutionDelegate _next)
        {
            string memberId = string.Empty;
            try
            {
                DtsLoggger.Logger.Info("start check captcha!");
                bool valid = true;
                string errorMessage = string.Empty;
                string captchaToken = _context.HttpContext.Request.Headers[HeadersKeys.ReCaptchaToken];
                bool fromNative = !string.IsNullOrEmpty(_context.HttpContext.Request.Headers[HeadersKeys.Native]);
                memberId = _context.HttpContext.Request.Headers[HeadersKeys.MemberId];
                if (string.IsNullOrEmpty(memberId))
                    memberId = "Unknow";
                var codes = _clubRepo.GetSmsCodes(memberId);


                DtsLoggger.Logger.Info($"start PostAsync captcha for memberId: {memberId}");
                var recaptchaSecretKey = _configuration.GetConfigByValue<string>(ConfigurationKey.RecaptchaSecretKey);
                var dictionary = new Dictionary<string, string>
                    {
                        { "secret", recaptchaSecretKey },
                        { "response", captchaToken }
                    };
                var encodedBody = new FormUrlEncodedContent(dictionary);
                HttpResponseMessage recaptchaResponse = null;
                string stringContent = string.Empty;
                using (var http = new HttpClient())
                {
                    recaptchaResponse = await http.PostAsync(_configuration.GetConfigByValue<string>(ConfigurationKey.RecaptchaGoogleApi), encodedBody);
                    stringContent = await recaptchaResponse.Content.ReadAsStringAsync();
                }
                DtsLoggger.Logger.Info($"end PostAsync captcha for memberId: {memberId}");

                if (codes.Any() || fromNative)
                {
                    if (fromNative)
                        DtsLoggger.Logger.Info($"skip captcha in native for memberId: {memberId}");
                    if (codes.Any())
                        DtsLoggger.Logger.Info($"skip captcha for memberId: {memberId}");
                    await _next();
                    return;
                }

                if (!recaptchaResponse.IsSuccessStatusCode)
                {
                    _context.HttpContext.Response.StatusCode = 401;
                    errorMessage = "Unable to verify recaptcha token";
                    valid = false;
                    LoggerHelper.Error(errorMessage);
                }

                if (string.IsNullOrEmpty(stringContent))
                {
                    _context.HttpContext.Response.StatusCode = 401;
                    errorMessage = "Invalid reCAPTCHA verification response";
                    valid = false;
                    LoggerHelper.Error(errorMessage);
                }
                var googleReCaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponseAttributeDTO>(stringContent);

                if (!googleReCaptchaResponse.Success)
                {
                    errorMessage = string.Join(",", googleReCaptchaResponse.ErrorCodes);
                    _context.HttpContext.Response.StatusCode = 401;
                    valid = false;
                    LoggerHelper.Error(errorMessage);

                }

                if (googleReCaptchaResponse.Score < _configuration.GetConfigByValue<double>(ConfigurationKey.ReCaptchaScoreNeeded))
                {
                    _context.HttpContext.Response.StatusCode = 401;
                    errorMessage = "This is a potential bot. Signup request rejected";
                    valid = false;
                    LoggerHelper.Error($"{errorMessage}, memberId: {memberId}, Score: {googleReCaptchaResponse.Score}");
                }

                if (!valid)
                    _context.Result = new BadRequestObjectResult($"{errorMessage}, memberId: {memberId}");
                else
                {
                    DtsLoggger.Logger.Info($"succsses check captcha!, , memberId: {memberId}, score: {googleReCaptchaResponse.Score}");
                    await _next();
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"failed RecaptchaAttribute, Message: {ex.Message}, StackTrace: {ex.StackTrace}, memberId: {memberId}");
                //_context.HttpContext.Response.StatusCode = 401;
                //_context.Result = new BadRequestObjectResult("Invalid!");
                await _next(); // לתת למשתמשים להמשיך במקרה שאין תקשורת מול גוגל
            }
        }
    }
}

