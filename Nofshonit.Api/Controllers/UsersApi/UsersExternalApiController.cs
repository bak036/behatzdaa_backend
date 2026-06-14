using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nofshonit.Api.Attributes;
using Nofshonit.Api.Base;
using Nofshonit.Api.Controllers.Base;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Nofshonit.Logs;

namespace Nofshonit.Api.Controllers.UsersApi
{
    [Route("api/users")]
    [ApiController]
    public class UsersExternalApiController : BaseController
    {

        private readonly IUserService _service;


        public UsersExternalApiController()
        {
            _service = Container.Resolve<IUserService>();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        /// <summary>
		///  The method returns a user after authentication. For Histadrut, The method will also make a basic registration.
		/// </summary>
		/// <param name="userRequestDto">The Given details for the authentication</param>
		/// <returns>
		///  Returns the user's details from the DB
		/// </returns>
        public async Task<BaseResponse<ResponseUserDTO>> Authenticate([FromBody]AuthenticateUserRequestDTO userRequestDto)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, userRequestDto);
                response = await _service.Authenticate(userRequestDto);
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
		///  The method return if the given id is valid in Histadrut 
		/// </summary> 
		/// <param name="histadrutValidationDTO">
		///  The Dto with the identity number as both id and password
		///	</param> 
		/// <returns>
		///  Returns true if the user is valid, false otherwise
		/// </returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("histadrutValidation")]
        public async Task<BaseResponse<IDFValidationResponseDTO>> HistadrutValidation(string memberid)
        {
            var response = new BaseResponse<IDFValidationResponseDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, memberid);
                var res = await _service.MinistryOfDefenceValidation(memberid);
                response.Data = res;
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
		///  The method will return the user after updating his / her details
		/// </summary> 
		/// <param name="updateMamberUserDTO">
		///  The Dto with all the given options of updating
		///	</param> 
		/// <returns>
		///  Returns true if the user has been updated, false otherwise
		/// </returns>
        [HttpPost]
        [Route("updateMember")]
        public async Task<BaseResponse<ResponseUserDTO>> UpdateMember([FromBody]UpdateMamberUserDTO updateMamberUserDTO)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            { 
                OnStart(logItem, null, updateMamberUserDTO);

                response = await _service.UpdateMember(updateMamberUserDTO);
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

        [HttpPost]
        [Route("UpdateMemberCookiesAcceptedDate")]
        public async Task<bool> UpdateMemberCookiesAcceptedDate()
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {


                OnStart(logItem, null);

                response.Data = await _service.UpdateMemberCookiesAcceptedDate();
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
            return response.Data;
        }



        /// <summary> 
		///  The method will return the user after updating his / her password
		/// </summary> 
		/// <param name="userPasswordInfoDto">
		///  The Dto with the relevant fields for updating the user's password
		///	</param> 
		/// <returns>
		///  Returns true if the password has been updated, false otherwise
		/// </returns>
        [HttpPost]
        [Route("updatePassword")]
        public async Task<BaseResponse<ResponseUserDTO>> UpdatePassword([FromBody]UserPasswordInfoDTO userPasswordInfoDto)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, userPasswordInfoDto);

                response = await _service.UpdatePassword(userPasswordInfoDto);
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
        [HttpPost]
        [Route("resetPassword")]
        public async Task<BaseResponse<ResponseUserDTO>> ResetPassword([FromBody]UserPasswordInfoDTO userPasswordInfoDto)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, userPasswordInfoDto);

                response = await _service.UpdatePassword(userPasswordInfoDto);
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
        [HttpPost]
        [Route("recoverPassword")]
        /// <summary>
        ///  The method creates a random 16 digits token and returns a link 
        ///  to the update password page with the token
        /// </summary>
        /// <param name="body">
        ///  The id of the member who wishes to recover his password
        /// </param>
        ///  <remarks>
        ///  This API is kept for password reset functionality used by the GiftCard system.
        /// </remarks>
        public async Task<BaseResponse<string>> RecoverPassword([FromBody]RecoverPasswordRequestBodyDTO body)
        {
            var response = new BaseResponse<string>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, body);

                response = await _service.RecoverPassword(body.MemberId);
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
		///  The method will return the user after creating a new row in the database
		/// </summary> 
		/// <param name="userDto">
		///  The Dto with the relevant fields for the registration
		///	</param> 
		/// <returns>
		///  Returns true if the user has been created, false otherwise
		/// </returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<BaseResponse<ResponseUserDTO>> Register([FromBody]UserDTO userDto)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, userDto);

                response = await _service.CreateNewUser(userDto);

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
		///  The method will return the user according to the given Token from Header
		/// </summary> 
        [HttpGet]
        [Route("getCurrentUser")]
        public BaseResponse<ResponseUserDTO> GetCurrentUser()
        {
            
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                response = _service.GetCurrentUser();
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

        [HttpGet]
        [Route("LogOut")]
        public void LogOut()
        {
            try
            {
                _service.LogOut();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error on LogOut, Message: {ex.Message}, InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("validatePasswordForRecover")]
        /// <summary>
        ///  The method validates the token from the RecoverPassword request and 
        ///  if the validation is successful, then it returns the user details
        /// </summary>
        /// <param name="tokenCode">The token to validate</param>
        /// <returns>Returns the user details</returns>
        public BaseResponse<ResponseUserDTO> ValidatePasswordForRecover([FromQuery] string tokenCode, [FromQuery] string memberId)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { tokenCode });

                response = _service.ValidatePasswordForRecover(tokenCode, memberId);

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


        [HttpPost("userSlinkSMS")]
        public async Task<BaseResponse<bool>> UserSlinkSMS()
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem);

                response = await _service.UserSlinkSMS();

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
        [HttpGet]
        [Route("getMemberByAccessToken")]
        public BaseResponse<ResponseUserDTO> GetUserByAccessToken([FromQuery]string accessToken)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { accessToken });

                response = _service.GetUserByAccessToken(accessToken);
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

        [RecaptchaValidation]
        [AllowAnonymous]
        [HttpPost]
        [Route("loginBySms")]
        public async Task<BaseResponse<ContactMemberDetails>> LoginBySms([FromBody] Member member)
        {
            var response = new BaseResponse<ContactMemberDetails>();
            var logItem = new LogDTO();

            string memberId = member.memberId;
            string hash = member.hash;
            try
            {
                OnStart(logItem, new object[] { memberId });

                response = await _service.LoginBySms(memberId, hash);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be, response, logItem);
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

        public class Member
        {
            public string memberId { get; set; }
            public string hash { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("UserOldOrdersSMS")]
        public async Task< BaseResponse<ContactMemberDetails>> UserOldOrdersSMS([FromQuery]string memberId)
        {
            var response = new BaseResponse<ContactMemberDetails>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { memberId });
                int num = 0;
                if (memberId.Trim().Length <=10 && int.TryParse(memberId, out num))
                {
                    response = await _service.LoginBySms(memberId,"");

                }

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
        [HttpGet]
        [Route("validateLoginBySms")]
        public async Task<BaseResponse<ResponseUserDTO>> ValidateLoginBySms([FromQuery]string memberId, [FromQuery]string code)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, new object[] { memberId });
                string userIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                DtsLoggger.Logger.Info($"start login ip addres => ${userIpAddress}");
                response = await _service.ValidateLoginBySms(memberId, code);
            }
            catch (BusinessException be)
            {
                OnBusinessException(be, response, logItem);
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
        [HttpGet]
        [Route("getMemberHashedData")]
        public MaskDataDTO GetMemberHashedData([FromQuery]string memberId)
        {
            var response = new BaseResponse<MaskDataDTO>();
            var logItem = new LogDTO();

            try
            {
                //OnStart(logItem, new object[] { memberId });

                response = _service.GetMemberHashedData(memberId);
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                //OnEnd(logItem, response);
            }
            return response.Data;
        }

        [HttpPost]
        [Route("UpdateBiometricToken")]
        public async Task<ActionResult> UpdateBiometricToken([FromBody] BiometicsInfoDTO biometicsInfoDTO)
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                DtsLoggger.Logger.Info($"start UpdateBiometricToken for memberid: {biometicsInfoDTO.memberId}");
                response = await _service.UpdateBiometricToken(biometicsInfoDTO);
                DtsLoggger.Logger.Info($"finish UpdateBiometricToken for memberid: {biometicsInfoDTO.memberId}");
            }
            catch (Exception e)
            {
                OnException(e, response, logItem);
            }
            finally
            {
                OnEnd(logItem, response);
            }
            return Ok();
        }


        /// <summary> 
        ///  The method will mark the member as inactive if he has no operations in the last X years
        /// </summary> 
        /// <param name="memberId">
        ///	</param> 
        /// <returns>
        ///  Returns true if the user has been marked as deleted, false otherwise
        /// </returns>
        [HttpPost]
        [Route("deleteMember")]
        public async Task<BaseResponse<bool>> DeleteMember()
        {
            var response = new BaseResponse<bool>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, null);

                response = await _service.DeleteMember();

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
        ///  The method will XXXXXXXXXXXXXX
        /// </summary> 
        /// <param name="IdentityNumber">
        ///	</param> 
        /// <returns>
        ///  Returns  XXXXXXXXXXXXXXXXXXXXX
        /// </returns>




        [AllowAnonymous]
        [HttpPost]
        [Route("joinBySms")]
        public async Task<BaseResponse<ResponseUserDTO>> JoinBySMS([FromBody] JoinBySmsDTO user)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, null);

                response = await _service.JoinBySMS(user.IdentityNumber);

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
        ///  The method will XXXXXXXXXXXXXX
        /// </summary> 
        /// <param name="IdentityNumber">
        /// <param name="code">




        ///	</param> 
        /// <returns>
        ///  Returns  XXXXXXXXXXXXXXXXXXXXX
        /// </returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("validateJoinBySMS")]
        public async Task<BaseResponse<ResponseUserDTO>> ValidateJoinBySMS([FromBody] JoinBySmsDTO user)
        {
            var response = new BaseResponse<ResponseUserDTO>();
            var logItem = new LogDTO();

            try
            {
                OnStart(logItem, null, null);

                response = await _service.ValidateJoinBySMS(user.IdentityNumber, user.code);

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