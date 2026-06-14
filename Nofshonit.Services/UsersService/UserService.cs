using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Services.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nofshonit.Services.UsersService
{
    public class UserService : BaseService, IUserService
    {
        public UserService()
        {
        }

        public async Task<List<Nofshonit.Common.EF.Club.AllMembers>> GetAllMembers(int top = 100)
        {
            return await Container.Resolve<IUserBL>().GetAllMembers(top);
        }
        
        public async Task<BaseResponse<ResponseUserDTO>> Authenticate(AuthenticateUserRequestDTO userRequestDto,bool test=false)
        {
            var organizationKey = Container.Resolve<IContextManager>().CurrentOrganization().DBName;
            return await Container.ResolveByOrganization<IUserBL>(organizationKey).Authenticate(userRequestDto,test);
        }

        public Task<BaseResponse<ResponseUserDTO>> CreateNewUser(UserDTO userDto)
        {
            return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).CreateNewUser(userDto);
        }

        public async Task<BaseResponse<ResponseUserDTO>> UpdatePassword(UserPasswordInfoDTO userPasswordInfoDto)
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).UpdatePassword(userPasswordInfoDto);
        }

        public async Task<BaseResponse<ResponseUserDTO>> UpdateMember(UpdateMamberUserDTO updateMamberUserDTO)
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).UpdateMember(updateMamberUserDTO);
        }

        public async Task<bool> UpdateMemberCookiesAcceptedDate()
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).UpdateMemberCookiesAcceptedDate();
        }

        public Task<IDFValidationResponseDTO> MinistryOfDefenceValidation(string memberId)
        {
            return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).MinistryOfDefenceValidation(memberId);
        }

		public BaseResponse<ResponseUserDTO> GetCurrentUser()
		{
			return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).GetCurrentUser();
		}

        public void LogOut()
        {
            Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).LogOut();
        }

        public Task<BaseResponse<string>> RecoverPassword(string memberId)
		{
			return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).RecoverPassword(memberId);
		}

        public BaseResponse<ResponseUserDTO> ValidatePasswordForRecover(string tokenCode, string memberId)
		{
			return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).ValidatePasswordForRecover(tokenCode, memberId);
		}

        public async Task<BaseResponse<bool>> UserSlinkSMS()
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).UserSlinkSMS();
        }

        public BaseResponse<ResponseUserDTO> GetUserByAccessToken(string accessToken)
        {
            return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).GetUserByAccessToken(accessToken);
        }

        public async Task<BaseResponse<ContactMemberDetails>> LoginBySms(string memberId,string hash)
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).LoginBySms(memberId,hash);
        }

        public BaseResponse<MaskDataDTO> GetMemberHashedData(string memberId)
        {
            return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).GetMemberHashedPhoneNumberAndEmail(memberId);
        }
        public Task<BaseResponse<ResponseUserDTO>> ValidateLoginBySms(string memberId, string code)
        {
            return Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).ValidateLoginBySms(memberId, code);
        }

        public async Task<BaseResponse<bool>> UpdateBiometricToken(BiometicsInfoDTO biometicsInfoDTO)
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).UpdateBiometricToken(biometicsInfoDTO);
        }

        public async Task<BaseResponse<bool>> DeleteMember()
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).DeleteMember();
        }

        public async Task<BaseResponse<ResponseUserDTO>> JoinBySMS(string IdentityNumber)
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).JoinBySmsAsync(IdentityNumber);

        }

        public async Task<BaseResponse<ResponseUserDTO>> ValidateJoinBySMS(string IdentityNumber, string code)
        {
            return await Container.ResolveByOrganization<IUserBL>(Container.Resolve<IContextManager>().CurrentOrganization().DBName).ValidateJoinBySmsAsync(IdentityNumber, code);

        }
        public async Task<int> ValidatePinCode(string memberId, string pincode)
        {
            var organizationKey = Container.Resolve<IContextManager>().CurrentOrganization().DBName;
            return await Container.ResolveByOrganization<IUserBL>(organizationKey).ValidatePinCode(memberId, pincode);
        }


    }
}