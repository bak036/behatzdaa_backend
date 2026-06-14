using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Base;
using Nofshonit.Common.DTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace Nofshonit.Api.Controllers.AddressesApi
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressesExternalApiController : BaseController
    {
        private readonly IAddressesService _service;


        public AddressesExternalApiController()
        {
            _service = Container.Resolve<IAddressesService>();
		}

        //[AllowAnonymous]
        [HttpGet("GetCities")]
        public BaseResponse<List<CityDTO>> GetCities()
        {
            var response = new BaseResponse<List<CityDTO>>();
			var logItem = new LogDTO();
			try
			{
				OnStart(logItem);
				response = _service.GetCities();
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
    
        //[AllowAnonymous]
        [HttpGet("GetCityStreets")]
		/// <summary>
		///  The method returns a list of all the streets of a city by the city id
		/// </summary>
		/// <param name="cityId">The City id number</param>
		/// <returns>
		///  Returns list of all the streets of a city by the city id
		/// </returns>
		public BaseResponse<List<StreetDTO>> GetCityStreets([FromQuery]int cityId, [FromHeader]string OrganizationId)
        {
            var response = new BaseResponse<List<StreetDTO>>();
			var logItem = new LogDTO();

            try
            {
				OnStart(logItem, new object[] { cityId });

				response =  _service.GetCityStreets(cityId);
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

        //[AllowAnonymous]
        [HttpGet("GetRegions")]
        public BaseResponse<List<RegionDTO>> GetRegions()
        {
            var response = new BaseResponse<List<RegionDTO>>();
			var logItem = new LogDTO();

			try
			{
			    OnStart(logItem);

				response = _service.GetRegions();
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
