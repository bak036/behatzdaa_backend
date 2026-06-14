using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nofshonit.Common.Interfaces
{
    public interface IUserBL
    {
        Task<List<Nofshonit.Common.EF.Club.AllMembers>> GetAllMembers(int top = 100);

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


        Task<BaseResponse<bool>> UserSlinkSMS();

        BaseResponse<ResponseUserDTO> GetUserByAccessToken(string accessToken);
        Task<BaseResponse<ContactMemberDetails>> LoginBySms(string memberId, string hash);

        BaseResponse<MaskDataDTO> GetMemberHashedPhoneNumberAndEmail(string memberID);
        Task<BaseResponse<ResponseUserDTO>> ValidateLoginBySms(string memberId, string code);
        Task<BaseResponse<bool>> UpdateBiometricToken(BiometicsInfoDTO biometicsInfoDTO);

        Task<BaseResponse<bool>> DeleteMember();

        Task<BaseResponse<ResponseUserDTO>> JoinBySmsAsync(string IdentityNumber);

        Task<BaseResponse<ResponseUserDTO>> ValidateJoinBySmsAsync(string IdentityNumber, string code);
        Task<int> ValidatePinCode(string memberId, string inputPinCode);


    }
}