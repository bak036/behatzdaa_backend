using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Category;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils;
using Nofshonit.Infrastructure.Utils.Log;
using Nofshonit.Common.EF.DTS_Logs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nofshonit.Api.Controllers.MediaApi
{

    [EnableCors("CorsPolicy")]
    [Route("api/media")]
    public class MediaExternalApiController : BaseController
    {
        private IMediaService _service;

        public MediaExternalApiController()
        {
            _service = Container.Resolve<IMediaService>();
        }

        /// <summary>
        /// get  all Greeting Types by organization
        /// </summary>
        [HttpGet("GetGreetingTypes")]
        public BaseResponse<List<string>> GetGreetingTypes()
        {
            BaseResponse<List<string>> response = new BaseResponse<List<string>>();
            var logItem = new LogDTO();

            try
            {
               OnStart(logItem);
                response.Data = _service.GetGreetingTypes();
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }

            return response;
        }

        /// <summary>
        /// return all greeting messages by types
        /// </summary>
        [HttpGet("GetGreetings")]
        public BaseResponse<List<GreetingDTO>> GetGreetings()
        {
            BaseResponse<List<GreetingDTO>> response = new BaseResponse<List<GreetingDTO>>();
            var logItem = new LogDTO();

            try
            {
               OnStart(logItem);
                response.Data = _service.GetGreetings();
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }

            return response;

        }

        /// <summary>
        /// return greeting messages by single type
        /// </summary>
        /// <param name="type"></param>
        [HttpGet("GetMessagesByType")]
        public BaseResponse<List<string>> GetMessagesByType([FromQuery] string type)
        {
            BaseResponse<List<string>> response = new BaseResponse<List<string>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { type });
                response.Data = _service.GetMessagesByType(type);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }

            return response;
        }

        [HttpGet("GetOrganizationNews")]
        /// <summary>
        ///  Returns the current organization news from the db
        /// </summary>
        public BaseResponse<string> GetOrganizationNews()
        {
            var response = new BaseResponse<string>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                var news = _service.GetOrganizationNews();
                response.Data = news;
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }

        [HttpGet("GetImagesSlider")]
        /// <summary>
        ///  Returns the current organization imges silder from the db
        /// </summary>
        public BaseResponse<List<ImageSliderDTO>> GetImagesSlider()
        {
            var response = new BaseResponse<List<ImageSliderDTO>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                var result = _service.GetImagesSlider();
                response.Data = result;

            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("GetMessagesByIdList")]
        /// <summary>
        ///  Returns a list of messages by the id's given
        /// </summary>
        /// <param name="IdList">The id's of the messages</param>
        public BaseResponse<List<Messages>> GetMessagesById([FromBody]List<int> IdList)
        {
            var response = new BaseResponse<List<Messages>>();
            var logItem = new LogDTO();

            try
            {
               OnStart(logItem, null, IdList);
                response.Data = _service.GetMessagesById(IdList);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpPost("GetMessagesByKeyList")]
        /// <summary>
        ///  Returns a list of messages by the key's given
        /// </summary>
        /// <param name="KeyList">The key's of the messages</param>
        public BaseResponse<List<Messages>> GetMessagesByKey([FromBody]List<int> KeyList)
        {
            var response = new BaseResponse<List<Messages>>();
            var logItem = new LogDTO();

            try
            {
               OnStart(logItem, null, KeyList);
                response.Data = _service.GetMessagesByKey(KeyList);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }

        [HttpGet("GetMessagesByContext")]
        /// <summary>
        ///  Returns a list of messages by the context given
        /// </summary>
        /// <param name="context">The context of the messages</param>
        public BaseResponse<List<Messages>> GetMessagesByContext([FromQuery]string context)
        {
            var response = new BaseResponse<List<Messages>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { context });
                response.Data = _service.GetMessagesByContext(context);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }
        /// <summary>
        /// The method returns all the banners for organization
        /// </summary>
        /// <returns>List of messages</returns>
        //[AllowAnonymous]
        [HttpGet("GetCommercial")]
        public BaseResponse<List<Messages>> GetCommercial()
        {
            var response = new BaseResponse<List<Messages>>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                response.Data = _service.GetCommercial();
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }
        [AllowAnonymous]
        [HttpPost("AddReferralHistory")]
        public BaseResponse<bool> AddReferralHistory([FromBody] ReferralHistory data)
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);
                response.Data = _service.AddReferralHistory(data);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be.Message, response, logItem);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return response;
        }
    }
}
