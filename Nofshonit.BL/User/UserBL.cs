using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nofshonit.BL.BLHelper;
using Nofshonit.BL.RestApiGW;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Nofshonit.Common.DTOs.MapperManagement;
using Nofshonit.Common.DTOs.Pulseem;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.BL.Exceptions;
using Nofshonit.Common.FeaturesManagement;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Common.Interfaces;
using Nofshonit.Common.Utils;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils;
using Nofshonit.Repositories.ClubModel;
using Nofshonit.Repositories.DtsOnlineModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.BL.Config;
using Microsoft.EntityFrameworkCore;
using Nofshonit.Logs;

namespace Nofshonit.BL.User
{
    public class UserBL : BaseBL, IUserBL
    {
        private IClubRepo _clubRepo;
        private IMapperManager _mapper;
        private List<int> messageKeys;
        private int lastLoginAttemptsMinExpiry;
        private int lastLoginAttemptsExpiry;
        private int updateDetailsMonthExpiry;
        private int recoverPasswordMinExpiry;
        private int userTokenExpiry;
        private int lastLoginAllowedPeriod;
        private string allowedFullLoginIPs;
        private IConfigBL _configBL;
        private IConfigurationManager _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        private IAddressesBL _addressesBL;
        private IContactUsBL _contactUsBL;
        private IDtsOnlineRepo _dtsOnlineRepo;
        private IContextManager _contextManager;
        private IRestApiGW _restApiGW;
        private ICacheManager Cache;


        public UserBL() : base()
        {
            Cache = Container.Resolve<ICacheManager>();
            _httpContextAccessor = Container.Resolve<IHttpContextAccessor>();
            _addressesBL = Container.Resolve<IAddressesBL>();
            _contactUsBL = Container.Resolve<IContactUsBL>();
            _dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
            _contextManager = Container.Resolve<IContextManager>();
            _configBL = Container.Resolve<IConfigBL>();
            _clubRepo = Container.Resolve<IClubRepo>();
            _mapper = Container.Resolve<IMapperManager>();
            _configuration = Container.Resolve<IConfigurationManager>();
            _restApiGW = Container.Resolve<IRestApiGW>();
            messageKeys = new List<int>() { 10041, 10234, 10311, 722, 736, 723, 1021, 10022, 10260, 10261, 10262, 10023, 728, 10307, 1044, 10240, 10241, 10242, 10243, 10044, 10255, 10256, 10279, 10037, 10277, 10275, 10024, 10290, 10244, 223, 340, 729, 40910, 67, 51136 };
            lastLoginAttemptsMinExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.LastLoginAttemptsMinExpiry);
            lastLoginAttemptsExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.LastLoginAttemptsExpiry);
            updateDetailsMonthExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.UpdateDetailsMonthExpiry);
            recoverPasswordMinExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.MinutesToExpired_RecoverPassword);
            lastLoginAllowedPeriod = _configuration.GetConfigByValue<int>(ConfigurationKey.AllowedLoginPeriod);
            userTokenExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.MinutesToExpired_UserToken);
            allowedFullLoginIPs = _configuration.GetConfigByValue<string>(ConfigurationKey.AllowedFullLoginIPs);

        }

        public async Task<List<Common.EF.Club.AllMembers>> GetAllMembers(int top = 100)
        {
            return await _clubRepo.GetAllMembers(top);

        }

        public async Task<BaseResponse<ResponseUserDTO>> Authenticate(AuthenticateUserRequestDTO userRequestDto, bool test = false) // test param is for unit tests 
        {
            
            var currentUserIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Request.Headers.GetUserIP();
            var allowedIPs = allowedFullLoginIPs;


            if (!allowedIPs.Contains(currentUserIp))
            {
                if (userRequestDto.AuthenticateOrJoin == EAuthenticateOrJoin.Authenticate && 
                    (userRequestDto.LoginType == ELoginType.MemberIdAndPassword || userRequestDto.LoginType == ELoginType.BiometricToken))
                {
                    throw new BusinessException("באופן זמני הכניסה תתאפשר באמצעות קוד חד פעמי בלבד. (לא תתאפשר כניסה עם סיסמא או עם זיהוי ביומטרי)");
                }
            }



            switch (userRequestDto.LoginType)
            {
                case ELoginType.MemberIdAndPassword:
                    #region MemberIdAndPassword 
                    var isMember = new MinistryOfDefenceValidationResponseDTO();
                    switch (userRequestDto.AuthenticateOrJoin)
                    {
                        case EAuthenticateOrJoin.Authenticate:
                            return await Authenticate_Login(userRequestDto, test);

                        case EAuthenticateOrJoin.Join:
                            return await Authenticate_Join(userRequestDto, test);

                        default:
                            throw new Exception("No Valid AuthenticateOrJoin has given as a detail.");
                    }
                #endregion
                case ELoginType.BiometricToken:
                    #region BiometricToken
                    //throw new BusinessException("באופן זמני הכניסה תתאפשר באמצעות קוד חד פעמי בלבד. (לא תתאפשר כניסה עם סיסמא או עם זיהוי ביומטרי)");
                    return await Authenticate_BiometricToken(userRequestDto);
                #endregion
                default:
                    throw new Exception("No Valid Login Type.");
            }

        }

        private async Task<BaseResponse<ResponseUserDTO>> Authenticate_BiometricToken(AuthenticateUserRequestDTO userRequestDto)
        {
            string memberId = userRequestDto.Id;
            ApiLoggerBL.LogConnectorData(memberId);
            bool verified = false;
            DtsLoggger.Logger.Info($"start Authenticate_BiometricToken for memberid: {userRequestDto.Id}");
			//string publicKeyFromDb = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAp6HzbSgZPkJPfZJWydFAKdzUWlQcGHCTZhghg8HwHOfRZp3QZ/iiDORVzdIlW6XYPz76aAn8Nxm/v4NbsQsFPbwIcc7CPOJe21VT+7f6ocZ4kef0dqxUOGuK1FynrqzsAeYoaeTW+w/HElXODOEzZs3CfyE3d4hy3TTM/mVyQGV1FO/hHWB/zXq7ryQ8hXP/ueJimmJvitB7UweemRxvEYfVx52VVAgzg1RqVWeRj8L/obfm0lwQtIAHdDOnIi/cwpsyKQNikjMsf4dFgt14fcOgFdSG06jB840GnOsRZM04CWZQ9ttwAvoNGK/zjriRYGySQ4Ey0K0l5G3UVr56mQIDAQAB";
			var userFromDb = await _clubRepo.GetUserByUserID(memberId);
            string publicKeyFromDb = _clubRepo.getPublicKeyById(userFromDb);
            DtsLoggger.Logger.Info($"publicKey for memberid: {userRequestDto.Id}, {publicKeyFromDb}");
            DtsLoggger.Logger.Info($"paylod for memberid: {userRequestDto.Id}, {userRequestDto.PaylodBiometricToken}");
            DtsLoggger.Logger.Info($"privateKey for memberid: {userRequestDto.Id}, {userRequestDto.PrivateBiometricToken}");
            byte[] data = Convert.FromBase64String(userRequestDto.PaylodBiometricToken);
            //byte[] data = Convert.FromBase64String("dGF0b0Bmcm9td2luMzIuY29t");
            byte[] signature = Convert.FromBase64String(userRequestDto.PrivateBiometricToken);
            //byte[] signature = Convert.FromBase64String("lWKRRgWBA2lBAfUvBS+54s9kmHTH3nJwcvYYmjCg5QpWQ9joY7Rzpq0zZjOhyxASXoAN4Vz8+mqSqPWi/4DFH7947ZWZSbopPfxiI7jjDRMAVymG0B+dRVjiMow48ZvhgP/FGSZqeLAei77Z0aAmwN2TBxkClqBpt9uy+nkI7V/TJGAbbLcWfiPWNVOGsU0smoFDQLlJjkocahNSOqjj+9PPFVqbc/VVHQWsSoq1ZxtCPILFwPCCtUCDITXrU/riGMFJ282p/3rfhDJKYis9/izR98/zgBLRoCew8zu8Za4UNWaHaR3HP/6voQI2NiVSKtss1VjvwjwXYIOh56yeSw==");
            byte[] publicKey = Convert.FromBase64String(publicKeyFromDb);

            byte[] modulus;
            byte[] exponent;
            ExtractPublicKeyParameters(publicKey, out modulus, out exponent);

            using (var rsa = new RSACryptoServiceProvider())
            {
                // Create parameters
                var rsaParam = new RSAParameters()
                {
                    Modulus = modulus,
                    Exponent = exponent
                };

                // Import public key
                rsa.ImportParameters(rsaParam);

                // Create signature verifier with the rsa key
                var signatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);

                // Set the hash algorithm to SHA256.
                signatureDeformatter.SetHashAlgorithm("SHA256");

                // Compute hash
                byte[] hash;
                using (SHA256 sha256 = SHA256.Create())
                {
                    hash = sha256.ComputeHash(data);
                }

                verified = signatureDeformatter.VerifySignature(hash, signature);
                if (!verified)
                    throw new BusinessException("זיהוי ביומטרי שגוי");
            }

            int? premiumTypeBeforeChenged = userFromDb.PremiumType;
            UserDTO userDto = _mapper.Map(userFromDb, typeof(UserDTO));

            userFromDb = await CalculatePremiumTypeAndUpdateMember(userFromDb);
            userDto.ClubCreditCard = userFromDb.ClubCreditCard;
            if (premiumTypeBeforeChenged != userFromDb.PremiumType)
            {
                userDto.IsPremiumTypeChanged = true;
                _clubRepo.RemoveAllProductsByMemberId(userFromDb.MemberId);
            }

            var ableToTryAgain = _clubRepo.CheckIfAbleToTryLogin(userFromDb);
            if (!ableToTryAgain)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10041).MessageText, 10041);


            if ((userFromDb != null && userFromDb.Active == 0))
            {
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 223).MessageText);

            }

            if (userRequestDto.LoginType == ELoginType.BiometricToken && userFromDb.PrivacyPolicyAcceptedDate == null && userRequestDto.ConfirmPrivacyPolicy != true)
                throw new BusinessException("<p>NeedToConfirmPrivacyPסlicy</p>");

            //אין זכאות על פי משרד הביטחון ולא ביצע התחוברות מעולם
            if ((userFromDb != null && userFromDb.PremiumType == (int)EIDFPremiumeType.NotApproved && userFromDb.UserToken == null && userFromDb.EncryptedUserPassword == null && userFromDb.LastEncryptedUserPassword == null))
            {
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);

            }
            else
            {
                CreateUserToken(userDto.Id);
                userDto.LoginType = ELoginType.BiometricToken;
                await _clubRepo.UpdateLoginAttempts(true, userFromDb);
            }

            //Eligibility expired 
            if (userFromDb.PremiumType == (int)EIDFPremiumeType.NotApproved)
            {
                userDto.UpdateOrRegister = ENeedUpdateOrFinishRegistration.UserExpired;
                return new BaseResponse<ResponseUserDTO>
                {
                    Status = true,
                    Data = new ResponseUserDTO(userDto),
                    ErrorDescription = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 736).MessageText
                };
            }

            ContextManager.SetCurrentUserCache(userDto.Id);
            await _clubRepo.UpdateLastLogin(userFromDb);

            // first login After reset all users passwords
            if (string.IsNullOrEmpty(userFromDb.EncryptedUserPassword) && userFromDb.PremiumType != (int)EIDFPremiumeType.NotApproved)
            {
                userDto.ForgetPasswordToken = Guid.NewGuid().ToString();
                // ContextManager.SetCurrentUserTokenLoginCache(userDto.Id, userDto.Token);

                return new BaseResponse<ResponseUserDTO>
                {
                    Status = true,
                    Data = new ResponseUserDTO(userDto),
                    ErrorDescription = MessagesUtil.GetMessagesByKey(messageKeys)
                                                   .FirstOrDefault(m => m.MessageKey == (string.IsNullOrEmpty(userFromDb.LastEncryptedUserPassword) ? 10242 : 10307)).MessageText
                };
            }

            //  _httpContextAccessor.HttpContext.Session.SetInt32("Login_Source", (int)ELoginType.OTPShortCode);


            return new BaseResponse<ResponseUserDTO>
            {
                Status = true,
                Data = new ResponseUserDTO(userDto),
            };
        }

        // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
        static readonly byte[] SeqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };



        public static void ExtractPublicKeyParameters(byte[] publicKey, out byte[] modulus, out byte[] exponent)
        {
            modulus = new byte[0];
            exponent = new byte[0];

            byte[] seq = new byte[15];

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(publicKey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return;

                seq = binr.ReadBytes(15);       //read the Sequence OID
                if (!CompareBytearrays(seq, SeqOid))    //make sure Sequence for OID is correct
                    return;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8203)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return;

                bt = binr.ReadByte();
                if (bt != 0x00)     //expect null byte next
                    return;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes
                else
                    return;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                    lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte(); //advance 2 bytes
                    lowbyte = binr.ReadByte();
                }
                else
                    return;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                int modsize = BitConverter.ToInt32(modint, 0);

                int firstbyte = binr.PeekChar();
                if (firstbyte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it
                    binr.ReadByte();    //skip this null byte
                    modsize -= 1;   //reduce modulus buffer size by 1
                }

                modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                    return;
                int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                exponent = binr.ReadBytes(expbytes);
            }

            finally
            {
                binr.Close();
            }

        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        private async Task<Common.EF.Club.AllMembers> CalculatePremiumTypeAndUpdateMember(Common.EF.Club.AllMembers userFromDb)
        {
            var isMember = await CalculatePremiumTypeForMember(userFromDb);
            var userFromDbRes = _clubRepo.SetIdfDataForMember(isMember).Result;
            userFromDbRes.PremiumType = (int)isMember.premiumeType;
            return userFromDbRes;
        }
        private async Task<IDFValidationResponseDTO> CalculatePremiumTypeForMember(Common.EF.Club.AllMembers userFromDb, bool isJoin = false, bool addRowToAllowanceHistory = true)
        {
			var member = _clubRepo.GetMemberFromAllowedRegister(userFromDb.MemberId);
			if (member is not null)
			{
				return new IDFValidationResponseDTO() { Zehut = userFromDb.MemberId, isValid = true, darga = EDarga.Miluim, premiumeType = (EIDFPremiumeType)member.PremiumType , ClubCreditCard = userFromDb.ClubCreditCard > 0};
			}

			var isMember = MinistryOfDefenceValidation(userFromDb.MemberId).Result;
            if (!isMember.isValid.HasValue && isJoin)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10262).MessageText);
            EIDFPremiumeType PremiumeTypeFromMinistry = isMember.premiumeType;
            bool IsMemberHaveMaxBehatsdaaCreditCard = false;
            DtsLoggger.Logger.Info("Login MinistryOfDefenceValidation finish" + "  - USER_ID -  " + userFromDb.MemberId);

            if (CheckUserHaveMaxBehatsdaaCreditCard(userFromDb.MemberId).Result)
            {
                isMember.ClubCreditCard = true;
                IsMemberHaveMaxBehatsdaaCreditCard = true;
                var res = _clubRepo.UpdateClubCreditCard(userFromDb).Result;
            }

            //calculate PremiumType type for member
            if (isMember.premiumeType == EIDFPremiumeType.Approved)
                isMember.premiumeType = EIDFPremiumeType.Approved;
            else if (IsMemberHaveMaxBehatsdaaCreditCard|| isMember.premiumeType == EIDFPremiumeType.Allowed) 
                isMember.premiumeType = EIDFPremiumeType.Allowed;
            else
                isMember.premiumeType = EIDFPremiumeType.NotApproved;


            //add row to allowanceHistory only if premiumeType is changed, and only for registered members
            if (userFromDb.IdentityGuid != null && userFromDb.IdentityGuid.Length > 0 && userFromDb.PremiumType != (int)isMember.premiumeType && addRowToAllowanceHistory)
            {
                if (userFromDb.AllowSmsAndMail ?? false)
                {
                    UserDTO userDto = _mapper.Map(userFromDb, typeof(UserDTO));
                    this.RemoveClientFromList(userDto);
                    userDto.PremiumType = (int)isMember.premiumeType;
                    await this.AddClientToList(userDto);
                }
                AllowanceHistory allowanceHistory = new AllowanceHistory()
                {
                    MemberID = userFromDb.MemberId,
                    InsertDate = DateTime.Now,
                    IDFResponse = (short)PremiumeTypeFromMinistry,
                    PremiumType = (short)isMember.premiumeType,
                    ClubCreditCard = IsMemberHaveMaxBehatsdaaCreditCard
                };
                _clubRepo.AddMemberToAllowanceHistory(allowanceHistory);
            }
            return isMember;
        }
        private async Task<BaseResponse<ResponseUserDTO>> Authenticate_Login(AuthenticateUserRequestDTO userRequestDto, bool test = false)
        {
            var userFromDbCheck = await _clubRepo.GetUserByUserID(userRequestDto.IdentityNumber);

            if (userFromDbCheck != null && userFromDbCheck.MemberId.Equals(userRequestDto.Password))
            {
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
            }

            if (userFromDbCheck != null && userFromDbCheck.MemberId.Equals(userFromDbCheck.EncryptedUserPassword))
            {

                await _clubRepo.UpdateLoginAttempts(false, _mapper.Map(userFromDbCheck, typeof(UserDTO)));
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10279).MessageText);
            }

          

            if ((userFromDbCheck != null && userFromDbCheck.Active == 0))
            {
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 223).MessageText);

            }

            userRequestDto.IdentityNumber.PadLeft(9, '0');
            userRequestDto.Password = Cryptor.MD5Encrypt(userRequestDto.Password);
            userRequestDto.Id = userRequestDto.IdentityNumber;
            var userDto = (UserDTO)_mapper.Map(await _clubRepo.GetUserByUserDTO(_mapper.Map(userRequestDto, typeof(UserDTO))), typeof(UserDTO));

            if (userDto != null)
            {
                var userUpdated = _clubRepo.GetUserByUserIDSync(userDto.Id);
                userDto = _mapper.Map(userUpdated, typeof(UserDTO));
                userDto.LoginType = userRequestDto.LoginType;

                //5 falied login attempts 
                if (userDto.LoginAttempts >= lastLoginAttemptsExpiry && (DateTime.Now - userDto.LastLoginAttempts.Value).Minutes < lastLoginAttemptsMinExpiry)
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10041).MessageText, 10041);

                DtsLoggger.Logger.Info("Login UpdateLoginAttempts start" + "  - USER_ID -  " + userRequestDto.Id);
                await _clubRepo.UpdateLoginAttempts(true, userFromDbCheck);
                DtsLoggger.Logger.Info("Login UpdateLoginAttempts finish" + "  - USER_ID -  " + userRequestDto.Id);
                DtsLoggger.Logger.Info("Login GetUserByUserDTO start" + "  - USER_ID -  " + userRequestDto.Id);
                var userFromDb = await _clubRepo.GetUserByUserDTO(userDto);
                DtsLoggger.Logger.Info("Login GetUserByUserDTO finish" + "  - USER_ID -  " + userRequestDto.Id);
                //CreateUserToken(userDto.Id);

                if (userFromDb != null)
                {
                    DtsLoggger.Logger.Info("Login MinistryOfDefenceValidation start" + "  - USER_ID -  " + userRequestDto.Id);
                    userFromDb = await CalculatePremiumTypeAndUpdateMember(userFromDb);
                    userDto = (UserDTO)_mapper.Map(userFromDb, typeof(UserDTO));
                    if (userFromDbCheck.PremiumType != userFromDb.PremiumType)
                    {
                        userDto.IsPremiumTypeChanged = true;
                        _clubRepo.RemoveAllProductsByMemberId(userFromDb.MemberId);
                    }

                    if (userDto.PremiumType == (int)EIDFPremiumeType.NotApproved)
                    {
                        CreateUserToken(userDto.Id, true);
                    }
                    else
                    {
                        CreateUserToken(userDto.Id);
                    }

                    var userCache = ContextManager.SetCurrentUserCache(userDto.Id);
                    userDto.UpdateOrRegister = userCache.UpdateOrRegister;
                    if (userFromDb == null)
                    {
                        string msg = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10234).MessageText;
                        throw new BusinessException(msg);
                    }

                  

                    if (isMemberDetailsMissing(userFromDb.Email, userFromDb.PhoneNumber, userFromDb.MobilePhone, userFromDb.EncryptedUserPassword))
                        throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10279).MessageText);

                    if (userDto.UpdateOrRegister == null && NeedToUpdateDetails(ref userDto))
                    {
                        return new BaseResponse<ResponseUserDTO>
                        {
                            Status = true,
                            Data = new ResponseUserDTO(userDto),
                            ErrorDescription = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10242).MessageText
                        };
                    }

                    await _clubRepo.UpdateLastLogin(userFromDb);

                    var userID = ContextManager.CurrentUser().Id;
                    ContextManager.SetCurrentUserCache(userID);

                    ApiLoggerBL.LogConnectorData(userID);


                    if (string.IsNullOrEmpty(userFromDb.MemberCardNumber) || await _clubRepo.ShouldAddCard())
                    {
                        var addCard = _clubRepo.AddCardToMember(userID, false);
                        if (!addCard)
                        {
                            throw new BusinessException("Failed Adding New Card To Member" + "  - USER_ID -  " + userRequestDto.Id);
                        }

                    }

                    //2. בדיקה מול DB
                    if (userDto.PremiumType == (int)EIDFPremiumeType.NotApproved)
                    {
                        userDto.UpdateOrRegister = ENeedUpdateOrFinishRegistration.UserExpired;
                        return new BaseResponse<ResponseUserDTO>
                        {
                            Status = true,
                            Data = new ResponseUserDTO(userDto),
                            ErrorDescription = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 736).MessageText
                        };
                    }

                    if (userDto.PremiumType != (int)EIDFPremiumeType.NotApproved)
                    {

                        return new BaseResponse<ResponseUserDTO>
                        {
                            Status = true,
                            Data = new ResponseUserDTO(userDto),
                        };
                    }

                    if (userDto.PremiumType == (int)EIDFPremiumeType.NotApproved)
                    {
                        throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 736).MessageText);
                    }

                }
                throw new Exception("Any Problem with converting the user. (Authenticate)");
            }
            DtsLoggger.Logger.Info("Login UpdateLoginAttempts start" + "  - USER_ID -  " + userRequestDto.Id);
            var ableToTryAgain = await _clubRepo.UpdateLoginAttempts(false, _mapper.Map(userRequestDto, typeof(UserDTO)));
            DtsLoggger.Logger.Info("Login UpdateLoginAttempts finish" + "  - USER_ID -  " + userRequestDto.Id);

            if (!ableToTryAgain)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10041).MessageText);

            // Customer service receives calls about users without any credentials the can be used to authenticate the user,
            // if the member is missing one of those credentials update his password to his member id.
            //TODO check if need to remove
            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
        }


        private async Task<bool> CheckUserHaveMaxBehatsdaaCreditCard(string memberId)
        {

            LeumiCardSvc.AuthenticationAnswer res;
            try
            {
                LeumiCardSvc.LCBehatsdaaClient lCBehatsdaa = new LeumiCardSvc.LCBehatsdaaClient();
                bool? skipValidation = Container.Resolve<IConfigurationManager>().GetConfigByValue<bool>(ConfigurationKey.SkipMaxCardValidation);
                if (skipValidation.HasValue && skipValidation.Value)
                    return true;
                res = await lCBehatsdaa.AuthenticationAsync(memberId, String.Empty, "3");
                if (res != null)
                {
                    DtsLoggger.Logger.Info("lCBehatsdaa.AuthenticationAsync Member =>{0}, RC => {1} :" + res.RC_DESC, memberId, res.RC);
                    if (res.RC == 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, "Error On CheckUserHaveMaxBehatsdaaCrditCard");
            }
            return false;
        }

        private bool isMemberDetailsMissing(string userEmail, string userPhoneNumber, string userMobile, string encryptedUserPassword)
        {
            return ((string.IsNullOrEmpty(userEmail) &&
                  (string.IsNullOrEmpty(userMobile) && string.IsNullOrEmpty(userPhoneNumber))));

        }

        private async Task<BaseResponse<ResponseUserDTO>> Authenticate_JoinOld(AuthenticateUserRequestDTO userRequestDto, bool test = false)
        {
            userRequestDto.IdentityNumber.PadLeft(9, '0');
            userRequestDto.Password.PadLeft(9, '0');
            // List<string> ClubCreditCardFactorySymbolExclusionPay = (await _configBL.GetValueByKey("ClubCreditCardFactorySymbolExclusionPay"))[0].Value.Split(',').ToList();

            //זכאות משרד הביטחון
            var isMember = await MinistryOfDefenceValidation(userRequestDto.IdentityNumber);
            ///bool isExclude = await _clubRepo.IsMemberAllowedRegister(userRequestDto.IdentityNumber);
            //אין תקשורת עם משרד הביטחון
            if (!isMember.isValid.HasValue)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10262).MessageText);

            var userFromDb = await _clubRepo.GetUserByUserID(userRequestDto.IdentityNumber);

            if (userFromDb != null)
            {
                if (isMember.isValid.HasValue && userFromDb.Active == 0)
                {
                    ////Tami Todo - message number?
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10240).MessageText);
                }
            }

            if (isMember.isValid.HasValue && isMember.isValid.Value && isMember.premiumeType != EIDFPremiumeType.NotApproved)
            {
                var userDto = (UserDTO)_mapper.Map(userRequestDto, typeof(UserDTO));
                //GenerateIsMemberResult(isMember.MemberStatus.Value);
                //userDto.MemberSpecialID = isMember.MemberSpecialID;
                userDto.RegistrationDate = DateTime.Now;
                userDto.IdentityGuid = Guid.NewGuid().ToString();
                userDto.Id = userRequestDto.IdentityNumber;
                userDto.Darga = (int)isMember.darga;
                userDto.PremiumType = (int)isMember.premiumeType;
                //if (isMember.MemberSpecialID.Equals("0000103166") || isMember.MemberSpecialID.Equals("0000362509") || isMember.MemberSpecialID.Equals("103166") || isMember.MemberSpecialID.Equals("362509"))

                userDto.ClubCreditCard = (await CheckUserHaveMaxBehatsdaaCreditCard(userDto.Id)) ? 1 : 0;


                return await CreateBasicNewUser(userDto);
            }
            else
            {
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 736).MessageText);
            }
            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 723).MessageText);
        }

        private async Task<BaseResponse<ResponseUserDTO>> Authenticate_Join(AuthenticateUserRequestDTO userRequestDto, bool test = false)
        {
            userRequestDto.IdentityNumber.PadLeft(9, '0');
            //זכאות משרד הביטחון
            Nofshonit.Common.EF.Club.AllMembers member = new Nofshonit.Common.EF.Club.AllMembers() { MemberId = userRequestDto.IdentityNumber };
            ApiLoggerBL.LogConnectorData(userRequestDto.IdentityNumber);
            IDFValidationResponseDTO isMember = await CalculatePremiumTypeForMember(member, true);
            //if member exist in BehatzdaaNoCards table, member is allowed
            string json = JsonConvert.SerializeObject(isMember).Replace("\"", " ").Replace(@"\", " ").Replace("{", "").Replace("}", "");
            string message = string.Format("Authenticate_Join, answer from MinistryOfDefenceValidation for IdentityNumber {0} is {1}", userRequestDto.IdentityNumber, json);
            DtsLoggger.Logger.Info(message);
            bool isHavaMaxCard = false;

            if (isMember.premiumeType != EIDFPremiumeType.NotApproved)
            {
                string leadingZeroMobilePhone = isMember.cellPhone.PadLeft(10, '0');

                RegistrationAudits regAuditFromDb = await _clubRepo.GetRegistrationAuditByIdentity(userRequestDto.IdentityNumber);
                if (regAuditFromDb == null)
                {

                    RegistrationAudits registrationAudits = new RegistrationAudits
                    {
                        Cellphone = leadingZeroMobilePhone,
                        CreatedDate = DateTime.Now,
                        Darga = (int)isMember.darga,
                        PremiumType = (int)isMember.premiumeType,
                        IdentityNumber = isMember.Zehut,
                        IsMember = (isMember.isValid.HasValue && isMember.isValid.Value == true || (isMember.premiumeType == EIDFPremiumeType.Allowed)) ? 1 : 0,
                        AttemptsCount = 0,
                        ClubCreditCard = isMember.ClubCreditCard ? 1 : 0,
                    };

                    await _clubRepo.CreateNewRegistrationAudit(registrationAudits);
                }
                else
                {
                    regAuditFromDb.Cellphone = leadingZeroMobilePhone;
                    regAuditFromDb.CreatedDate = DateTime.Now;
                    regAuditFromDb.Darga = (int)isMember.darga;
                    regAuditFromDb.PremiumType = (int)isMember.premiumeType;
                    regAuditFromDb.IdentityNumber = isMember.Zehut;
                    regAuditFromDb.IsMember = (isMember.isValid.HasValue && isMember.isValid.Value == true || (isMember.premiumeType == EIDFPremiumeType.Allowed)) ? 1 : 0;
                    //regAuditFromDb.AttemptsCount = 0;
                    regAuditFromDb.ClubCreditCard = isMember.ClubCreditCard ? 1 : 0;
                    _clubRepo.UpdateNewRegistrationAudit(regAuditFromDb);
                }

                string maskedPhone = string.Format("{1}****-{0}", leadingZeroMobilePhone.Substring(0, 3), leadingZeroMobilePhone.Substring(leadingZeroMobilePhone.Length - 3, 3));

                UserDTO user = new UserDTO
                {
                    IdentityNumber = userRequestDto.IdentityNumber,
                    MobilePhone = maskedPhone,

                };

                return new BaseResponse<ResponseUserDTO>
                {
                    Status = true,
                    Data = new ResponseUserDTO(user),
                    //Message = string.Format(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 51105).MessageText, maskedPhone),
                };
            }
            else
            {
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 67).MessageText);

            }



        }

        private async Task<BaseResponse<ResponseUserDTO>> CreateBasicNewUser(UserDTO userDto)
        {
            //if (await _clubRepo.GetUserByUserID(userDto.IdentityNumber) == null)
            var userFromDb = await _clubRepo.GetUserByUserID(userDto.IdentityNumber);
            //מבוצע בלוגאין
            if (userFromDb == null
                || isMemberDetailsMissing(userFromDb.Email, userFromDb.PhoneNumber, userFromDb.MobilePhone, userFromDb.EncryptedUserPassword))
            {
                var convertedUser = (Common.EF.Club.AllMembers)_mapper.Map(userDto, typeof(Common.EF.Club.AllMembers));
                convertedUser.MemberId = GenerateNewUserId(userDto);
                convertedUser.EncryptedUserPassword = Cryptor.MD5Encrypt(userDto.Password);
                convertedUser.UserSiteLastLogin = DateTime.Now;
                convertedUser.DateCreated = DateTime.Now;
                userDto.Id = convertedUser.MemberId;

                convertedUser.Active = 1;
                if (CheckLoginCompatibility(userDto.LoginType))
                {
                    if (await _clubRepo.UpdateNewUser(userFromDb, convertedUser) ||
                        await _clubRepo.CreateNewUser(convertedUser))
                    {

                        var isCardAdded = true;
                        isCardAdded = _clubRepo.AddCardToMember(userDto.Id, false);
                        if (!isCardAdded)
                            throw new Exception("Any problem with AddCardToMember.");

                        CreateUserToken(userDto.Id);
                        await _clubRepo.UpdateLastUpdate(userDto.Id, DateTime.Now, userDto.PremiumType, userDto.Darga);
                        ContextManager.SetCurrentUserCache(convertedUser.MemberId);
                        var responseData = new ResponseUserDTO(_mapper.Map(convertedUser, typeof(UserDTO)));
                        responseData.UpdateOrRegister = ENeedUpdateOrFinishRegistration.FinishRegistration;
                        return await Task.FromResult(new BaseResponse<ResponseUserDTO>
                        {
                            Status = true,
                            Data = responseData
                        });
                    }
                    throw new Exception("Any fail in clubRepo. CreateNewUser.");
                }
                throw new Exception("The feature is not permitted OR The loginType does not match the feature type");
            }
            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10240).MessageText);
        }

        public async Task<BaseResponse<ResponseUserDTO>> UpdateMember(UpdateMamberUserDTO updateMamberUserDTO)
        {
            var userDto = (UserDTO)_mapper.Map(updateMamberUserDTO, typeof(UserDTO));
            userDto.Id = ContextManager.CurrentUser().Id;
            userDto.PremiumType = ContextManager.CurrentUser().PremiumType;
            if (!utils.ValidateCharactersOnly(updateMamberUserDTO.FirstName))
                throw new Exception();
            if (!utils.ValidateCharactersOnly(updateMamberUserDTO.LastName))
                throw new Exception();
            if (!utils.ValidateCharactersOnly(updateMamberUserDTO.FactorySymbol))
                throw new Exception();
            if (!utils.ValidateCharactersOnly(updateMamberUserDTO.CityName) || updateMamberUserDTO.CityName == null)
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.CityId.ToString()))
                throw new Exception();
            if (!utils.ValidateCharactersAndNumbersOnly(updateMamberUserDTO.StreetName))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.ApartmentNumber.ToString()))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.HouseNumber.ToString()))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.MobilePhone))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.PartnerPhone))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.NumOfChildren.ToString()))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.Mailbox))
                throw new Exception();
            if (!utils.ValidateNumbersOnly(updateMamberUserDTO.Zip))
                throw new Exception();


            switch (updateMamberUserDTO.NewOrUpdate)
            {
                case ENewOrUpdate.New:
                    return await UpdateMember_New(updateMamberUserDTO, userDto);
                case ENewOrUpdate.Update:
                    return await UpdateMember_Update(updateMamberUserDTO, userDto);
                default:
                    throw new Exception("No Valid NewOrUpdate has given as a detail.");
            }

        }

        public async Task<bool> UpdateMemberCookiesAcceptedDate()
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            bool res = await _clubRepo.UpdateMemberCookiesAcceptedDate(currentUser.Id);

            currentUser.CookiesAcceptedDate = DateTime.Now;
            Cache.Set(string.Format(CacheKeys.UserId, currentUser.Id), currentUser);

            return res;
        }

        private async Task<BaseResponse<ResponseUserDTO>> UpdateMember_New(UpdateMamberUserDTO updateMamberUserDTO, UserDTO userDto)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            var userFromDb = await _clubRepo.GetUserByUserID(currentUser.Id);

            if (userFromDb != null)
            {


                userDto.Token = userFromDb.UserToken;
                userDto.LastUpdateMember = DateTime.Now;

                if (!new EmailAddressAttribute().IsValid(updateMamberUserDTO.Email))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10243).MessageText);
                if((!string.IsNullOrEmpty(updateMamberUserDTO.Email) || !string.IsNullOrWhiteSpace(updateMamberUserDTO.MobilePhone)) && !updateMamberUserDTO.IsUpdateDetailsApproved)
                    throw new BusinessException("לא ניתן לעדכן פרטים מבלי לאשר את התקנון");
                
                if (updateMamberUserDTO.MobilePhone.Trim() != userFromDb.MobilePhone.Trim())
                    throw new UnauthorizedAccessException("לא ניתן לערוך מספר טלפון ראשי");

                var value = await _clubRepo.UpdateMember(userDto);
                if (value)
                {
                    ContextManager.SetCurrentUserCache(currentUser.Id);
                    _clubRepo.CreateNewUserRequestLog(updateMamberUserDTO.IdentityNumber, updateMamberUserDTO.IsUpdateDetailsApproved);

                    if (updateMamberUserDTO.AllowSmsAndMail ?? false)
                    {
                        try
                        {

                            if (!await AddClientToList(userDto))
                                throw new Exception("Problem with Pulseem service OR any problem with the given details.");
                        }
                        catch (Exception e)
                        {
                            LoggerHelper.Error(e, "Error On UpdateMember_New");

                            //add message to response 
                        }
                    }

                    if (string.IsNullOrEmpty(userFromDb.MemberCardNumber) || await _clubRepo.ShouldAddCard())
                    {
                        bool isCardAdded = _clubRepo.AddCardToMember(userDto.Id, false);
                        if (!isCardAdded)
                            throw new Exception("Any problem with AddCardToMember.");

                    }
                    var registraionDate = _clubRepo.GetRegisrationDateByMember(currentUser.Id);
                    //Send Join SMS IF true
                    if (!await SlinkProcess(true, registraionDate))
                        throw new Exception("Any problem with Slink Service.");

                    userFromDb = await _clubRepo.GetUserByUserID(currentUser.Id);


                    var retUser = (UserDTO)_mapper.Map(userFromDb, typeof(UserDTO));
                    retUser.RegionId = _addressesBL.GetRegionByCityId(userDto.CityId).Data.RegionId;

                    return new BaseResponse<ResponseUserDTO>
                    {
                        Status = true,
                        Data = new ResponseUserDTO(retUser),
                        Message = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10256).MessageText
                    };

                }
            }
            else
                throw new Exception("Cannot find the user according to the given details. (UpdateMember)");

            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);

        }

        private async Task<BaseResponse<ResponseUserDTO>> UpdateMember_Update(UpdateMamberUserDTO updateMamberUserDTO, UserDTO userDto)
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();

            var userFromDb = await _clubRepo.GetUserByUserID(currentUser.Id);


            if (userFromDb != null)
            {

                userDto.Token = userFromDb.UserToken;
                userDto.Password = userFromDb.EncryptedUserPassword;
                userDto.LastUpdateMember = DateTime.Now;
                userDto.AllowSmsAndMail = updateMamberUserDTO.AllowSmsAndMail;
                if (string.IsNullOrEmpty(userDto.StreetName.Trim()) || string.IsNullOrEmpty(userDto.CityName.Trim()) || userDto.CityId == null || userDto.CityId == -1 || string.IsNullOrEmpty(userDto.FirstName.Trim()) || string.IsNullOrEmpty(userDto.LastName.Trim()))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10024).MessageText);
                if (!new EmailAddressAttribute().IsValid(updateMamberUserDTO.Email))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10243).MessageText);
                if(updateMamberUserDTO.MobilePhone.Trim() != userFromDb.MobilePhone.Trim())
                    throw new UnauthorizedAccessException("לא ניתן לערוך מספר טלפון ראשי");
                if (!utils.ValidateUpdateContactApproval(updateMamberUserDTO.IsUpdateDetailsApproved, (UserDTO)_mapper.Map(userFromDb, typeof(UserDTO)), userDto))
                    throw new BusinessException("לא ניתן לעדכן פרטים מבלי לאשר את התקנון");

                bool isPartnerPhoneChanged = userFromDb.PartnerPhone != updateMamberUserDTO.PartnerPhone;
                bool isEmailPartnerChanged = userFromDb.PartnerEmail != updateMamberUserDTO.PartnerEmail;

                if (isPartnerPhoneChanged || isEmailPartnerChanged)
                {
                    var currentUserDTO = (UserDTO)_mapper.Map(userFromDb, typeof(UserDTO));
                    SendSmsAfterContactUpdate(currentUserDTO, userFromDb.MobilePhone);
                }

                var value = await _clubRepo.UpdateMember(userDto);
                if (value)
                {
                    ContextManager.SetCurrentUserCache(currentUser.Id);
                    _clubRepo.CreateUpdateUserRequestLog((UserDTO)_mapper.Map(userFromDb, typeof(UserDTO)), userDto, updateMamberUserDTO.IsUpdateDetailsApproved);
                    if (userFromDb.AllowSmsAndMail != updateMamberUserDTO.AllowSmsAndMail)
                        if (updateMamberUserDTO.AllowSmsAndMail ?? false)
                        {
                            try
                            {
                                if (!await AddClientToList(userDto))
                                    throw new Exception("Problem with Pulseem service OR any problem with the given details.");
                                var result = await _dtsOnlineRepo.UpdateSubscriptionToDB(UpdateSubscription(updateMamberUserDTO, true));
                                if (!result) throw new Exception("Problem with UpdateSubscriptionToDB - Enable");
                            }
                            catch (Exception ex)
                            {
                                LoggerHelper.Error(ex, "Error On UpdateMember_Update");

                                //add message to response 
                            }

                        }
                        else
                        {
                            try
                            {
                                if (!await PulseemDisableClient(userDto))
                                    throw new Exception("Problem with Pulseem service OR any problem with the given details.");
                                var result = await _dtsOnlineRepo.UpdateSubscriptionToDB(UpdateSubscription(updateMamberUserDTO, false));
                                if (!result) throw new Exception("Problem with UpdateSubscriptionToDB - Disable");
                            }
                            catch (Exception ex)
                            {
                                LoggerHelper.Error(ex, "Error On UpdateMember_Update");
                                //add message to response 
                            }
                        }
                    userFromDb = await _clubRepo.GetUserByUserID(userDto.Id);

                    var retUser = _mapper.Map(userFromDb, typeof(UserDTO));
                    retUser.RegionId = _addressesBL.GetRegionByCityId(userDto.CityId).Data.RegionId;
                    retUser.CardNumber = currentUser.CardNumber;
                    ContextManager.SetCurrentUserCache(currentUser.Id);
                    return new BaseResponse<ResponseUserDTO>
                    {
                        Status = true,
                        Data = new ResponseUserDTO(retUser),
                        Message = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 1021).MessageText
                    };
                }
            }
            // throw new Exception("Cannot find the user according to the given details. (UpdateMember)");

            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
        }
        private void SendSmsAfterContactUpdate(UserDTO currentUser, string mobilePhone)
        {
            try
            {
                int orgId = ContextManager.CurrentOrganization().OrgId;

                string smsMessage = $"היי {currentUser.FirstName},\n\n" + "לידיעתך, עודכנו פרטי ההתקשרות בחשבונך. המשמעות היא שישלח אליהם SMS בכל התחברות לאתר ובכל טעינה. אם לא בוצע על ידך, יש לפנות לשירות הלקוחות";

                if (!string.IsNullOrEmpty(mobilePhone))
                {
                    var userSms = new SmsQueue
                    {
                        MemberId = currentUser.Id,
                        OrganizationId = orgId,
                        SenderName = "Behatsdaa",
                        Subscribers = mobilePhone,
                        Message = smsMessage,
                        MessageLengh = smsMessage.Length,
                        DeliveryDelayInMinutes = 0,
                        ExpirationDelayInMinutes = 120,
                        SmsType = (byte)orgId,
                        SeveralAttempts = 0,
                        Priority = 0,
                        SmsSend = false
                    };

                    _dtsOnlineRepo.AddContactToSMSQueue(userSms);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex, "Error while sending SMS after contact update");
            }
        }

        private UpdateSubscriptionDTO UpdateSubscription(UpdateMamberUserDTO updateMamberUserDTO, bool subscribed)
        {
            return new UpdateSubscriptionDTO
            {
                Id = ContextManager.CurrentUser().Id,
                Email = updateMamberUserDTO.Email,
                OrganizationId = ContextManager.CurrentOrganization().OrgId,
                Subscribed = subscribed
            };
        }

        private async void PulseemSetClientsStatus(string successPulseemStatus, string pulseemBaseurl, Dictionary<string, string> pulseemHeaders, UserDTO userDto, ENewStatusSetClients eNewStatusSetClients)
        {
            SetClientStatusRequest setClientStatusRequest = new SetClientStatusRequest()
            {
                cellphoneAndEmailList = new List<string>()
                    {
                        userDto.MobilePhone,
                        userDto.Email,
                    },
                emailOrCellphone = (int)EEmailOrCellphoneSetClients.Any,
                newStatus = (int)eNewStatusSetClients,
            };
            if (!string.IsNullOrEmpty(userDto.PartnerEmail))
                setClientStatusRequest.cellphoneAndEmailList.Add(userDto.PartnerEmail);

            ApiRequestModel apiRequestModel = new ApiRequestModel()
            {
                baseUrl = pulseemBaseurl,
                relativeUrl = PulseemKeys.SetClientStatus,
                method = EHttpRequestType.POST,
                data = setClientStatusRequest,
                headers = pulseemHeaders,

            };
            SetClientStatusResponse setClientStatusResponse = _restApiGW.ApiRequest<SetClientStatusResponse>(apiRequestModel);
            if (setClientStatusResponse.status != successPulseemStatus)
                LoggerHelper.Error($"Error in SetClientStatus Pulseem for {userDto.Id}, error from pulseem: {setClientStatusResponse.message}");
        }

        private void CheckPulseemConnection(string pulseemBaseurl, Dictionary<string, string> pulseemHeaders)
        {
            ApiRequestModel apiRequestModel = new ApiRequestModel()
            {
                baseUrl = pulseemBaseurl,
                relativeUrl = PulseemKeys.CheckConnection,
                method = EHttpRequestType.GET,
                headers = pulseemHeaders,
            };
            CheckConnectionResponse checkConnectionResponse = _restApiGW.ApiRequest<CheckConnectionResponse>(apiRequestModel);
            if (checkConnectionResponse.accountName != _configuration.GetConfigByValue<string>(ConfigurationKey.PulseemOrgName))
            {
                LoggerHelper.Error("Error in connection to pulseem");
                throw new Exception("\"Error in connection to pulseem");
            }

        }

        private async Task<bool> AddClientToList(UserDTO userDto)
        {
            try
            {
                string successPulseemStatus = "Success";
                string pulseemBaseurl = HttpUrls.Pulseem;
                Dictionary<string, string> pulseemHeaders = new Dictionary<string, string>()
                {
                    { HeadersKeys.PulseemApiKey, _configuration.GetConfigByValue<string>(ConfigurationKey.PulssemApiKey) }
                };
                this.CheckPulseemConnection(pulseemBaseurl, pulseemHeaders);
                string birthDate = (userDto.BirthDate ?? DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", null)).ToString("dd/MM/yyyy");
                var groupId = userDto.PremiumType == 3 ? PulseemGroupIDs.Miluim : PulseemGroupIDs.Mamshihim;
                AddClientsRequest addClientsRequest = new AddClientsRequest()
                {
                    groupIds = new List<int>() { groupId },
                    clientsData = new List<ClientsData>()
                    {
                        new ClientsData()
                        {
                            email = userDto.Email,
                            firstName = userDto.FirstName,
                            lastName = userDto.LastName,
                            birthDate = birthDate,
                            cellphone = userDto.MobilePhone,
                            needOptin = false,
                            overwrite = true
                        }
                    }
                };

                if (!string.IsNullOrEmpty(userDto.PartnerEmail))
                    addClientsRequest.clientsData.Add(
                        new ClientsData()
                        {
                            email = userDto.PartnerEmail,
                            firstName = $"בן זוג של - {userDto.FirstName} {userDto.LastName}",
                            birthDate = birthDate,
                            needOptin = false,
                            overwrite = true
                        }
                    );
                ApiRequestModel apiRequestModel = new ApiRequestModel()
                {
                    baseUrl = pulseemBaseurl,
                    relativeUrl = PulseemKeys.AddClients,
                    method = EHttpRequestType.POST,
                    data = addClientsRequest,
                    headers = pulseemHeaders,

                };
                AddClientsResponse addClientsResponse = _restApiGW.ApiRequest<AddClientsResponse>(apiRequestModel);
                bool isSuccessed = addClientsResponse.status == successPulseemStatus && string.IsNullOrEmpty(addClientsResponse.error);
                if (!isSuccessed)
                {
                    LoggerHelper.Error($"Pulseem AddClients Fail! pulseems error: {addClientsResponse.error}");
                }
                else
                {
                    this.PulseemSetClientsStatus(successPulseemStatus, pulseemBaseurl, pulseemHeaders, userDto, ENewStatusSetClients.Active);
                    Pulseem pulseemLog = new()
                    {
                        DateAdded = DateTime.Now,
                        MemberId = userDto.Id,
                        Email = userDto.Email,
                        MemberFirstName = userDto.FirstName,
                        MemberLastName = userDto.LastName,
                        PartnerEmail = userDto.PartnerEmail,
                        Source = "web",
                        Telephone = userDto.MobilePhone,
                        SentToPulseem = true,
                        OrganizationId = ContextManager.CurrentOrganization().OrgId
                    };
                    _dtsOnlineRepo.AddPulseemLog(pulseemLog);
                }
                return isSuccessed;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in AddClientToList, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return false;
            }
        }
        private void RemoveClientFromList(UserDTO userDto)
        {
            try
            {
                string successPulseemStatus = "Success";
                string pulseemBaseurl = HttpUrls.Pulseem;
                Dictionary<string, string> pulseemHeaders = new()
                {
                    { HeadersKeys.PulseemApiKey, _configuration.GetConfigByValue<string>(ConfigurationKey.PulssemApiKey) }
                };

                RemoveClientFromGroupRequest removeClientFromGroupRequest = new()
                {
                    GroupIDs = new List<int> { userDto.PremiumType == (int)EIDFPremiumeType.Approved ? PulseemGroupIDs.Miluim : PulseemGroupIDs.Mamshihim },
                    Cellphone = new List<string> { userDto.MobilePhone },
                    Email = new List<string> { userDto.Email }
                };

                ApiRequestModel apiRequestModel = new()
                {
                    baseUrl = pulseemBaseurl,
                    relativeUrl = PulseemKeys.RemoveClientFromGroup,
                    method = EHttpRequestType.POST,
                    data = removeClientFromGroupRequest,
                    headers = pulseemHeaders,
                };
                AddClientsResponse addClientsResponse = _restApiGW.ApiRequest<AddClientsResponse>(apiRequestModel);
                bool isSuccessed = addClientsResponse.status == successPulseemStatus && string.IsNullOrEmpty(addClientsResponse.error);
                if (!isSuccessed)
                    LoggerHelper.Error($"Pulseem AddClients Fail! pulseems error: {addClientsResponse.error}");
                Pulseem pulseemLog = new()
                {
                    DateAdded = DateTime.Now,
                    MemberId = userDto.Id,
                    Email = userDto.Email,
                    MemberFirstName = userDto.FirstName,
                    MemberLastName = userDto.LastName,
                    PartnerEmail = userDto.PartnerEmail,
                    Source = "web",
                    Telephone = userDto.MobilePhone,
                    SentToPulseem = false,
                    OrganizationId = ContextManager.CurrentOrganization().OrgId
                };
                _dtsOnlineRepo.AddPulseemLog(pulseemLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in AddClientToList, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        private async Task<bool> PulseemDisableClient(UserDTO userDto)
        {
            try
            {
                string successPulseemStatus = "Success";
                string pulseemBaseurl = _configuration.GetConfigByValue<string>(ConfigurationKey.PulseemApiUrl);
                Dictionary<string, string> pulseemHeaders = new Dictionary<string, string>()
                {
                    { HeadersKeys.PulseemApiKey, _configuration.GetConfigByValue<string>(ConfigurationKey.PulssemApiKey) }
                };
                this.CheckPulseemConnection(pulseemBaseurl, pulseemHeaders);
                this.PulseemSetClientsStatus(successPulseemStatus, pulseemBaseurl, pulseemHeaders, userDto, ENewStatusSetClients.Removed);
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in AddClientToList, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<BaseResponse<ResponseUserDTO>> UpdatePassword(UserPasswordInfoDTO userPasswordInfoDto)
        {
            if (userPasswordInfoDto.NewPassword?.Trim() == userPasswordInfoDto.MemberId?.Trim())
                throw new BusinessException("לא ניתן לעדכן את הסיסמא בצורה הנוכחית");

            var passwordValid = ValidatePassword(userPasswordInfoDto.NewPassword);
            if (passwordValid.isValid == false)
            {
                throw new BusinessException(passwordValid.ErrorMessage);
            }

            switch (userPasswordInfoDto.NewOrUpdate)
            {
                case ENewOrUpdate.Reset:
                    return await UpdatePassword_Reset(userPasswordInfoDto);
                default:
                    throw new Exception("No valid NewOrUpdate.");
            }

        }



        private async Task<BaseResponse<ResponseUserDTO>> UpdatePassword_Reset(UserPasswordInfoDTO userPasswordInfoDto)
        {
            if (userPasswordInfoDto != null)
            {
                var userFromDb = await _clubRepo.GetUserByUserID(userPasswordInfoDto.MemberId);
                // var validation = ValidatePasswordForRecover(userPasswordInfoDto.ForgetPasswordToken, userPasswordInfoDto.MemberId);

                if (userFromDb != null && !string.IsNullOrEmpty(userPasswordInfoDto.ForgetPasswordToken)
                    && (!string.IsNullOrEmpty(userFromDb.ForgetPasswordToken) && userFromDb.ForgetPasswordToken.Trim().Equals(userPasswordInfoDto.ForgetPasswordToken))
                    && userFromDb.MemberId.Equals(userPasswordInfoDto.MemberId)
                    && !string.IsNullOrEmpty(userPasswordInfoDto.NewPassword))
                {
                    userPasswordInfoDto.NewPassword = Cryptor.MD5Encrypt(userPasswordInfoDto.NewPassword);
                    if (userPasswordInfoDto.NewPassword.Equals(userFromDb.EncryptedUserPassword)
                        || userPasswordInfoDto.NewPassword.Equals(userFromDb.LastEncryptedUserPassword))
                        throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10044).MessageText);
                    var value = await _clubRepo.UpdatePassword(userPasswordInfoDto);
                    if (value)
                    {
                        ContextManager.SetCurrentUserCache(userPasswordInfoDto.MemberId);
                        return new BaseResponse<ResponseUserDTO>
                        {
                            Status = true,
                            Data = new ResponseUserDTO(_mapper.Map(userFromDb, typeof(UserDTO)))
                        };
                    }
                }
                if (userFromDb == null)
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10241).MessageText);
                else
                    throw new BusinessException("תם הזמן לשינוי סיסמא. יש לחזור על התהליך מחדש.");
            }
            throw new Exception("Given user is null OR any other problem with reset password. (UpdatePassword_Reset)");
        }

        /// <summary> 
        ///  The method creates a random 16 digits token and returns a link  
        ///  to the update password page with the token 
        /// </summary> 
        /// <param name = "memberId" >
        ///  The id of the member who wishes to recover his password 
        /// </param> 
        /// <returns> 
        ///  Returns a link to the update password page 
        /// </returns> 
        public async Task<BaseResponse<string>> RecoverPassword(string memberId)
        {

            var currentUserIp = ContainerManager.Container.Resolve<IHttpContextAccessor>().HttpContext.Request.Headers.GetUserIP();
            var allowedIPs = allowedFullLoginIPs;


            if (!allowedIPs.Contains(currentUserIp))
            {
                 throw new BusinessException("אינך מורשה לבצע פעולה זו");
                
            }




            //var a = CreateUsersScript(true);
            var userFromDB = _clubRepo.GetUserByUserID(memberId).Result;
            if (userFromDB != null)
            {
                if (string.IsNullOrEmpty(userFromDB.EncryptedUserPassword))
                {
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10261).MessageText);
                }

                var userDto = (UserDTO)_mapper.Map(userFromDB, typeof(UserDTO));

                if (isMemberDetailsMissing(userDto.Email, userDto.PhoneNumber, userDto.MobilePhone, userDto.Password))
                {
                    UserPasswordInfoDTO userUpdatePasswordDTO = new UserPasswordInfoDTO
                    {
                        NewPassword = Cryptor.MD5Encrypt(memberId),
                        MemberId = memberId
                    };
                    await _clubRepo.UpdatePassword(userUpdatePasswordDTO);

                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10311).MessageText);
                }

                userDto.ForgetPasswordToken = RecoverPasswordTokenCreator();
                userDto.ForgetPasswordCreated = DateTime.Now;
                _clubRepo.UpdateForRecoverPasswordAsync(userDto);

                var link = _configuration.GetConfigByValue<string>(ConfigurationKey.RecoverPasswordURL) + userDto.ForgetPasswordToken + "&memberId=" + userDto.Id;

                var emailQueue = RecoverPasswordEmailQueueCreator(userDto, link);

                var smsQueue = RecoverPasswordSmsCreator(userDto, link);

                if (emailQueue != null)
                    _dtsOnlineRepo.AddContactToEmailQueue(emailQueue);
                if (smsQueue != null)
                    await _dtsOnlineRepo.AddContactToSMSQueue(smsQueue);
                if (emailQueue == null && smsQueue == null)
                    throw new BusinessException("המשתמש שהוזן לא סיים את תהליך ההרשמה. יש לפנות לשירות לקוחות.");
                var parameters = new object[] { string.Format("{1}****-{0}", userDto.MobilePhone.Substring(0, 3), userDto.MobilePhone.Substring(userDto.MobilePhone.Length - 3, 3)), EncryptEmail(userDto.Email) };

                return new BaseResponse<string>
                {
                    Status = true,
                    Data = string.Format(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10260).MessageText, parameters)
                };

            }
            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10241).MessageText);
        }

        private string RecoverPasswordTokenCreator()
        {
            var rnd = new Random();
            int rnd1 = rnd.Next(111111, 999999);
            int rnd2 = rnd.Next(111111, 999999);
            long token = ((long)rnd1 << 32) | (rnd2 & 0xFFFFFFFFL);
            return token.ToString().Trim();
        }

        private EmailQueue RecoverPasswordEmailQueueCreator(UserDTO userDto, string link)
        {
            if (userDto.Email != null)
                return new EmailQueue
                {
                    EmailTo = userDto.Email,
                    EmailDateAdded = DateTime.Now,
                    EmailFrom = ContextManager.CurrentOrganization().ServiceMail,
                    EmailBody = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10255).MessageText + ":\n" + link,
                    EmailSendDate = DateTime.Now,
                    EmailSubject = "בהצדעה - איפוס סיסמא",
                    EmailType = 100000 + ContextManager.CurrentOrganization().OrgId,
                    IsBodyHtml = true,
                    IsSendEmail = false,
                    SeveralAttempts = 0,
                    EmailQueueUsersId = 2 //TEMPORARY!
                };
            return null;

        }

        private SmsQueue RecoverPasswordSmsCreator(UserDTO userDto, string link)
        {
            var textsInHtml = SerializerHelper.HtmlToString(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10255).MessageText);
            var message = textsInHtml.First() + "  " + link;
            int orgId = _contextManager.CurrentOrganization().OrgId;
            if (userDto.MobilePhone != null)
                return new SmsQueue
                {
                    MemberId = userDto.Id,
                    DateAdded = DateTime.Now,
                    OrganizationId = orgId,
                    SenderName = "Behatsdaa",
                    Subscribers = userDto.MobilePhone,
                    Message = message,
                    MessageLengh = message.Length,
                    DeliveryDelayInMinutes = 0,
                    ExpirationDelayInMinutes = 120,
                    SmsType = (byte)orgId,
                    SeveralAttempts = 0,
                    Priority = 0,
                    SmsSend = false,
                };
            return null;
        }

        public BaseResponse<ResponseUserDTO> GetCurrentUser()
        {
            ResponseUserDTO currentUser = ContextManager.CurrentUser();
            currentUser.RegionId = _addressesBL.GetRegionByCityId(currentUser.CityId).Data.RegionId;
            if (currentUser != null)
            {
                var json = JsonConvert.SerializeObject(currentUser);
                var userClone = JsonConvert.DeserializeObject<ResponseUserDTO>(json);
                userClone.PinCode = string.Empty;
                userClone.ForgetPasswordToken = string.Empty;
                return new BaseResponse<ResponseUserDTO>
                {
                    Status = true,
                    Data = userClone
                };
            }
            throw new Exception("No current user has been saved in cache.");
        }

        public void LogOut()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookiesKeys.AccessToken);
        }

        public async Task<BaseResponse<bool>> UserSlinkSMS()
        {
            var cardNumber = ContextManager.CurrentUser().CardNumber;
            var cardDto = _clubRepo.GetCardActivationAndExpiryDate(cardNumber);
            if (cardNumber != null && cardDto != null && cardDto.CardStatus == (byte)ECardStatus.ACTIVE)
            {
                var slinkProcess = await SlinkProcess(false);

                return new BaseResponse<bool>
                {
                    Data = slinkProcess,
                    Status = slinkProcess
                };
            }
            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10037).MessageText);
        }

        private async Task<bool> SlinkProcess(bool newMember, DateTime? RegistrationDate = null)
        {

            if (newMember)
                return await _dtsOnlineRepo.SlinkProcess_Join(RegistrationDate);
            else
            {
                var generatedLinkCode = GenerateRandomString();
                return await _dtsOnlineRepo.SlinkProcess(generatedLinkCode);
            }

        }

        private string GenerateRandomString()
        {
            int maxSize = 10;
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();

        }


        public async Task<IDFValidationResponseDTO> MinistryOfDefenceValidation(string memberId)
        {

                return await HttpRequestManager.GetIDFValidationForMember(memberId);
        }


		private bool CheckLoginCompatibility(ELoginType loginType)
        {
            return Feature.HasPermission(loginType);
        }

        private string GenerateNewUserId(UserDTO userDto)
        {
            switch (userDto.LoginType)
            {
                case ELoginType.Google:
                case ELoginType.Facebook:
                case ELoginType.EmailAndPass:
                case ELoginType.UserNameAndPass:
                    return _clubRepo.GetNextSequenceValue();
                case ELoginType.MemberIdAndPassword:
                    return userDto.Id.PadLeft(9, '0');
                case ELoginType.IdentityAndPassword:
                    return userDto.IdentityNumber.PadLeft(9, '0');
                default:
                    throw new Exception(("No valid LoginType."));
            }
        }

        public void CreateUserToken(string memberId, bool isNotApprovedMemeber = false)
        {
            string newAccessToken = this.GenerateAccessToken(memberId, isNotApprovedMemeber);
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(_configuration.GetConfigByValue<int>(ConfigurationKey.JwtExpireMinutes)),
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookiesKeys.AccessToken, newAccessToken, cookieOptions);
            _httpContextAccessor.HttpContext.Request.Headers.SetCommaSeparatedValues(HeadersKeys.Token, newAccessToken);
        }

        /// <summary> 
        ///  The method returns the member true ststus 
        /// </summary> 
        /// <param name="MemberStatus">The first status</param> 
        /// <returns>Returns the member true ststus</returns> 
        private int GenerateIsMemberResult(int MemberStatus)
        {
            switch (MemberStatus)
            {
                case 0:
                    return 0;
                case 2:
                    return 1;
                case 4:
                    return 4;
                case 5:
                    return 5;
                case 7:
                    return 3;
                case 999:
                    return 999;
            }
            return 0;
        }

        private (bool isValid, string ErrorMessage) ValidatePassword(string password)
        {
            string ErrorMessage = string.Empty;
            bool isValid = false;
            if (string.IsNullOrEmpty(password)) return (false, "שגיאה כללית");

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,14}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(password))
            {
                ErrorMessage = "הסיסמה חייבת להכיל לפחות אות קטנה אחת ";
                isValid = false;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                ErrorMessage = "הסיסמה חייבת להכיל לפחות אות גדולה אחת ";
                isValid = false;
            }
            else if (!hasMiniMaxChars.IsMatch(password))
            {
                ErrorMessage = "אורך הסיסמה חייב להכיל 8 - 14 תווים";
                isValid = false;
            }
            else if (!hasNumber.IsMatch(password))
            {
                ErrorMessage = "הסיסמה חייבת להכיל לפחות ספרה אחת";
                isValid = false;
            }
            else if (!hasSymbols.IsMatch(password))
            {
                ErrorMessage = "הסיסמה חייבת להכיל לפחות תו מיוחד אחד";
                isValid = false;
            }
            else
            {
                isValid = true;
            }

            return (isValid, ErrorMessage);
        }

        private bool NeedToUpdateDetails(ref UserDTO userDto)
        {
            //TODO check overLoad
            userDto.UpdateOrRegister = LastUpdateMemberUtil.LastUpdateMemberGenerator(userDto.LastUpdateMember);

            return userDto.UpdateOrRegister != null;
        }


        /// <summary> 
        ///  The method validates the token from the RecoverPassword request and  
        ///  if the validation is successful, then it returns the user details 
        /// </summary> 
        /// <param name="tokenCode">The token to validate</param> 
        /// <returns>Returns the user details</returns> 
        public BaseResponse<ResponseUserDTO> ValidatePasswordForRecover(string tokenCode, string memberId)
        {
            var userFromDB = _clubRepo.GetUserByUserID(memberId).Result;
            if (userFromDB != null)
            {
                var userDto = (UserDTO)_mapper.Map(userFromDB, typeof(UserDTO));

                (string DBMember_token, DateTime? DBMember_created) = _clubRepo.ValidateRecoverPasswordToken(userDto, tokenCode);

                TimeSpan span = DateTime.Now.Subtract((DateTime)DBMember_created);
                var expiredTime = _configuration.GetConfigByValue<int>(ConfigurationKey.MinutesToExpired_RecoverPassword);
                bool validation = DBMember_token.Trim().Equals(tokenCode) && span.TotalMinutes <= expiredTime;

                if (validation)
                {
                    return new BaseResponse<ResponseUserDTO>
                    {
                        Status = true,
                        Data = new ResponseUserDTO(userDto)
                    };
                }
                // need to throw bussiness exceptions? are there messages for the errors?
                else
                {
                    throw new Exception(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10244).MessageText);

                }
            }
            throw new Exception("No User has been found in the DB according to the given details. (ValidatePasswordForRecover)");
        }

        public BaseResponse<ResponseUserDTO> GetUserByAccessToken(string accessToken)
        {
            string hexToken = Cryptor.ConvertHexToString(accessToken, System.Text.Encoding.Unicode);
            string res = Cryptor.Decrypt(hexToken);

            string memberID = res.Substring(0, 9);

            string today = res.Substring(9);

            DateTime parsed = DateTime.Today.AddDays(1);

            if (!DateTime.TryParse(today, out parsed))
                DateTime.TryParseExact(today, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed);

            if (!parsed.Equals(DateTime.Today))
                throw new BusinessException("מזהה משתמש לא בתוקף");

            var user = new ResponseUserDTO();
            user.Id = memberID;
            user.IdentityNumber = memberID;

            return new BaseResponse<ResponseUserDTO>
            {
                Status = true,
                Data = user,
            };

        }

        private int GetMonths(DateTime? fromDate, DateTime toDate)
        {
            if (fromDate >= toDate)
                return 0;

            var months = ((toDate.Year * 12) + toDate.Month) - ((fromDate.Value.Year * 12) + fromDate.Value.Month);
            if (toDate.Day >= fromDate.Value.Day)
                months++;

            return months;
        }

        private bool ShouldCheckAllowedLoginConfigPeriod(DateTime? date)
        {
            if (date != null)
                return (DateTime.Now - date.Value).Days >= lastLoginAllowedPeriod;
            return false;
        }

        public Task<BaseResponse<ResponseUserDTO>> CreateNewUser(UserDTO userDto)
        {
            throw new Exception("There is no use of Register method in Behatsdaa registration process.");
        }
        public async Task<int> ValidatePinCode(string memberId, string inputPinCode)
        {
            var pincodeMaxAttempts = _configuration.GetConfigByValue<int>("PincodeMaxAttempts");

            using (var context = ContextManager.ClubContext())
            {
                var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId == memberId);
                if (userFromDb == null)
                    throw new Exception("User not found");

                if (userFromDb.PinCode != null && userFromDb.PinCode == inputPinCode)
                {
                    return 0; 
                }

                userFromDb.PinCodeAttempts += 1;

                if (userFromDb.PinCodeAttempts >= pincodeMaxAttempts)
                {
                    userFromDb.PinCode = null;
                    userFromDb.PinCodeAttempts = 0;

                    ContextManager.ClearCurrentUserCache(userFromDb.MemberId);
                    await context.SaveChangesAsync();
                    return 2;
                }
                else
                {
                    await context.SaveChangesAsync();
                    return 1;
                }
            }
        }

        public async Task<bool> GetUserWithClubCreditCard(bool iscontrolled)
        {
            using (var context = ContextManager.ClubContext())
            {
                var clubCreditCardList = new List<Nofshonit.Common.EF.Club.AllMembers>();
                var users = await GetAllMembers(1000);
                List<string> ClubCreditCardFactorySymbolExclusionPay = (await _configBL.GetValueByKey("ClubCreditCardFactorySymbolExclusionPay"))[0].Value.Split(',').ToList();
                foreach (var user in users)
                {
                    var res = await MinistryOfDefenceValidation(user.MemberId);
                    // if (hist.MemberSpecialID.Equals("0000103166") || hist.MemberSpecialID.Equals("0000362509") || hist.MemberSpecialID.Equals("103166") || hist.MemberSpecialID.Equals("362509"))
                    if (ClubCreditCardFactorySymbolExclusionPay.Contains(res.Zehut))
                        clubCreditCardList.Add(user);
                }
                return true;
            }
        }


        //SCRIPTS 
        public async Task<bool> CreateUsersScript(bool iscontrolled)
        {
            // var identities = new List<string>();

            var identities = new List<string> { "316001197", "316002161", "316002484", "316018381", "316018449", "316002237", "316006782", "316078526", "205350465", "014253694", "205356488", "304000078", "205350465", "014253694", "205356488", "304000078" };

            //var identities = new List<string> { "205350465", "014253694", "205356488", "205361199", "304000078" }; // users for Yaara

            if (iscontrolled)
            {
                foreach (var identity in identities.Distinct())
                {
                    var result = await Authenticate(new AuthenticateUserRequestDTO { AuthenticateOrJoin = EAuthenticateOrJoin.Join, IdentityNumber = identity, Password = identity, LoginType = ELoginType.IdentityAndPassword }, false);
                    if (result.Status)
                    {
                        var userFromDb = _clubRepo.GetUserByUserIDSync(identity);
                        UserDTO userDto = _mapper.Map(userFromDb, typeof(UserDTO));
                        CreateUserToken(userDto.Id);
                        result = await UpdatePassword(new UserPasswordInfoDTO { NewOrUpdate = ENewOrUpdate.New, NewPassword = "123456789" });

                        if (result.Status)
                        {
                            result = await UpdateMember(new UpdateMamberUserDTO
                            {

                                FirstName = "ישראל",
                                LastName = "ישראלי",
                                Email = "nisima@dts-4u.com",
                                MobilePhone = "0528083209",
                                CityName = "בית שמש",
                                CityId = 7,
                                StreetName = "הלוי",
                                HouseNumber = "12",
                                EmailSubscribe = true,
                                Gender = EGender.Male,
                                Zip = "55900",
                                NewOrUpdate = ENewOrUpdate.New,
                                FactorySymbol = "הסתדרות",
                                ApartmentNumber = "13",
                                NumOfChildren = 4
                            });

                            if (result.Status)
                            {
                                result = await Authenticate(new AuthenticateUserRequestDTO { AuthenticateOrJoin = EAuthenticateOrJoin.Authenticate, IdentityNumber = identity, Password = "123456789", LoginType = ELoginType.IdentityAndPassword }, false);
                                if (result.Status)
                                    continue;
                            }

                        }
                    }
                    return false;
                }
                return true;
            }
            throw new Exception("the script is not controlled.");
        }

        public async Task<BaseResponse<ContactMemberDetails>> LoginBySms(string memberId, string hash)
        {
            ApiLoggerBL.LogConnectorData(memberId);
            var userFromDb = _clubRepo.GetUserByUserIDSync(memberId);
            var maxAmountOfSmsIn2Minutes = _configuration.GetConfigByValue<int>(ConfigurationKey.MaxAmountOfSmsIn2Minutes);
            var smsLoginCodeLength = _configuration.GetConfigByValue<int>(ConfigurationKey.SmsLoginCodeLength);
            var smsLoginCodeMaxAgeInMinutes = _configuration.GetConfigByValue<int>(ConfigurationKey.SmsLoginCodeMaxAgeInMinutes);

            if (userFromDb == null)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);

            if (userFromDb != null && userFromDb.Active == 0)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 223).MessageText);

            if (!_clubRepo.CheckIfAbleToTryLogin(userFromDb))
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10041).MessageText, 10041);

            if (isMemberDetailsMissing(userFromDb.Email, userFromDb.PhoneNumber, userFromDb.MobilePhone, userFromDb.EncryptedUserPassword))
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10279).MessageText);
            var phone = string.IsNullOrWhiteSpace(userFromDb.MobilePhone) ? userFromDb.PhoneNumber : userFromDb.MobilePhone;

            //1.בדיקה מול משבה"ט
             //IDFValidationResponseDTO isValidMember = await CalculatePremiumTypeForMember(userFromDb, addRowToAllowanceHistory:false);

                //if (isValidMember.premiumeType == EIDFPremiumeType.NotApproved)
                    //throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 51136).MessageText);

            //Random generator = new Random();
            //var code = generator.Next(0, 999999999).ToString("D6").Substring(0, smsLoginCodeLength);

            string code = utils.CreateOtp(smsLoginCodeLength);

            var codes = _clubRepo.GetSmsCodes(memberId);

            DtsLoggger.Logger.Info($"[Create OTP Code] [MemberId: {memberId ?? "NULL"}] ,[code: {code}] , [SessionId: {_httpContextAccessor?.HttpContext?.Session?.Id}]");


            if (codes.Any() && codes.Where(x => x.InsertDate > DateTime.Now.AddMinutes(-2)).Count() > maxAmountOfSmsIn2Minutes)
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10275).MessageText);

            _clubRepo.DeleteSmsCodesByMemberId(memberId);
            _clubRepo.SaveSmsCodeForLogin(memberId, code);
            int orgId = _contextManager.CurrentOrganization().OrgId;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("הקוד לכניסה חד פעמית לאתר בהצדעה: " + code);
            sb.AppendLine($"<#> Your verification code is {code}");
            sb.AppendLine();
            sb.Append($"@{ContextManager.ClubContext().AppConfig.FirstOrDefault(f => f.Key == ConfigurationKey.Domain).Value} #{code}");
            sb.AppendLine();
            sb.AppendLine(hash);
            var message = sb.ToString();

            string connectionString = _configuration.GetConnectionStringByValue<string>(ConfigurationKey.Dts_Logs_DB);
            string smsServiceUrl = _configuration.GetConfigByValue<string>(ConfigurationKey.SendSMSIIS_Url);

            if (!string.IsNullOrEmpty(phone))
            {
                utils.SendSMS(connectionString, smsServiceUrl, phone, orgId, memberId, message);
                //SmsClient client = new SmsClient(connectionString, smsServiceUrl);
                //var res = client.SendOtpSMS(orgId, memberId, "Behatsdaa", phone, message, 0, 1);
            }

            if (!string.IsNullOrEmpty(userFromDb.PartnerPhone))
            {
                utils.SendSMS(connectionString, smsServiceUrl, userFromDb.PartnerPhone, orgId, memberId, message);
            }


            if (!string.IsNullOrEmpty(userFromDb.Email))
            {

                var emailQ = new EmailQueue
                {
                    EmailTo = userFromDb.Email,
                    EmailDateAdded = DateTime.Now,
                    EmailFrom = ContextManager.CurrentOrganization().ServiceMail,
                    EmailBody = message,
                    EmailSendDate = DateTime.Now,
                    EmailSubject = "כניסה חד פעמית לאתר בהצדעה",
                    EmailType = 100000 + ContextManager.CurrentOrganization().OrgId,
                    IsBodyHtml = true,
                    IsSendEmail = false,
                    SeveralAttempts = 0,
                    EmailQueueUsersId = 2 //TEMPORARY!
                };
                var repo = _dtsOnlineRepo;
                repo.AddContactToEmailQueueSync(emailQ);
            }


            if (!string.IsNullOrEmpty(userFromDb.PartnerEmail))
            {
                var partnerEmailQ = new EmailQueue
                {
                    EmailTo = userFromDb.PartnerEmail,
                    EmailDateAdded = DateTime.Now,
                    EmailFrom = ContextManager.CurrentOrganization().ServiceMail,
                    EmailBody = message,
                    EmailSendDate = DateTime.Now,
                    EmailSubject = "כניסה חד פעמית לאתר בהצדעה",
                    EmailType = 100000 + ContextManager.CurrentOrganization().OrgId,
                    IsBodyHtml = true,
                    IsSendEmail = false,
                    SeveralAttempts = 0,
                    EmailQueueUsersId = 2
                };
                _dtsOnlineRepo.AddContactToEmailQueueSync(partnerEmailQ);
            }


            string maskedPhone = string.Empty, maskedEmail = string.Empty;
            if (!string.IsNullOrEmpty(phone))
                maskedPhone = string.Format("{1}****-{0}", phone.Substring(0, 3), phone.Substring(phone.Length - 3, 3));
            if (!string.IsNullOrEmpty(userFromDb.Email))
                maskedEmail = EncryptEmail(userFromDb.Email);



            string maskedPartnerPhone = string.Empty, maskedPartnerEmail = string.Empty;

            if (!string.IsNullOrEmpty(userFromDb.PartnerPhone))
                maskedPartnerPhone = string.Format("{1}****-{0}", userFromDb.PartnerPhone.Substring(0, 3), userFromDb.PartnerPhone.Substring(userFromDb.PartnerPhone.Length - 3, 3));

            if (!string.IsNullOrEmpty(userFromDb.PartnerEmail))
                maskedPartnerEmail = EncryptEmail(userFromDb.PartnerEmail);


            return new BaseResponse<ContactMemberDetails>
            {
                Status = true,
                Data = new ContactMemberDetails(
                    maskedPhone,
                    maskedEmail,
                    maskedPartnerPhone,
                    maskedPartnerEmail,
                    userFromDb.PrivacyPolicyAcceptedDate
                )
            };
        }


        public async Task<BaseResponse<ResponseUserDTO>> JoinBySmsAsync(string IdentityNumber)
        {
            //1 - Go to RegistrationAudits table and select all data by IdentityNumber;
            RegistrationAudits regAudits = await _clubRepo.GetRegistrationAuditByIdentity(IdentityNumber);

            if (regAudits != null)
            {
                //bool result = await _clubRepo.UpdateJoinByOtpAttempts(false, regAudits);
                bool result = await _clubRepo.CanSendOtp(regAudits);
                if (!result)
                {
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10290).MessageText);
                }

                var maxAmountOfSmsIn2Minutes = _configuration.GetConfigByValue<int>(ConfigurationKey.MaxAmountOfSmsIn2Minutes);
                var smsLoginCodeLength = _configuration.GetConfigByValue<int>(ConfigurationKey.SmsLoginCodeLength);
                var smsLoginCodeMaxAgeInMinutes = _configuration.GetConfigByValue<int>(ConfigurationKey.SmsLoginCodeMaxAgeInMinutes);


                //2 - if I have cell number. send otp
                string code = utils.CreateOtp(smsLoginCodeLength);

                var codes = _clubRepo.GetSmsCodes(IdentityNumber);

                if (codes.Any() && codes.Where(x => x.InsertDate > DateTime.Now.AddMinutes(-2)).Count() > maxAmountOfSmsIn2Minutes)
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10275).MessageText);

                _clubRepo.DeleteSmsCodesByMemberId(IdentityNumber);
                _clubRepo.SaveSmsCodeForLogin(IdentityNumber, code);
                int orgId = _contextManager.CurrentOrganization().OrgId;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("הקוד לכניסה חד פעמית לאתר בהצדעה: " + code);
                sb.Append($"@{ContextManager.ClubContext().AppConfig.FirstOrDefault(f => f.Key == ConfigurationKey.Domain).Value} #{code}");
                var message = sb.ToString();
                if (!string.IsNullOrEmpty(regAudits.Cellphone))
                {
                    string connectionString = _configuration.GetConnectionStringByValue<string>(ConfigurationKey.Dts_Logs_DB);
                    string smsServiceUrl = _configuration.GetConfigByValue<string>(ConfigurationKey.SendSMSIIS_Url);

                    utils.SendSMS(connectionString, smsServiceUrl, regAudits.Cellphone, orgId, IdentityNumber, message);
                }
            }

            return new BaseResponse<ResponseUserDTO>
            {
                Status = true,
                Data = new ResponseUserDTO
                {
                    IdentityNumber = regAudits.IdentityNumber
                }
            };
        }

        public async Task<BaseResponse<ResponseUserDTO>> ValidateJoinBySmsAsync(string IdentityNumber, string code)
        {
            //1 - Go to RegistrationAudits table and select all data by IdentityNumber;
            RegistrationAudits regAudits = await _clubRepo.GetRegistrationAuditByIdentity(IdentityNumber);
            var smsLoginCodeMaxAgeInMinutes = _configuration.GetConfigByValue<int>(ConfigurationKey.SmsLoginCodeMaxAgeInMinutes);
            var maxAmountOfSmsIn2Minutes = _configuration.GetConfigByValue<int>(ConfigurationKey.MaxAmountOfSmsIn2Minutes);


            if (regAudits != null)
            {
                //update join attempts
                bool result = await _clubRepo.UpdateJoinByOtpAttempts(false, regAudits);
                if (!result)
                {
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10290).MessageText);
                }
                //get sms code saved in session
                var codes = _clubRepo.GetSmsCodes(IdentityNumber);

                //check otp exist
                if (!codes.Any(c => c.Code == code))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10277).MessageText);

                //check otp expiration 
                if (codes.Any(c => c.InsertDate < DateTime.Now.AddMinutes(smsLoginCodeMaxAgeInMinutes * (-1))))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10277).MessageText);


                //delete codes from session
                _clubRepo.DeleteSmsCodesByMemberId(IdentityNumber);

                //registered  is not allowed
                if (regAudits.IsMember == 0)
                {
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 51136).MessageText);
                }

                //check if user exist in all members
                var userFromDb = _clubRepo.GetUserByUserIDSync(IdentityNumber);

                //if exists throw an exception
                if (userFromDb != null && !isMemberDetailsMissing(userFromDb.Email, userFromDb.PhoneNumber, userFromDb.MobilePhone, userFromDb.EncryptedUserPassword))
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10240).MessageText);

                var userDto = new UserDTO();
                //check if user has max credit card
                userDto.ClubCreditCard = regAudits.ClubCreditCard;
                userDto.PremiumType = regAudits.PremiumType;
                //if (!await _clubRepo.CreateNewUser(newUser))
                //{
                //    LoggerHelper.Error($"ValidateJoinBySmsAsync CreateNewUser - for memberid: {IdentityNumber}, failed");
                //    throw new Exception($"ValidateJoinBySmsAsync CreateNewUser - for memberid: {IdentityNumber}, failed");
                //}


                userDto.Id = IdentityNumber.PadLeft(9, '0');
                userDto.Password = userDto.Id;
                userDto.IdentityNumber = userDto.Id;
                userDto.LoginType = ELoginType.MemberIdAndPassword;
                userDto.RegistrationDate = DateTime.Now;
                userDto.IdentityGuid = Guid.NewGuid().ToString();
                userDto.Darga = regAudits.Darga;
                Common.EF.Club.AllMembers member = new Common.EF.Club.AllMembers() { MemberId = userDto.IdentityNumber };
                userDto.MobilePhone = regAudits.Cellphone;
                try
                {
                    return await CreateBasicNewUser(userDto);
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error($"ValidateJoinBySmsAsync CreateNewUser - for memberid: {IdentityNumber}, failed");
                    throw new Exception($"ValidateJoinBySmsAsync CreateNewUser - for memberid: {IdentityNumber}, failed");
                }
            }
            else
            {
                LoggerHelper.Error($"ValidateJoinBySmsAsync Error - for memberid: {IdentityNumber}, regAudits is null");
                throw new Exception($"ValidateJoinBySmsAsync Error - for memberid: {IdentityNumber}, regAudits is null");
            }




        }

        public BaseResponse<MaskDataDTO> GetMemberHashedPhoneNumberAndEmail(string memberID)
        {
            MaskDataDTO response = new MaskDataDTO();
            var member = _clubRepo.GetUserByUserIDSync(memberID);
            var phone = string.IsNullOrWhiteSpace(member.MobilePhone) ? member.PhoneNumber : member.MobilePhone;
            if (!string.IsNullOrEmpty(phone))
                response.phone = MaskPfonel(member.MobilePhone);
            if (!string.IsNullOrEmpty(member.Email))
                response.email = MaskEmail(member.Email);
            return new BaseResponse<MaskDataDTO>
            {
                Status = true,
                Data = response
            };

        }



        private string MaskEmail(string input)
        {
            string pattern = @"(?<=[\w]{3})[\w-\._\+%]*(?=[\w]{1}@)";
            string result = Regex.Replace(input, pattern, m => new string('*', m.Length));
            return result;
        }

        private string MaskPfonel(string phoneNumber)
        {
            var prefix = phoneNumber.Substring(0, 3);
            var suffix = phoneNumber.Substring(7, 3);
            return prefix + "-****" + suffix;
        }
        public async Task<BaseResponse<ResponseUserDTO>> ValidateLoginBySms(string memberId, string code)
        {
            var userFromDb = _clubRepo.GetUserByUserIDSync(memberId);
            var smsLoginCodeMaxAgeInMinutes = _configuration.GetConfigByValue<int>(ConfigurationKey.SmsLoginCodeMaxAgeInMinutes);

            UserDTO userDto = _mapper.Map(userFromDb, typeof(UserDTO));
            userDto.LoginType = ELoginType.MemberIdAndPassword;

            if (!_clubRepo.CheckIfAbleToTryLogin(userFromDb))
                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10041).MessageText, 10041);

            var codes = _clubRepo.GetSmsCodes(memberId);
            

            DtsLoggger.Logger.Info($"[MemberId: {memberId ?? "NULL"}] Input code: {code ?? "NULL"}, smsLoginCodeMaxAgeInMinutes: {smsLoginCodeMaxAgeInMinutes}");

            if (codes != null)
            {
                DtsLoggger.Logger.Info($"[Verify OTP Code] [MemberId: {memberId ?? "NULL"}] ,[codes: {(codes != null ? string.Join(",", codes.Select(c => c?.Code ?? "NULL")) : "NULL")}] , [SessionId: {_httpContextAccessor?.HttpContext?.Session?.Id ?? "NULL"}]");

                foreach (var c in codes)
                {
                    var cCode = c?.Code ?? "NULL";
                    var cInsertDate = c?.InsertDate.ToString() ?? "NULL";
                    DtsLoggger.Logger.Info($"[MemberId: {memberId ?? "NULL"}] Code in list: {cCode}, InsertDate: {cInsertDate}");
                }
            }
            else
            {
                DtsLoggger.Logger.Info($"[MemberId: {memberId ?? "NULL"}] codes list is NULL");
            }


            if (codes.Any(c => c.Code == code && c.InsertDate > DateTime.Now.AddMinutes(smsLoginCodeMaxAgeInMinutes * (-1))))
            {
                _clubRepo.DeleteSmsCodesByMemberId(memberId);

                /*if (userFromDb.ClubCreditCard <= 0)
                {
                    if (await CheckUserHaveMaxBehatsdaaCreditCard(userFromDb.MemberId))
                    {
                        await _clubRepo.UpdateClubCreditCard(userFromDb.MemberId);
                        userFromDb = _clubRepo.GetUserByUserIDSync(memberId);
                        userDto.ClubCreditCard = userFromDb.ClubCreditCard;
                    }
                }*/

                int? CurrentPremiumeType = userFromDb.PremiumType;
                userFromDb = await CalculatePremiumTypeAndUpdateMember(userFromDb);
                userDto.IsPremiumTypeChanged = CurrentPremiumeType != userFromDb.PremiumType;
                if (userDto.IsPremiumTypeChanged)
                {
                    _clubRepo.RemoveAllProductsByMemberId(userFromDb.MemberId);
                }
                userDto.PremiumType = userFromDb.PremiumType;
                //אין זכאות על פי משרד הביטחון ולא ביצע התחוברות מעולם
                if ((userFromDb != null && userFromDb.PremiumType == (int)EIDFPremiumeType.NotApproved && userFromDb.UserToken == null && userFromDb.EncryptedUserPassword == null && userFromDb.LastEncryptedUserPassword == null))
                {
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
                }
                else
                {
                    if (userFromDb.PremiumType == (int)EIDFPremiumeType.NotApproved)
                    {
                        CreateUserToken(userDto.Id, true);
                    }
                    else
                    {
                        CreateUserToken(userDto.Id);
                    }

                    await _clubRepo.UpdateLoginAttempts(true, userFromDb);
                }

                //Eligibility expired 

                if (userFromDb.PremiumType == (int)EIDFPremiumeType.NotApproved)
                {
					var transactions = await _clubRepo.GetWebServiceTransactionsByMemberId(userFromDb.MemberId);
					if (transactions != null && transactions.Any())
					{
						userDto.UpdateOrRegister = ENeedUpdateOrFinishRegistration.UserExpired;
						return new BaseResponse<ResponseUserDTO>
						{
							Status = true,
							Data = new ResponseUserDTO(userDto),
							ErrorDescription = MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 736).MessageText
						};
						
					}
                    else {
						throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
					}
                }

                if (string.IsNullOrEmpty(userFromDb.MemberCardNumber) || await _clubRepo.ShouldAddCard())
                {
                    bool isCardAdded = _clubRepo.AddCardToMember(userFromDb.MemberId, false);
                    if (!isCardAdded)
                        throw new BusinessException("Failed Adding New Card To Member" + "  - USER_ID -  " + userFromDb.MemberId);

                }

                ResponseUserDTO userCache = ContextManager.SetCurrentUserCache(userDto.Id);
                await _clubRepo.UpdateLastLogin(userFromDb);

                // first login After reset all users passwords
                if (userFromDb.PremiumType != (int)EIDFPremiumeType.NotApproved)
                {
                    userDto.ForgetPasswordToken = Guid.NewGuid().ToString();
                    // ContextManager.SetCurrentUserTokenLoginCache(userDto.Id, userDto.Token);

                    return new BaseResponse<ResponseUserDTO>
                    {
                        Status = true,
                        Data = new ResponseUserDTO(userDto),
                        ErrorDescription = MessagesUtil.GetMessagesByKey(messageKeys)
                                                       .FirstOrDefault(m => m.MessageKey == (string.IsNullOrEmpty(userFromDb.LastEncryptedUserPassword) ? 10242 : 10307)).MessageText
                    };
                }
                userDto.UpdateOrRegister = userCache.UpdateOrRegister;
               
                //  _httpContextAccessor.HttpContext.Session.SetInt32("Login_Source", (int)ELoginType.OTPShortCode);
                
                return new BaseResponse<ResponseUserDTO>
                {
                    Status = true,
                    Data = new ResponseUserDTO(userDto),
                };


            } 
            else 
            {
                DtsLoggger.Logger.Info("Login UpdateLoginAttempts start" + "  - USER_ID -  " + userDto.Id);
                var ableToTryAgain = await _clubRepo.UpdateLoginAttempts(false, userFromDb);
                DtsLoggger.Logger.Info("Login UpdateLoginAttempts finish" + "  - USER_ID -  " + userDto.Id);

                if (!ableToTryAgain)
                {
                    _clubRepo.CreateLogAfterLoginBlockUser(memberId);
                    throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10041).MessageText, 10041);
                }
            }

            throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 10277).MessageText);
        }

        public async Task<BaseResponse<bool>> UpdateBiometricToken(BiometicsInfoDTO biometicsInfoDTO)
        {
            return await _clubRepo.UpdateBiometricToken(biometicsInfoDTO);
        }

        public async Task<BaseResponse<bool>> DeleteMember()
        {

            var response = new BaseResponse<bool>();
            var currentUser = ContextManager.CurrentUser();
            string memberId = currentUser.Id;
            DtsLoggger.Logger.Info($"start DeleteMember for memberid: {memberId}");

            try
            {

                int MemberInactivityInYears = int.Parse(ContextManager.ClubContext().AppConfig.FirstOrDefault(f => f.Key == ConfigurationKey.MemberInactivityInYears).Value);
                int decreaseMemberInactivityInYears = MemberInactivityInYears * -1;

                //get transactions in the last MemberInactivityInYears years 
                var dontHaveTransactions = _clubRepo.CheckAccountTransactions(memberId, decreaseMemberInactivityInYears);

                if (!dontHaveTransactions) //can't delete the member
                {
                    response.Data = false;
                    return response;
                }
                else //delete the member
                {
                    ////open case in crm
                    ContactUsDTO contactUsDTO = new ContactUsDTO
                    {
                        IdentityNumber = currentUser.IdentityNumber,
                        FullName = currentUser.FirstName + " " + currentUser.LastName,
                        InputEmail = currentUser.Email,
                        MobilePhone = currentUser.MobilePhone,
                        OrganizationId = ContextManager.CurrentOrganization().OrgId,
                        PremiumType = currentUser.PremiumType,
                        ClubCreditCard = currentUser.ClubCreditCard,
                        Description = "התקבלה בקשה למחיקת חשבון משתמש מהחבר. לטיפול באישור מנהל/ת בלבד",
                        CrmType = 418,
                        CrmSubjectId = 56,
                        //Subject = "צור קשר פניות לקוחות-כניסה לאתר"
                        Subject = "כניסה לאתר"

                    };
                    await _contactUsBL.OpenServiceCaseRequest(contactUsDTO);


                    //עדכון ה-Member בטבלת AllMembers ל - Active = 0
                    await _clubRepo.UpdateMemberActive(memberId, false);

                    DtsLoggger.Logger.Info($"DeleteMember - change member status to in-active for memberid: {memberId}");

                    response.Data = true;
                    return response;
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"DeleteMember - for memberid: {memberId}, error:{ex.Message}");

                throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 340).MessageText);
                //throw new BusinessException("תהליך מחיקת המשתמש נכשל, אנא נסה שוב במועד מאוחר יותר");
            }
        }


        private string GenerateEmailBody(ResponseUserDTO userDTO)
        {
            StringBuilder message = new StringBuilder();
            message.Append("<html xmlns='http://www.w3.org/1999/xhtml'>");
            message.Append("<head>");
            message.Append("</head>");
            message.Append("<body dir='rtl'>");
            message.Append("התקבלה בקשה למחיקת חשבון משתמש מהחבר:   <br />");

            if (!string.IsNullOrEmpty(userDTO.FirstName) && !string.IsNullOrEmpty(userDTO.LastName))
                message.Append("שם מלא: " + userDTO.FirstName + " " + userDTO.LastName + "<br />");

            if (!string.IsNullOrEmpty(userDTO.MobilePhone))
                message.Append("טלפון נייד: " + userDTO.MobilePhone + "<br />");

            if (!string.IsNullOrEmpty(userDTO.Email))
                message.Append("אימייל: " + userDTO.Email + "<br />");

            message.Append("תעודת זהות.: " + userDTO.IdentityNumber + "<br />");
            message.Append("</body>");
            message.Append("</html>");
            return message.ToString();
        }

        private string GenerateAccessToken(string memberId, bool isNotApprovedMemeber = false)
        {
            var jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetConfigByValue<string>(ConfigurationKey.JwtKey)));
            var credentials = new SigningCredentials(jwtSecurityKey, SecurityAlgorithms.HmacSha256);
            var claims = isNotApprovedMemeber ? 
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, memberId),
                    new Claim(ClaimTypes.Role, "unapproved_user")
                } :
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, memberId)
                };

            var token = new JwtSecurityToken(
                _configuration.GetConfigByValue<string>(ConfigurationKey.JwtIssuer),
                _configuration.GetConfigByValue<string>(ConfigurationKey.JwtAudience),
                claims,
                expires: DateTime.Now.AddMinutes(_configuration.GetConfigByValue<int>(ConfigurationKey.JwtExpireMinutes)),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string EncryptEmail(string email)
        {
            string encryptedEmail = "";
            string[] emailParts = email.Split('@');
            string username = emailParts[0];
            string domain = emailParts[1];

            if (username.Length >= 3)
                encryptedEmail += username.Substring(0, 3);
            encryptedEmail += new String('*', 3);
            encryptedEmail += username[username.Length - 1];
            encryptedEmail += "@" + domain.Substring(0, 2);
            encryptedEmail += "***";

            return encryptedEmail;
        }
    }
}

