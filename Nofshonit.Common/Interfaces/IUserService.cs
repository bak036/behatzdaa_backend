using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.Infrastructure;

namespace Nofshonit.Common.Interfaces
{
    public interface IUserService
    {

        Task<List<Nofshonit.Common.EF.Club.AllMembers>> GetAllMembers(int top = 0);

        Task<BaseResponse<ResponseUserDTO>> Authenticate(AuthenticateUserRequestDTO userRequestDto,bool test=false);

        Task<BaseResponse<ResponseUserDTO>> CreateNewUser(UserDTO userDto);

        Task<BaseResponse<ResponseUserDTO>> UpdatePassword(UserPasswordInfoDTO userPasswordInfoDto);

		Task<BaseResponse<string>> RecoverPassword(string memberId);

        BaseResponse<ResponseUserDTO> GetCurrentUser();

        void LogOut();

        Task<BaseResponse<ResponseUserDTO>> UpdateMember(UpdateMamberUserDTO updateMamberUserDTO);
        Task<bool> UpdateMemberCookiesAcceptedDate();

        Task<IDFValidationResponseDTO> MinistryOfDefenceValidation(string memberId);

		BaseResponse<ResponseUserDTO> ValidatePasswordForRecover(string tokenCode, string memberId);

		//Silent Login


        Task<BaseResponse<bool>> UserSlinkSMS();

        BaseResponse<ResponseUserDTO> GetUserByAccessToken(string accessToken);
        Task<BaseResponse<ContactMemberDetails>> LoginBySms(string memberId,string hash);
        BaseResponse<MaskDataDTO> GetMemberHashedData(string memberID);
        Task<BaseResponse<ResponseUserDTO>> ValidateLoginBySms(string memberId, string code);
        Task<BaseResponse<bool>> UpdateBiometricToken(BiometicsInfoDTO biometicsInfoDTO);

        Task<BaseResponse<bool>> DeleteMember();

        Task<BaseResponse<ResponseUserDTO>> JoinBySMS(string IdentityNumber);
        Task<BaseResponse<ResponseUserDTO>> ValidateJoinBySMS(string IdentityNumber, string code);
        Task<int> ValidatePinCode(string memberId, string pincode);
    }
}
