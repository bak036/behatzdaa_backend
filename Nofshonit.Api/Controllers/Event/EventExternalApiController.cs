using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Event;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Nofshonit.Api.Controllers.Event
{
    [EnableCors("CorsPolicy")]
    [Route("api/event")]
    public class EventExternalApiController : BaseController
    {
        private readonly IEventService _service;
        
        public EventExternalApiController()
        {
            _service = Container.Resolve<IEventService>();
        }
      

        /// <summary>
        /// return category details with all tickets by event
        /// </summary>
        [HttpGet("EventCatalog")]
        public BaseResponse<CatalogDTO> EventCatalog([FromQuery]int eventId, [FromQuery]long categoryId)
        {
            BaseResponse<CatalogDTO> response = new BaseResponse<CatalogDTO>();
            var logItem = new LogDTO();

			try
			{
				OnStart(logItem, new object[] { eventId });
                response.Data = Container.Resolve<IEventService>().EventCatalog(eventId, categoryId);
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
        /// save member reservation tickets
        /// step before CreateOrder
        /// in step of insert to basket
        /// </summary>
        //[HttpPost("CreateReservation")]
         BaseResponse<CreateReservationResponseDTO> CreateReservation([FromBody] CreateReservationRequest request)
        {
            BaseResponse<CreateReservationResponseDTO> response = new BaseResponse<CreateReservationResponseDTO>();
            var logItem = new LogDTO();

			try
			{
				OnStart(logItem, null, request);
                response.Data = Container.Resolve<IEventService>().CreateReservation(request);
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
