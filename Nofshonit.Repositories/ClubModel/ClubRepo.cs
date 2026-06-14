using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nofshonit.Common;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.DTOs.Cards;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.DTOs.Limitations;
using Nofshonit.Common.DTOs.Product;
using Nofshonit.Common.DTOs.RequestDTOs;
using Nofshonit.Common.EF.Club;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Exceptions;
using Nofshonit.Common.Extensions;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Logs;
using Nofshonit.Repositories.DtsLogsModel;
using Nofshonit.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Nofshonit.Repositories.DtsOnlineModel;
using AllMembers = Nofshonit.Common.EF.Club.AllMembers;
using Requests = Nofshonit.Common.EF.Club.Requests;

namespace Nofshonit.Repositories.ClubModel
{
	public class ClubRepo : BaseRepo, IClubRepo
	{
		private const string AUTO_COMPLETE_SCRIPT = "SearchCategoryByName";
		private const string PROC_TEXT_PARAM = "@Text";
		private const string PROC_SUPER_CATEGORY_PARAM = "@SuperCategory";
		private const string PROC_SELECT_TOP_PARAM = "@SelectTop";
		private const string PROC_ORGID_PARAM = "@OrganizationID";
		private List<int> messageKeys;
		private readonly IDtsLogsRepo _dtsLogsRepo;
		private readonly IConfigurationManager _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IDtsOnlineRepo _dtsOnlineRepo;
		private ICacheManager Cache;

		public ClubRepo()
		{
			messageKeys = new List<int>();
			_dtsLogsRepo = Container.Resolve<IDtsLogsRepo>();
			_configuration = Container.Resolve<IConfigurationManager>();
			_httpContextAccessor = Container.Resolve<IHttpContextAccessor>();
			Cache = Container.Resolve<ICacheManager>();
			_dtsOnlineRepo = Container.Resolve<IDtsOnlineRepo>();
		}


		#region AppConfig 
		public async Task<List<AppConfig>> getValueByKeyList(List<string> keyList)
		{

			return await ContextManager.ClubContext().AppConfig.Where(app => keyList.Any(key => key == app.Key)).ToListAsync();

		}
		#endregion

		public bool CheckIfAbleToTryLogin(Common.EF.Club.AllMembers userFromDb)
		{
			return userFromDb.Active == 1;
		}

		#region User Methods

		public string GetOrCreateShortUrl(string longURL, string memberId)
		{
			try
			{
				using (var connection = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				{
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "SP_SendOldOrdersLink_SMS ";
					command.Parameters.AddWithValue("@longURL", longURL);
					command.Parameters.AddWithValue("@memberId", memberId);
					command.Parameters.AddWithValue("@OrganizationId", ContextManager.CurrentOrganization().OrgId);
					var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
					reader.Read();
					return reader.GetValue(0).ToString();

				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, "Error on GetOrCreateShortUrl");
				return null;
			}
		}
		public bool MemberExistInExcludeCretitCard(string memberId)
		{
			var tmp = ContextManager.ClubContext().ClubCreditCardExcluded.FirstOrDefault(x => x.MemberId == memberId);
			return tmp != null;
		}

		public async Task<List<Common.EF.Club.AllMembers>> GetAllMembers(int top = 100)
		{
			return await ContextManager.ClubContext().AllMembers.Take(top).ToListAsync();
		}

		public async Task<Common.EF.Club.AllMembers> GetUserByUserDTO(UserDTO userDto)
		{
			switch (userDto.LoginType)
			{
				case ELoginType.MemberIdEqualsPassword:
					return await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.MemberId.Equals(userDto.Id)
																											&& e.EncryptedUserPassword == null);

				case ELoginType.EmailAndPass:
					var userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.Email.Equals(userDto.Email)
																									 && e.EncryptedUserPassword.Equals(userDto.Password));
					if (userFromDb != null)
						return userFromDb;
					return null;
				case ELoginType.MemberIdAndPassword:
					userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.MemberId.Equals(userDto.Id)
																								 //&& e.EncryptedUserPassword.Equals(userDto.Password)
																								 );

					if (userFromDb != null)
						return userFromDb;

					var userFromDbLastPassword = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.MemberId.Equals(userDto.Id)
																						   && e.LastEncryptedUserPassword.Equals(userDto.Password));

					if (userFromDbLastPassword != null)
						throw new BusinessException("הנך מנסה להכנס עם סיסמה ישנה, הזן מחדש את סיסמתך החדשה.");
					return null;
				case ELoginType.UserNameAndPass:
					userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.MemberName.Equals(userDto.Username)
																								 && e.EncryptedUserPassword.Equals(userDto.Password));
					if (userFromDb != null)
						return userFromDb;
					return null;
				case ELoginType.Facebook:
				case ELoginType.Google:
					userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.AccessId.Equals(userDto.AccessID));
					if (userFromDb != null)
						return userFromDb;
					return null;
				case ELoginType.IdentityAndPassword:
					userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.Tz.Equals(userDto.IdentityNumber)
																								 && e.EncryptedUserPassword.Equals(userDto.Password));
					if (userFromDb != null)
						return userFromDb;

					userFromDbLastPassword = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.Tz.Equals(userDto.IdentityNumber)
																								 && e.LastEncryptedUserPassword.Equals(userDto.Password));
					if (userFromDbLastPassword != null)
						throw new BusinessException("הנך מנסה להכנס עם סיסמה ישנה, הזן מחדש את סיסמתך החדשה.");

					return null;
				default:
					return null;
			}
		}

		public async Task<bool> UpdateUserToken(string token)
		{
			var userFromDb = await GetUserByUserID(ContextManager.CurrentUser().Id);
			if (userFromDb != null)
			{
				using (var context = ContextManager.ClubContext())
				{
					context.Attach(userFromDb);
					userFromDb.UserToken = token;
					var value = await context.SaveChangesAsync();
					if (value > 0)
						return true;
					throw new Exception("Failed save changes. (UpdateUserToken)");
				}
			}
			throw new Exception("Cannot find user. (UpdateUserToken)");
		}

		public async Task<bool> UpdateUserToken(string memberId, string token)
		{
			var userFromDb = await GetUserByUserID(memberId);
			if (userFromDb != null)
			{
				using (var context = ContextManager.ClubContext())
				{
					context.Attach(userFromDb);
					userFromDb.UserToken = token;
					var value = await context.SaveChangesAsync();
					if (value > 0)
						return true;
					throw new Exception("Failed save changes. (UpdateUserToken)");
				}
			}
			throw new Exception("Cannot find user. (UpdateUserToken)");
		}

		public string UpdateUserTokenSync(string memberId, string token)
		{
			var userFromDB = GetUserByUserIDSync(memberId);
			using (var context = ContextManager.ClubContext())
			{
				context.Attach(userFromDB);
				userFromDB.UserToken = token;
				var value = context.SaveChanges();
				if (value > 0)
				{
					var user = GetUserByUserIDSync(memberId);
					return user.UserToken;
				}
				throw new Exception("Failed save changes. (UpdateUserToken)");
			}
			throw new Exception("Cannot find user. (UpdateUserToken)");
		}

		public async Task<bool> UpdateUserAccessID(ELoginType loginType)
		{
			ResponseUserDTO currentUser = ContextManager.CurrentUser();

			var accessId = currentUser.AccessID;
			switch (loginType)
			{
				case ELoginType.Facebook:
				case ELoginType.Google:
					using (var context = ContextManager.ClubContext())
					{
						var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(currentUser.Id));
						context.Attach(userFromDb);
						userFromDb.AccessId = accessId;
						var value = await context.SaveChangesAsync();
						if (accessId != null && value > 0)
							return true;
						throw new Exception("Failed save changes. (UpdateUserAccessID)");
					}
				default:
					using (var context = ContextManager.ClubContext())
					{
						var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(currentUser.Id));
						context.Attach(userFromDb);
						userFromDb.AccessId = null;
						var value = await context.SaveChangesAsync();
						return true;
					}
			}
		}

		public async Task<bool> UpdateLastLogin(Common.EF.Club.AllMembers userFromDb)
		{
			using (var context = ContextManager.ClubContext())
			{
				context.Attach(userFromDb);
				userFromDb.UserSiteLastLogin = DateTime.Now;
				userFromDb.PrivacyPolicyAcceptedDate = userFromDb.PrivacyPolicyAcceptedDate == null ? DateTime.Now : userFromDb.PrivacyPolicyAcceptedDate;
				var value = await context.SaveChangesAsync();
				if (value > 0)
					return true;
				throw new Exception("Failed save changes. (updateLastLogin)");
			}

		}

		public void AddMemberToAllowanceHistory(AllowanceHistory allowanceHistory)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter(){ SqlDbType = SqlDbType.Bit , ParameterName="p0" , Value = allowanceHistory.ClubCreditCard},
						new SqlParameter(){ SqlDbType = SqlDbType.Int , ParameterName="p1" , Value = allowanceHistory.IDFResponse},
						new SqlParameter(){ SqlDbType = SqlDbType.DateTime , ParameterName="p2" , Value = allowanceHistory.InsertDate},
						new SqlParameter(){ SqlDbType = SqlDbType.NVarChar , ParameterName="p3" , Value = allowanceHistory.MemberID},
						new SqlParameter(){ SqlDbType = SqlDbType.Int , ParameterName="p4" , Value = allowanceHistory.PremiumType}
					};

					context.Database.ExecuteSqlRaw("INSERT INTO [AllowanceHistory] ([ClubCreditCard], [IDFResponse], [InsertDate]," +
						" [MemberID], [PremiumType]) VALUES (@p0, @p1, @p2, @p3, @p4)", parameters);
					context.SaveChanges();

				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, "Error on AddMemberToAllowanceHistory =>{0}", allowanceHistory.MemberID);
			}
		}
		public async Task<Nofshonit.Common.EF.Club.AllMembers> GetUserByUserID(string Id)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.AllMembers.FirstOrDefaultAsync(e => e.MemberId.Equals(Id));
				if (userFromDb != null)
					return userFromDb;
				return null;
			}

		}
		public async Task<bool> UpdateMemberCookiesAcceptedDate(string Id)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.AllMembers.FirstOrDefaultAsync(e => e.MemberId.Equals(Id));
				if (userFromDb != null)
					context.Attach(userFromDb);
				userFromDb.CookiesAcceptedDate = userFromDb.CookiesAcceptedDate == null ? DateTime.Now : userFromDb.CookiesAcceptedDate;
				await context.SaveChangesAsync();

				return true;
			}
		}

		public Common.EF.Club.AllMembers GetUserByUserIDSync(string Id)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = context.AllMembers.FirstOrDefault(e => e.MemberId.Equals(Id));
				return userFromDb;
			}

		}

		public async Task<Common.EF.Club.AllMembers> GetUserByUserToken(string token)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.UserToken.Equals(token));
				if (userFromDb != null)
					return userFromDb;
				return null;
			}
		}

		public async Task<bool> CreateNewUser(Common.EF.Club.AllMembers userEF)
		{
			if (userEF != null)
			{
				using (var context = ContextManager.ClubContext())
				{
					await context.AllMembers.AddAsync(userEF);
					await context.SaveChangesAsync();
					return true;
				}
			}
			return false;
		}

		public async Task<bool> CreateNewRegistrationAudit(RegistrationAudits regAudit)
		{
			try
			{
				if (regAudit != null)
				{
					using (var context = ContextManager.ClubContext())
					{
						await context.RegistrationAudits.AddAsync(regAudit);
						await context.SaveChangesAsync();
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}

		}
		public bool UpdateNewRegistrationAudit(RegistrationAudits regAudit)
		{
			try
			{
				if (regAudit != null)
				{
					using (var context = ContextManager.ClubContext())
					{
						var regAuditFromDb = context.RegistrationAudits.Where(x => x.IdentityNumber == regAudit.IdentityNumber).FirstOrDefault();
						if (regAuditFromDb != null)
						{
							regAuditFromDb.Cellphone = regAudit.Cellphone;
							regAuditFromDb.CreatedDate = regAudit.CreatedDate;
							regAuditFromDb.Darga = regAudit.Darga;
							regAuditFromDb.PremiumType = regAudit.PremiumType;
							regAuditFromDb.IdentityNumber = regAudit.IdentityNumber;
							regAuditFromDb.IsMember = regAudit.IsMember;
							regAuditFromDb.AttemptsCount = regAudit.AttemptsCount;
							regAuditFromDb.ClubCreditCard = regAudit.ClubCreditCard;
							context.SaveChanges();
						}
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}



		}

		public async Task<List<Common.EF.Club.RegistrationAudits>> GetAllRegistrationAudits(int top = 500)
		{
			return await ContextManager.ClubContext().RegistrationAudits.Take(top).ToListAsync();
		}

		public async Task<Common.EF.Club.RegistrationAudits> GetRegistrationAuditByIdentity(string identityNumber)
		{
			return await ContextManager.ClubContext().RegistrationAudits.FirstOrDefaultAsync(u => u.IdentityNumber.Equals(identityNumber));
		}

		public bool IsMemebrInBehatzdaaNoCards(string MemberId)
		{
			var member = ContextManager.ClubContext().BehatzdaaNoCards.Where(b => b.Tz == MemberId).FirstOrDefault();
			return member != null;
		}

		public async Task<bool> UpdateNewUser(Common.EF.Club.AllMembers userEF, Common.EF.Club.AllMembers updatedMemebr) //Deal with the special situation of 800K already signed up Histadrut users 
		{
			if (userEF != null)
			{
				using (var context = ContextManager.ClubContext())
				{
					context.Attach(userEF);
					userEF.EncryptedUserPassword = updatedMemebr.EncryptedUserPassword;
					userEF.UserToken = updatedMemebr.UserToken;
					userEF.MobilePhone = updatedMemebr.MobilePhone;
					userEF.RegistrationDate = updatedMemebr.RegistrationDate;
					await context.SaveChangesAsync();
					return true;
				}
			}
			return false;
		}

		public async Task<Common.EF.Club.AllMembers> SetIdfDataForMember(IDFValidationResponseDTO member)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					Common.EF.Club.AllMembers userFromDb = context.AllMembers.FirstOrDefault(u => u.MemberId.Equals(member.Zehut));

					if (userFromDb != null && member.isValid.HasValue)
					{
						userFromDb.PremiumType = (int)member.premiumeType;
						if (member.premiumeType == EIDFPremiumeType.NotApproved)
							userFromDb.PremiumType = 4;
						if (userFromDb.ClubCreditCard != 2)
						{
							if (member.ClubCreditCard)
								userFromDb.ClubCreditCard = 1;
							else
								userFromDb.ClubCreditCard = 0;
						}
						userFromDb.Darga = (int)member.darga;
						context.SaveChanges();
					}
					return userFromDb;
				}
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public async Task<bool> UpdateMember(UserDTO userDto)
		{
			ResponseUserDTO currentUser = ContextManager.CurrentUser();

			using (var context = ContextManager.ClubContext())
			{
				if (userDto != null)
				{
					var userFromDb = context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(currentUser.Id)).Result;
					if (userFromDb != null)
					{
						var id = currentUser.Id;
						context.Attach(userFromDb);
						#region Updating the details
						{
							//userFromDb.MemberId = id;
							userFromDb.MemberFirstName = userDto.FirstName;
							userFromDb.MemberLastName = userDto.LastName;
							userFromDb.MemberName = $"{userDto.FirstName.Trim()} {userDto.LastName.Trim()}";
							userFromDb.BirthDate = userDto.BirthDate;
							//userFromDb.Tz = currentUser.IdentityNumber;
							userFromDb.MobilePhone = userDto.MobilePhone;
							userFromDb.PhoneNumber = userDto.PhoneNumber;
							userFromDb.CityName = userDto.CityName;
							userFromDb.City = userDto.CityId;
							userFromDb.Address = $"{userDto.StreetName} {userDto.HouseNumber}";
							userFromDb.StreetName = userDto.StreetName;
							userFromDb.HouseNumber = userDto.HouseNumber;
							userFromDb.ApartmentNumber = userDto.ApartmentNumber;
							userFromDb.Zip = userDto.Zip;
							userFromDb.Email = userDto.Email;
							userFromDb.NumOfChildren = userDto.NumOfChildren;
							userFromDb.Gender = (short)userDto.Gender;
							userFromDb.PartnerName = userDto.PartnerName;
							userFromDb.PartnerPhone = userDto.PartnerPhone;
							userFromDb.PartnerEmail = userDto.PartnerEmail;
							userFromDb.AccessId = userDto.AccessID;
							userFromDb.UserToken = currentUser.Token;
							userFromDb.PremiumType = userDto.PremiumType ?? userFromDb.PremiumType;
							userFromDb.MemberSpecialId = userDto.MemberSpecialID;
							userFromDb.ForgetPasswordToken = userDto.ForgetPasswordToken;
							userFromDb.ForgetPasswordCreated = userDto.ForgetPasswordCreated;
							userFromDb.LastUpdateMember = DateTime.Now;
							userFromDb.AllowSmsAndMail = userDto.AllowSmsAndMail;
							//userFromDb.PinCode = userDto.PinCode;
							userFromDb.FactorySymbol = userDto.FactorySymbol;
							userFromDb.Mailbox = userDto.Mailbox;
							userFromDb.Entrance = userDto.Entrance;
							userFromDb.PartnerPhone = userDto.PartnerPhone;
							userFromDb.PrivacyPolicyAcceptedDate = userFromDb.PrivacyPolicyAcceptedDate == null ? DateTime.Now : userFromDb.PrivacyPolicyAcceptedDate;
						}
						#endregion
						var value = await context.SaveChangesAsync();
						return true;
					}
					throw new Exception("Cannot find user in Db according to details.");
				}
				throw new Exception("Given UserDTO = null.");
			}
		}
		public bool CreateUpdateUserRequestLog(UserDTO oldUser, UserDTO newUser, bool isUpdateDetailsApproved)
		{
			using (var context = ContextManager.ClubContext())
			{
				string remark = BuildRemark(oldUser, newUser, isUpdateDetailsApproved);

				if (string.IsNullOrWhiteSpace(remark))
					return false;

				Requests requestLog = new Requests
				{
					RequestTime = DateTime.Now,
					RequestSource = 2,
					RequestOp = 916,
					RequestType = 1,
					RequestStatus = 1,
					Id1 = newUser.Id,
					Remark = remark,
					Xmlparam = "<Updated>1</Updated>",
				};

				context.Requests.Add(requestLog);
				context.SaveChanges();

				return true;
			}
		}

		public bool CreateNewUserRequestLog(string identityNumber, bool isUpdateDetailsApproved)
		{
			using (var context = ContextManager.ClubContext())
			{
				if (!isUpdateDetailsApproved)
					return false;

				Requests requestLog = new Requests
				{
					RequestTime = DateTime.Now,
					RequestSource = 2,
					RequestOp = 916,
					RequestType = 267,
					RequestStatus = 1,
					Id1 = identityNumber,
					Remark = " לקוח אישר הבנת משמעיות של הכנסת פרטי התקשרות באתר",
					Xmlparam = "<Updated>2</Updated>",
				};

				context.Requests.Add(requestLog);
				context.SaveChanges();
			}
			return true;
		}
		public bool CreateLogAfterLoginBlockUser(string identityNumber)
		{
			using (var context = ContextManager.ClubContext())
			{
				Requests requestLog = new Requests
				{
					RequestTime = DateTime.Now,
					RequestSource = 2,
					RequestOp = 916,
					RequestType = 268,
					RequestStatus = 1,
					Id1 = identityNumber,
					Xmlparam = "<Updated>1</Updated>",
				};

				context.Requests.Add(requestLog);
				context.SaveChanges();
			}
			return true;
		}
		public static string BuildRemark(UserDTO oldUser, UserDTO newUser, bool isUpdateDetailsApproved)
		{
			List<string> changes = new List<string>();

			if ((oldUser.Email ?? "").Trim() != (newUser.Email ?? "").Trim())
				changes.Add($"Email עודכן מ: {oldUser.Email ?? "ריק"} ל: {newUser.Email ?? "ריק"}");

			if ((oldUser.MobilePhone ?? "").Trim() != (newUser.MobilePhone ?? "").Trim())
				changes.Add($"MobilePhone עודכן מ: {oldUser.MobilePhone ?? "ריק"} ל: {newUser.MobilePhone ?? "ריק"}");

			if ((oldUser.PartnerEmail ?? "").Trim() != (newUser.PartnerEmail ?? "").Trim())
				changes.Add($"PartnerEmail עודכן מ: {oldUser.PartnerEmail ?? "ריק"} ל: {newUser.PartnerEmail ?? "ריק"}");

			if ((oldUser.PartnerPhone ?? "").Trim() != (newUser.PartnerPhone ?? "").Trim())
				changes.Add($"PartnerPhone עודכן מ: {oldUser.PartnerPhone ?? "ריק"} ל: {newUser.PartnerPhone ?? "ריק"}");

			if (isUpdateDetailsApproved)
			{
				changes.Add("לקוח אישר הבנת משמעיות של הכנסת פרטי התקשרות באתר");
			}

			return string.Join(" | ", changes);
		}





		public async Task<bool> UpdateMemberActive(string memberId, bool active)
		{
			using (var context = ContextManager.ClubContext())
			{
				if (!string.IsNullOrEmpty(memberId))
				{
					var userFromDb = context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(memberId)).Result;
					if (userFromDb != null)
					{
						if (active)
							userFromDb.Active = 1;
						else
							userFromDb.Active = 0;

						context.Attach(userFromDb);

						var value = await context.SaveChangesAsync();
						return true;
					}
					throw new Exception("Cannot find user in Db according to details.");
				}
				throw new Exception("Given UserDTO = null.");
			}
		}

		public async Task<bool> UpdateLoginAttempts(bool nullify, AllMembers userFromDb)
		{
			var lastLoginAttemptsExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.LastLoginAttemptsExpiry);
			using (var context = ContextManager.ClubContext())
			{
				context.Attach(userFromDb);

				if (nullify)
				{
					userFromDb.LoginAttempts = 0;
					userFromDb.LastLoginAttempts = DateTime.Now;
					await context.SaveChangesAsync();
					return true;
				}

				userFromDb.LoginAttempts += 1;
				userFromDb.LastLoginAttempts = DateTime.Now;

				if (userFromDb.LoginAttempts >= lastLoginAttemptsExpiry)
				{
					userFromDb.Active = 2;
					userFromDb.LoginAttempts = 0;
					await context.SaveChangesAsync();
					return false;
				}

				await context.SaveChangesAsync();
				return true;
			}
		}

		public async Task<bool> UpdateJoinByOtpAttempts(bool nullify, RegistrationAudits regAudit)
		{

			var lastLoginAttemptsMinExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.LastLoginAttemptsMinExpiry);
			var lastRegisterAttemptsExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.lastRegisterAttemptsExpiry);


			using (var context = ContextManager.ClubContext())
			{
				if (regAudit == null)
					return true;
				if (nullify)
				{
					context.Attach(regAudit);
					regAudit.AttemptsCount = 0;
					regAudit.LastAttemptDate = DateTime.Now; // כל פעם שמבוצע תהליך רישום לעדכן את הפרמטר
					await context.SaveChangesAsync();
					return true;
				}
				if (regAudit.AttemptsCount < lastRegisterAttemptsExpiry)
				{
					context.Attach(regAudit);
					regAudit.AttemptsCount += 1;
					regAudit.LastAttemptDate = DateTime.Now;
					await context.SaveChangesAsync();
					return true;
				}
				if (regAudit.AttemptsCount >= lastRegisterAttemptsExpiry && (DateTime.Now - regAudit.LastAttemptDate.Value).Minutes >= lastLoginAttemptsMinExpiry)
				{
					context.Attach(regAudit);
					regAudit.AttemptsCount = 0;
					regAudit.LastAttemptDate = DateTime.Now; // כל פעם שמבוצע תהליך לוגין לעדכן את הפרמטר
					await context.SaveChangesAsync();
					return true;
				}
				if (regAudit.AttemptsCount >= lastRegisterAttemptsExpiry && (DateTime.Now - regAudit.LastAttemptDate.Value).Minutes < lastLoginAttemptsMinExpiry)
					return false;

				return false;
			}
		}

		public async Task<bool> CanSendOtp(RegistrationAudits regAudit)
		{
			var lastLoginAttemptsMinExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.LastLoginAttemptsMinExpiry);
			var lastLoginAttemptsExpiry = _configuration.GetConfigByValue<int>(ConfigurationKey.LastLoginAttemptsExpiry);

			using (var context = ContextManager.ClubContext())
			{
				if (regAudit == null)
					return false;

				if (regAudit.AttemptsCount >= lastLoginAttemptsExpiry && (DateTime.Now - regAudit.LastAttemptDate.Value).Minutes < lastLoginAttemptsMinExpiry)
					return false;

				return true;
			}
		}


		private async Task<Common.EF.Club.AllMembers> GetUserByLoginTypeParameter(UserDTO userDTO)
		{
			using (var context = ContextManager.ClubContext())
			{
				switch (userDTO.LoginType)
				{
					case ELoginType.EmailAndPass:
						var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.Email.Equals(userDTO.Email));
						if (userFromDb != null)
							return userFromDb;
						throw new Exception("No valid Email. (GetUserByLoginTypeParameter)");
					case ELoginType.MemberIdAndPassword:
						messageKeys.Add(722);
						userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(userDTO.Id));
						if (userFromDb != null)
							return userFromDb;
						throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
					case ELoginType.UserNameAndPass:
						userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberName.Equals(userDTO.Username));
						if (userFromDb != null)
							return userFromDb;
						throw new Exception("No valid IdentityNumber. (GetUserByLoginTypeParameter)");
					case ELoginType.Facebook:
					case ELoginType.Google:
						userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.AccessId.Equals(userDTO.AccessID));
						if (userFromDb != null)
							return userFromDb;
						throw new Exception("No valid AccessId. (GetUserByLoginTypeParameter)");
					case ELoginType.IdentityAndPassword:
						messageKeys.Add(722);
						userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.Tz.Equals(userDTO.IdentityNumber));
						if (userFromDb != null)
							return userFromDb;
						throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
					case ELoginType.BiometricToken:
						userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.Tz.Equals(userDTO.IdentityNumber));
						if (userFromDb != null)
							return userFromDb;
						throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 722).MessageText);
					default:
						messageKeys.Add(51121);
						throw new BusinessException(MessagesUtil.GetMessagesByKey(messageKeys).FirstOrDefault(m => m.MessageKey == 51121).MessageText);
				}
			}
		}

		public async Task<bool> IsMemberAllowedLogin()
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.MembersAllowedLogin.FirstOrDefaultAsync(u => u.MemberId.Equals(ContextManager.CurrentUser().Id));
				return userFromDb != null;
			}
		}

		public async Task<bool> IsMemberAllowedRegister(string Id)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.MembersAllowedLogin.FirstOrDefaultAsync(u => u.MemberId.Equals(Id));
				return userFromDb != null;
			}
		}
		public MembersAllowedLogin GetMemberFromAllowedRegister(string memberId)
		{
			List<MembersAllowedLogin> MembersAllowedLogin = GetMemberAllowedRegisterFromCache().Result;
			var member = MembersAllowedLogin.FirstOrDefault(m => m.MemberId == memberId);

			return member;
		}

		public async Task<List<MembersAllowedLogin>> GetMemberAllowedRegisterFromCache()
		{
			List<MembersAllowedLogin> MembersAllowedLogin = new List<MembersAllowedLogin>();
			if (CacheManager.Exists("MembersAllowedLogin"))
				MembersAllowedLogin = (List<MembersAllowedLogin>)CacheManager.Get("MembersAllowedLogin");

			var membersAllowedLogin = (List<MembersAllowedLogin>)Cache.Get("MembersAllowedLogin");
			if (membersAllowedLogin == null)
			{
				using (var context = ContextManager.ClubContext())
				{
					membersAllowedLogin = await context.MembersAllowedLogin.ToListAsync();
				}
				Cache.Set("MembersAllowedLogin", membersAllowedLogin);
			}
			return membersAllowedLogin;
		}

		public async Task<Common.EF.Club.AllMembers> IsMemberAllowedLogin(string Id)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.MembersAllowedLogin.FirstOrDefaultAsync(u => u.MemberId.Equals(Id));
				if (userFromDb != null)
				{

					Common.EF.Club.AllMembers user = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(userFromDb.MemberId));
					return user;
				}
			}
			return null;
		}

		public List<string> GetAllMembersAllowedLogin()
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.MembersAllowedLogin.Select(x => x.MemberId).ToList();
			}
		}

		public async Task<bool> UpdateMemberStatusAndMemberSpecialID(int? premiumType, string memberSpecialID)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(ContextManager.CurrentUser().Id));

				if (userFromDb != null)
				{
					context.Attach(userFromDb);
					userFromDb.PremiumType = premiumType;
					userFromDb.MemberSpecialId = memberSpecialID;
					if (memberSpecialID.Equals("0000103166") || memberSpecialID.Equals("0000362509") || memberSpecialID.Equals("103166") || memberSpecialID.Equals("362509"))
					{
						userFromDb.ClubCreditCard = 1;
						userFromDb.ClubCreditCardUpdateDate = DateTime.Now;
					}
					await context.SaveChangesAsync();
					return true;
				}
				throw new Exception("Cannot find user in DB. (UpdateMemberStatusAndMemberSpecialID)");
			}
		}

		public async Task<bool> ShouldAddCard()
		{

			using (var context = ContextManager.ClubContext())
			{
				var cardFromDb = await context.Cards.
					Where(m => m.Idmember.Equals(ContextManager.CurrentUser().Id.Trim()) && m.CardType == (int)ECardType.VERIFONE && (m.CardStatus == (int)ECardStatus.BLOCK || m.CardStatus == (int)ECardStatus.ACTIVE)).
					OrderBy(o => o.AddedTime).
					FirstOrDefaultAsync();
				return cardFromDb == null;
			}
		}

		public async Task<bool> ShouldAddCard(string memberId)
		{
			using (var context = ContextManager.ClubContext())
			{
				// var userID = ContextManager.CurrentUser().Id;
				var cardFromDb = await context.Cards.
					Where(m => m.Idmember.Equals(memberId.Trim()) && m.CardType == (int)ECardType.VERIFONE && (m.CardStatus == (int)ECardStatus.BLOCK || m.CardStatus == (int)ECardStatus.ACTIVE)).
					OrderBy(o => o.AddedTime).
					FirstOrDefaultAsync();
				return cardFromDb == null;
			}
		}
		public async Task<bool> UpdateClubCreditCard(AllMembers userFromDb, int ClubCreditCard = 1)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					if (userFromDb != null && (userFromDb.ClubCreditCard != 2 || ClubCreditCard == 1))
					{
						if (userFromDb != null)
						{
							context.Attach(userFromDb);
							userFromDb.ClubCreditCard = ClubCreditCard;
							return await context.SaveChangesAsync() > 0;

						}
					}
				}
			}
			catch (Exception e)
			{
				LoggerHelper.Error(e, "Error on UpdateClubCreditCard =>{0}", userFromDb.MemberId);
			}
			return false;
		}

		public async Task<bool> UpdatePassword(UserPasswordInfoDTO userPasswordInfoDto)
		{
			using (var context = ContextManager.ClubContext())
			{
				if (userPasswordInfoDto != null)
				{
					var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(userPasswordInfoDto.MemberId));
					if (userFromDb != null)
					{
						context.Attach(userFromDb);
						if (!string.IsNullOrEmpty(userFromDb.EncryptedUserPassword))
							userFromDb.LastEncryptedUserPassword = userFromDb.EncryptedUserPassword;

						userFromDb.EncryptedUserPassword = userPasswordInfoDto.NewPassword;
						userFromDb.PasswordChanged = DateTime.Now;
						userFromDb.ForgetPasswordToken = null;
						//TODO VAlidate remove
						//if (userFromDb.EncryptedUserPassword.Equals(userFromDb.LastEncryptedUserPassword))
						//    userFromDb.LastEncryptedUserPassword = null;
						await context.SaveChangesAsync();

						return true;
					}
					throw new Exception("No user found in DB according to the given details. (UpdatePassword)");
				}
				throw new Exception("Given details are Null. (UpdatePassword)");
			}
		}

		public async Task<Common.EF.Club.AllMembers> GetUserByEmailAsync(string email)
		{
			using (var context = ContextManager.ClubContext())
			{
				return await context.AllMembers.FirstOrDefaultAsync(userDb => userDb.Email.ToLower().Equals(email.ToLower()));
			}
		}

		public string GetNextSequenceValue()
		{
			var connectionString = ContextManager.ClubContext()._connectionString;
			SqlConnection connection = new SqlConnection(connectionString);
			SqlCommand command = new SqlCommand("GetNextSequenceValue", connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			reader.Read();
			var retValue = reader.GetValue(0);
			return retValue.ToString();
		}

		public int UpdateForRecoverPasswordAsync(UserDTO userDto)
		{
			var userFromDB = GetUserByUserID(userDto.Id).Result;
			using (var context = ContextManager.ClubContext())
			{
				context.Attach(userFromDB);
				userFromDB.ForgetPasswordToken = userDto.ForgetPasswordToken;
				userFromDB.ForgetPasswordCreated = userDto.ForgetPasswordCreated;
				return context.SaveChanges();
			}
		}

		public (string, DateTime?) ValidateRecoverPasswordToken(UserDTO userDto, string tokenCode)
		{
			var userFromDB = GetUserByUserID(userDto.Id).Result;
			using (var context = ContextManager.ClubContext())
			{
				context.Attach(userFromDB);
				return (userDto.ForgetPasswordToken, userFromDB.ForgetPasswordCreated);
			}
		}

		// For RecoverPassword
		public async Task<Common.EF.Club.AllMembers> GetUserByTokenCode(string tokenCode)
		{
			var userFromDb = await ContextManager.ClubContext().AllMembers.FirstOrDefaultAsync(e => e.ForgetPasswordToken.Equals(tokenCode));
			if (userFromDb != null)
				return userFromDb;
			return null;
		}

		public bool AddCardToMember(string memberId, bool IsPhysical)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = GetUserByUserID(memberId).Result;
				if (userFromDb != null)
				{
					var cardFromDb = context.Cards.FirstOrDefault(c => c.Idmember.Equals(memberId) && c.CardType == (int)ECardType.VERIFONE && c.CardStatus == (int)ECardStatus.ACTIVE);
					if (cardFromDb != null)
					{
						context.Attach(userFromDb);
						userFromDb.MemberCardNumber = cardFromDb.CardNumber;
						context.SaveChanges();
						ContextManager.SetCurrentUserCache(userFromDb.MemberId);
						return true;
					}
					var connectionString = ContextManager.ClubContext()._connectionString;
					var connection = new SqlConnection(connectionString);

					int bitCardType = IsPhysical ? 1 : 0;
					var command = new SqlCommand($"Add_Card_to_Member '{memberId}','{bitCardType}'", connection);

					connection.Open();
					var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
					reader.Read();
					var retValue = reader.GetValue(0).ToString();
					if (Int64.TryParse(retValue, out _) && retValue.Trim().Length == 16)
					{
						context.Attach(userFromDb);
						userFromDb.MemberCardNumber = retValue.ToString();
						context.SaveChanges();
						ContextManager.SetCurrentUserCache(userFromDb.MemberId);
						return true;
					}
				}
				return false;
			}
		}



		public async Task<AllMembersProperties> GetMembersPropertiesById(string memberId)
		{
			using (var context = ContextManager.ClubContext())
			{
				return await context.AllMembersProperties.FirstOrDefaultAsync(p => p.MemberId.Equals(memberId));
			}
		}

		public async Task<Common.EF.Club.Requests> GetRequest(string walletId, string memberId)
		{
			using (var context = ContextManager.ClubContext())
			{
				var request = await context.Requests.Where(p => p.WalletId.ToString().Equals(walletId) && p.Id1.Equals(memberId)).ToListAsync();
				if (request != null && request.Count > 0)
				{
					var reqList = request.OrderByDescending(e => e.RequestTime.Date).ThenByDescending(x => x.RequestTime.TimeOfDay).ToList();
					return reqList[0];
				}
				//return request[request.Count-1];

				return null;
			}
		}

		#endregion

		#region Product


		/// <summary>
		/// find varianDTO list by category. important! - OrderLimit,IsEmpty is not set
		/// </summary>
		/// <returns></returns>
		public List<VariantDTO> GetVariantDTOs(long categoryId, string memberId, bool orgIsNewSubsidy, SimpleInt categoryRedimType, Dictionary<int, string> redimTypesByOrganization, List<Common.EF.DTS_Online.TypeImplementationDate> implementationTypesList)
		{
			var result = new List<VariantDTO>();
			var now = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
			using (var context = ContextManager.ClubContext())
			{
				var isClubCreditCard = context.AllMembers.AsNoTracking().FirstOrDefault(x => x.MemberId == memberId).ClubCreditCard > 0;
				var productVars = context.CategoryVariants.AsNoTracking().Where(x => x.CategoryNumber == categoryId)
					.Join(context.ProductsVars.AsNoTracking().Where(y => !y.DisabledToOrder && !y.Disabled)
						  , cv => cv.Barcode, pv => pv.FullBarCode, (cv, pv) => pv)
					.ToList();
				productVars = productVars.Where(y => ((y.LastImplementationDate.HasValue && !ProductFunctions.CheckVariantExpired(y)) || !y.LastImplementationDate.HasValue)).ToList();
				var barcodes = productVars.Select(x => x.FullBarCode).ToList();
				var specsByVarsList = context.BusinessSubTypeSpecificationByVariants.AsNoTracking().Where(x => barcodes.Contains(x.BarCode)).ToList();
				var businessSubTypeList = productVars.Select(x => (int)x.BusinessSubTypeId.GetValueOrDefault()).Distinct().ToList();
				var specsCurrentList = context.BusinessSubTypeSpecificationCurrent.AsNoTracking().Where(x => businessSubTypeList.Contains(x.BusinessSubTypeId)).ToList();
				result = context.CategoryVariants.AsNoTracking().Where(x => x.CategoryNumber == categoryId)
					.Join(productVars, cv => cv.Barcode, pv => pv.FullBarCode, (cv, pv) => pv)

					.ToList().ConvertAll(x => new VariantDTO()
					{
						BarCode = x.FullBarCode,
						BenefitTypeId = ProductFunctions.GetBenefitTypeIdByVariantType(x.VariantType),
						Business = null,//TODO:must be implemented in BusinessServices,
						KupaPrice = Math.Round(x.CupaPrice.GetValueOrDefault(), 1),
						ExpireDate = (int)x.BusinessSubTypeId.GetValueOrDefault() == 26 ? DateTime.MinValue : (ProductFunctions.ReCalculateLastImplementationDate(x.LastImplementationDate.HasValue ? x.LastImplementationDate.Value : now, x.TypeCalcImplementationDate.HasValue ? x.TypeCalcImplementationDate.Value : 0, implementationTypesList)),//TODO: must be implement in BudgetServices. see in doc, calculated field
						GiftCardValue = x.VariantType == 11 ? x.LoadingAmount : null,//11->giftcard
						IsEmpty = false,//TODO: need be implemented in BudgetServices. calculation of the stock
						IsSendToFriend = x.IsSendToFriend.GetValueOrDefault(),
						Name = x.ShortNameVar,
						OrderLimit = -1,//TODO: must be implement in BudgetServices, limit by to user (daily weekly monthly)
						MonthlyLimit = !string.IsNullOrEmpty(x.MemberMonthlyLimitFormula) && (x.MemberMonthlyLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberMonthlyLimitFormula) : -1,
						YearlyLimit = !string.IsNullOrEmpty(x.MemberYearlyLimitFormula) && (x.MemberYearlyLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberYearlyLimitFormula) : -1,
						GeneralLimit = !string.IsNullOrEmpty(x.MemberGeneralLimitFormula) && (x.MemberGeneralLimitFormula.All(char.IsNumber)) ? int.Parse(x.MemberGeneralLimitFormula) : -1,
						Price = ProductFunctions.GetVariantPrice(ContextManager.CurrentUser().PremiumType, x, specsByVarsList.FirstOrDefault(y => y.BarCode == x.FullBarCode), specsCurrentList.FirstOrDefault(y => y.BusinessSubTypeId == x.BusinessSubTypeId), isClubCreditCard, orgIsNewSubsidy),
						RedimTypeId = ProductFunctions.GetVariantRedimTypeId(x, categoryRedimType),
						RedimTypeName = ProductFunctions.GetVariantRedimTypeName(x, categoryRedimType, redimTypesByOrganization),
						BusinessSubTypeId = (int)x.BusinessSubTypeId,
						IsCampaign = x.Iscampaign.GetValueOrDefault(),
						IrgunPrice = decimal.Parse(x.IrgunPriceFormula),

					});
			}
			return result;
		}

		public long CategoryIdByEventId(int eventId)
		{
			long result = 0;

			using (var context = ContextManager.ClubContext())
			{
				var first = context.ProductsVars.Where(x => x.TicketsHubEventId == eventId).Join(context.CategoryVariants, p => p.FullBarCode, c => c.Barcode, (p, c) => c).FirstOrDefault();
				if (first != null)
				{
					result = first.CategoryNumber;
				}
			}
			return result;
		}

		public long CategoryIdByVariant(string barcode)
		{
			long result = 0;

			using (var context = ContextManager.ClubContext())
			{
				var first = context.ProductsVars.Where(x => x.FullBarCode == barcode).Join(context.CategoryVariants.Where(x => x.Barcode == barcode), p => p.FullBarCode, c => c.Barcode, (p, c) => c).FirstOrDefault();
				if (first != null)
				{
					result = first.CategoryNumber;
				}
			}
			return result;
		}

		public bool ImplementNewCardVariant(bool isDigital = false)
		{
			try
			{
				ResponseUserDTO currentUser = ContextManager.CurrentUser();

				using (var context = ContextManager.ClubContext())
				{
					DtsLoggger.Logger.Info("ImplementNewCardVariant", currentUser.Id);

					var newCardVariantBarcode = _configuration.GetConfigByValue<string>(ConfigurationKey.NewCardVariant);
					var newDigitalCardVariantBarcode = _configuration.GetConfigByValue<string>(ConfigurationKey.NewDigitalCardVariant);

					var cardBarcode = isDigital ? newDigitalCardVariantBarcode.ToString() : newCardVariantBarcode.ToString();
					var order = context.Atractionsorders.FirstOrDefault(x => x.MemberId == currentUser.Id
					&& x.BarCode == cardBarcode
					&& x.MemberOrderQuntity == 1
					&& x.MemberOrderBlance == 0);
					if (order != null)
					{
						order.MemberOrderBlance = 1;
						//order.MemberOrderDateExe = DateTime.Now;
						// order.MemberTerminalExe = "1002486-3";
						context.SaveChanges();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		#endregion

		#region ShopingBasket

		public ProductsVars GetVariant(string barcode)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ProductsVars.FirstOrDefault(v => v.FullBarCode.Equals(barcode));
			}
		}

		public List<ProductsVars> GetVariantsByCategoryId(long categoryId)
		{
			List<ProductsVars> products = new List<ProductsVars>();
			using (var dbContext = ContextManager.ClubContext())
			{
				products = dbContext.ProductsVars.AsNoTracking().Where(x => !x.Disabled && !x.DisabledToOrder)
					.Join(dbContext.CategoryVariants.Where(x => x.CategoryNumber == categoryId), p => p.FullBarCode, pv => pv.Barcode, (p, pv) => p).ToList();

			}
			return products;
		}

		public List<ShopingBasket> GetCart(string memberId)
		{

			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ShopingBasket.AsNoTracking().Where(cart => cart.MemberId.Equals(memberId) && cart.Expired > DateTime.Now).ToList();
			}

		}

		public List<ShopingBasket> UpdateShoppingCartPricesGetCart(string memberId, int creditCardType)
		{

			using (var dbContext = ContextManager.ClubContext())
			{


				var basket = dbContext.ShopingBasket.Where(cart => cart.MemberId.Equals(memberId) && cart.Expired > DateTime.Now).ToList();
				var tmp = basket.FirstOrDefault().FinalPrice;
				basket.FirstOrDefault().FinalPrice = tmp * 2;
				dbContext.SaveChanges();
				basket.FirstOrDefault().FinalPrice = tmp;
				dbContext.SaveChanges();


				var orderCategories = basket.Select(y => y.CategoryNumber).Select(long.Parse);
				var productVars = dbContext.CategoryVariants.AsNoTracking().Where(x => orderCategories.Contains(x.CategoryNumber))
								   .Join(dbContext.ProductsVars.AsNoTracking().Where(y => !y.DisabledToOrder)
										 , cv => cv.Barcode, pv => pv.FullBarCode, (cv, pv) => pv)
								   .ToList();

				productVars = productVars.Where(y => ((y.LastImplementationDate.HasValue && !ProductFunctions.CheckVariantExpired(y)) || !y.LastImplementationDate.HasValue)).ToList();


				var specsByVarsList =
					dbContext.BusinessSubTypeSpecificationByVariants.AsNoTracking().Where(x => basket.Select(y => y.ProductBarcode).Contains(x.BarCode)).ToList();
				var businessSubTypeList = productVars.Select(x => (int)x.BusinessSubTypeId.GetValueOrDefault()).Distinct().ToList();
				var specsCurrentList = dbContext.BusinessSubTypeSpecificationCurrent.AsNoTracking().Where(x => businessSubTypeList.Contains(x.BusinessSubTypeId)).ToList();


				foreach (var item in basket)
				{
					var product = productVars.FirstOrDefault(x => x.FullBarCode == item.ProductBarcode);
					int basePrice = Convert.ToInt32(Repositories.Helpers.ProductFunctions.GetVariantPrice(ContextManager.CurrentUser().PremiumType, product,
										specsByVarsList.FirstOrDefault(y => y.BarCode == item.ProductBarcode),
										specsCurrentList.FirstOrDefault(y => y.BusinessSubTypeId == product.BusinessSubTypeId),
									creditCardType == 1, true));

					item.FinalPrice = basePrice * item.Quantity;

				}

				dbContext.SaveChanges();
				return basket;
			}

		}

		public void SyncShoppingBasket(string memberId, List<long> productsToRemove)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{

					var removeList = context.ShopingBasket.Where(s => s.MemberId == memberId && (productsToRemove.Contains(s.Id) || s.Expired < DateTime.Now)).ToList();
					context.ShopingBasket.RemoveRange(removeList);
					context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, "Error on SyncShopingBasket");
			}

		}

		public ShopingBasket GetProductByBarcode(string productBarCode)
		{

			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ShopingBasket.AsNoTracking().FirstOrDefault(p => p.ProductBarcode.Equals(productBarCode) && p.MemberId.Equals(ContextManager.CurrentUser().Id));
			}

		}

		public List<ShopingBasket> GetProductsByCategoryNumber(string categoryNumber)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ShopingBasket.AsNoTracking().Where(p => p.MemberId.Equals(ContextManager.CurrentUser().Id) && p.CategoryNumber.Equals(categoryNumber)).ToList();
			}

		}
		public List<ShopingBasket> GetAllProductsByMemberId(string MemberId)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ShopingBasket.AsNoTracking().Where(p => p.MemberId.Equals(MemberId)).ToList();
			}
		}
		public List<ShopingBasket> GetAllProducts()
		{

			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ShopingBasket.AsNoTracking().Where(p => p.MemberId.Equals(ContextManager.CurrentUser().Id)).ToList();
			}

		}

		public async Task<bool> UpdateProduct(ShopingBasket productToUpdate)
		{
			var productFromDB = GetProductByBarcode(productToUpdate.ProductBarcode);
			using (var context = ContextManager.ClubContext())
			{
				if (productToUpdate != null)
				{
					try
					{
						context.Attach(productFromDB);
						productFromDB.Quantity = productToUpdate.Quantity;
						productFromDB.FinalPrice = productToUpdate.FinalPrice;
						productFromDB.CreateDate = productToUpdate.CreateDate;
						productFromDB.Expired = productToUpdate.Expired;
						await context.SaveChangesAsync();
						return true;
					}
					catch (Exception e)
					{
						throw e;
					}
				}
				return false;
			}
		}

		public string getFormattedDate(DateTime? date)
		{
			if (date == null)
			{
				return "0001-01-01 00:00:00";
			}
			var split = date.ToString().Split(" ");
			var splitTwo = split[0].Split("/");
			var newDate = splitTwo[2] + "-" + splitTwo[1] + "-" + splitTwo[0] + " " + split[1];
			return newDate;
		}

		public bool AddProduct(ShopingBasket productToAdd)
		{

			try
			{

				using (var context = ContextManager.ClubContext())
				{// Year month day time - 2020-03-13 13:23:43.37
					var seatStatus = productToAdd.SeatsStatus == null ? 0 : productToAdd.SeatsStatus;
					var xmlParams = string.IsNullOrEmpty(productToAdd.XmlParams) ? "" : productToAdd.XmlParams;

					// var newExpired = getFormattedDate(productToAdd.Expired);
					//var newCreateDate = getFormattedDate(productToAdd.CreateDate).Replace("/","-");

					context.ShopingBasket.Add(
						new ShopingBasket()
						{
							MemberId = productToAdd.MemberId,
							CategoryNumber = productToAdd.CategoryNumber,
							ProductSubType = productToAdd.ProductSubType,
							Quantity = productToAdd.Quantity,
							XmlParams = xmlParams,
							FinalPrice = productToAdd.FinalPrice,
							SeatsStatus = seatStatus,
							ProductBarcode = productToAdd.ProductBarcode,
							Expired = productToAdd.Expired,
							ProductJsonForGA = productToAdd.ProductJsonForGA,
							CreateDate = DateTime.Now,
						}
						);
					context.SaveChanges();
					return true;

				}
			}
			catch (Exception e)
			{
				throw e;
			}


		}

		public bool RemoveProduct(string productBarcode)
		{
			using (var context = ContextManager.ClubContext())
			{
				if (productBarcode != null)
				{
					try
					{
						var item = GetProductByBarcode(productBarcode);
						if (item != null)
						{
							context.ShopingBasket.Remove(item);
							context.SaveChanges();
						}
						return true;
					}
					catch (Exception e)
					{
						throw e;
					}
				}
				return false;
			}
		}

		public bool RemoveProductsByCategoryNumber(string categoryNumber)
		{
			using (var context = ContextManager.ClubContext())
			{
				if (categoryNumber != null)
				{
					try
					{
						var products = GetProductsByCategoryNumber(categoryNumber);
						context.ShopingBasket.RemoveRange(products);
						context.SaveChanges();
						return true;
					}
					catch (Exception e)
					{
						throw e;
					}
				}
				return false;
			}
		}
		public bool RemoveAllProductsByMemberId(string MemberId)
		{
			using (var context = ContextManager.ClubContext())
			{

				try
				{
					var products = GetAllProductsByMemberId(MemberId);
					context.ShopingBasket.RemoveRange(products);
					context.SaveChanges();
					return true;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
		public bool RemoveAllProducts()
		{
			using (var context = ContextManager.ClubContext())
			{

				try
				{
					var products = GetAllProducts();

					string NewDigitalCardVariant = _configuration.GetConfigByValue<string>(ConfigurationKey.NewDigitalCardVariant).ToString();
					if (string.IsNullOrEmpty(NewDigitalCardVariant))
					{
						NewDigitalCardVariant = "1002486-4";
					}
					// if the purchase contains a zero priced item 
					// remove every item that is zero priced only.
					if (products.Any(p => p.FinalPrice == 0 && p.ProductBarcode == NewDigitalCardVariant))
					{
						products = products.Where(p => p.FinalPrice == 0 && p.ProductBarcode == NewDigitalCardVariant).ToList();
					}
					context.ShopingBasket.RemoveRange(products);
					context.SaveChanges();
					return true;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}

		#endregion

		#region Cards

		public long AddRequestRow(int status, string cardNumber, string cardNumber2, string remark, int type, long originalRequestId, decimal amount = -1, int walletID = 0)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				Common.EF.Club.Requests requests = new Common.EF.Club.Requests
				{
					RequestTime = DateTime.Now,
					RequestSource = 5,
					RequestOp = 0,
					RequestType = type,
					RequestStatus = status,
					ReasonCode = 0,
					Id1 = ContextManager.CurrentUser().Id,
					Amount = amount == -1 ? default(decimal?) : amount,
					Remark = remark,
					Card1 = cardNumber,
					Card2 = cardNumber2,
					OriginalRequestId = originalRequestId,
					WalletId = walletID
				};

				dbContext.Requests.Add(requests);
				dbContext.SaveChanges();
				return requests.RequestId;
			}
		}

		public void UpdateRequestRow(int status, string cardNumber, string cardNumber2, decimal amount, string remark, long originalRequestId, long requestId, string xmlParam = "", int walletId = 0, long paymentId = 0)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				Common.EF.Club.Requests req = dbContext.Requests.Where(x => x.RequestId == requestId).SingleOrDefault();
				req.Xmlparam = xmlParam;
				req.RequestTime = DateTime.Now;
				req.RequestSource = 5;
				req.RequestOp = 0;
				req.RequestStatus = status;
				req.ReasonCode = 0;
				req.Id1 = ContextManager.CurrentUser().Id;
				req.Remark = remark;
				req.Card1 = cardNumber;
				req.Card2 = cardNumber2;
				req.Amount = Math.Ceiling(amount * 100) / 100;
				req.PaymentID = paymentId;
				req.OriginalRequestId = originalRequestId;
				req.WalletId = walletId;

				dbContext.SaveChanges();
			}
		}

		public void InsertPaymentRow(decimal amount, string cardNumber, string serverTransactionId)
		{
			ResponseUserDTO currentUser = ContextManager.CurrentUser();

			using (var dbContext = ContextManager.ClubContext())
			{
				Nofshonit.Common.EF.Club.Payments payments = new Nofshonit.Common.EF.Club.Payments()
				{
					CardOwnerId = currentUser.IdentityNumber,
					Charged = amount,
					MemberId = currentUser.Id,
					Last4Digits = cardNumber.Substring(cardNumber.Length - 4),
					ConfirmationNumber = serverTransactionId,
					TimeStamp = DateTime.Now
				};

				dbContext.Payments.Add(payments);
				dbContext.SaveChanges();
			}
		}

		public long InsertPaymentRow(decimal amount, string cardNumber, string last4Digit, string serverTransactionId, string currentUserId, string currentUserIdentityNumber, string xml)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				Nofshonit.Common.EF.Club.Payments payments = new Nofshonit.Common.EF.Club.Payments()
				{
					CardOwnerId = currentUserIdentityNumber,
					Charged = amount,
					MemberId = currentUserId,
					Last4Digits = last4Digit,
					ConfirmationNumber = serverTransactionId,
					TimeStamp = DateTime.Now,
					Xml = xml
				};

				dbContext.Payments.Add(payments);
				dbContext.SaveChanges();
				return payments.PaymentId;
			}
		}

		public long InsertPayment(string memberId, decimal amount, string xml, string cardOwnerId, string last4digits, string serverTransactionId, byte creditCardStatus = 0)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				Nofshonit.Common.EF.Club.Payments payments = new Nofshonit.Common.EF.Club.Payments()
				{
					CardOwnerId = cardOwnerId,
					Charged = amount,
					Last4Digits = last4digits,
					MemberId = memberId,
					ConfirmationNumber = serverTransactionId,
					OperatorCommission = 0,
					OrganizationCommission = 0,
					PointsCashValue = 0,
					PointsChargedStatus = 0,
					PointsUsed = 0,
					ProductsOrderStatus = 0,
					TimeStamp = DateTime.Now,
					TrustProgramCommission = 0,
					UniquId = "",
					Xml = xml,
					CreditCardStatus = creditCardStatus
				};

				dbContext.Payments.Add(payments);
				dbContext.SaveChanges();
				return payments.PaymentId;
			}
		}

		public void UpdateOrAddToAllMembersProperties(ExtandPeyerDataDTO epd, string cardId, string memberId)
		{
			using (var context = ContextManager.ClubContext())
			{
				var allMemberProp = context.AllMembersProperties.FirstOrDefault(a => a.MemberId == memberId);
				if (allMemberProp != null)
					context.AllMembersProperties.Remove(allMemberProp);

				context.AllMembersProperties.Add(new AllMembersProperties()
				{
					CardExpiration = epd.PayerData.PayerCardExpiresMonth + epd.PayerData.PayerCardExpiresYear,
					CardId = cardId,
					CardNum = cardId.Substring(cardId.Length - 4, 4),
					CreationDate = DateTime.Now,
					MemberId = memberId,
					PayerTz = epd.PayerData.PayerCardTZ,
					CreditCardClub = epd.MaxClubId,
					//CardCode = epd.PayerData.PayerCardCVV.ToString()

				});
				context.SaveChanges();
				ContextManager.SetCurrentUserCache(memberId);
			}
		}
		public bool IsCancelRequriedAproove(string barCode)
		{
			if (string.IsNullOrEmpty(barCode)) return false;
			using (var clubContext = ContextManager.ClubContext())
			using (var dtsContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
			{
				var userAllowedCancel = clubContext.AppConfig.FirstOrDefault(f => f.Key == ConfigurationKey.AllowCancelExternalCoupon);

				if (userAllowedCancel == null || (userAllowedCancel != null && userAllowedCancel.Value.Trim() != "1"))
				{
					return false;
				}

				var product = clubContext.ProductsVars.FirstOrDefault(f => f.FullBarCode == barCode);

				if (!product.CuponStockId.HasValue || product.CuponStockId.Value <= 0) return false;
				var res = dtsContext.CouponsStocksDetails.FirstOrDefault(f => f.StockId == product.CuponStockId);
				return (res != null && res.StockType == 0);
			}

		}

		public LoadWalletToFuncDto LoadWallet(string cardNumber, string walletId)
		{
			try
			{
				var DTSOnlineContext = string.Format(_configuration.GetConnectionStringByValue<string>("DTSOnlineContext"), ContextManager.CurrentOrganization().DBName);
				//using (var context = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				using (var connection = new SqlConnection(DTSOnlineContext))
				//using (var connection = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				{
					//List<(long categoryNumber, string name, int sortIndex)> cats = new List<(long categoryNumber, string name, int sortIndex)>();
					LoadWalletToFuncDto cats = new LoadWalletToFuncDto();
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "sp_GetWalletDataDto";
					command.Parameters.AddWithValue("cardNumber", cardNumber);
					command.Parameters.AddWithValue("walletId", walletId);
					command.Parameters.AddWithValue("dbName", ContextManager.CurrentOrganization().DBName);
					command.Parameters.AddWithValue("orgId", ContextManager.CurrentOrganization().OrgId);
					SqlDataAdapter da = new SqlDataAdapter(command);
					DataTable dt = new DataTable();
					da.Fill(dt);
					foreach (DataRow r in dt.Rows)
						cats =
							new LoadWalletToFuncDto()
							{
								CardsPrefix = r["CardsPrefix"].ToString(),
								Cvv = r["cvv"].ToString(),
								DBName = r["DBName"].ToString(),
								DiscountRate = r["DiscountRate"].ToString().Length == 0 ? 0 : double.Parse(r["DiscountRate"].ToString()),
								IDMember = r["IDMember"].ToString(),
								IsLeverage = r["IsLeverage"].ToString() == "1" ? true : false,
								LoadMoneyTerminalNumber = r["LoadMoneyTerminalNumber"].ToString(),
								MaxInMonth = r["MaxInMonth"].ToString().Length == 0 ? 0 : double.Parse(r["MaxInMonth"].ToString()),
								MaxSumInCard = r["MaxSumInCard"].ToString(),
								MaxSumInCardForMonth = r["MaxSumInCardForMonth"].ToString(),
								OrganizationAllowedCardNumberByTZ = r["OrganizationAllowedCardNumberByTZ"].ToString(),
								OrganizationID = r["OrganizationID"].ToString(),
								OrganizationName = r["OrganizationName"].ToString(),
								PaymentTerminalPassword = r["PaymentTerminalPassword"].ToString(),
								PaymentTerminalUserName = r["PaymentTerminalUserName"].ToString(),
								SerieID = r["SeriesID"].ToString(),
								SeriesName = r["SerieName"].ToString(),
								TradeSitePaymentTerminalNumber = r["TradeSitePaymentTerminalNumber"].ToString(),
								WalletLoadMoneyId = r["ID"].ToString(),
								LoadedThisMonth = r["LoadedThisMonth"].ToString(),
								CardExpiration = r["CardExpiration"].ToString(),
								CardID = r["CardID"].ToString(),
								CardNum = r["CardNum"].ToString(),
								PayerTZ = r["PayerTZ"].ToString(),
							};

					return cats;
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error on LoadWallet sp_GetWalletDataDto, cardNumber: {cardNumber}," +
					$" Message: {ex.Message}, InnerException: {ex.InnerException}, StackTrace: {ex.StackTrace}");
				throw ex;
			}
		}
		public List<WalletData> GetWallets(string xmlString, string cardNumber, string memberId)
		{
			try
			{



				var dtsConnectionString = string.Format(_configuration.GetConnectionStringByValue<string>("DTSOnlineContext"), ContextManager.CurrentOrganization().DBName);
				//using (var context = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				using (var connection = new System.Data.SqlClient.SqlConnection(dtsConnectionString))
				//using (var connection = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				{
					//List<(long categoryNumber, string name, int sortIndex)> cats = new List<(long categoryNumber, string name, int sortIndex)>();
					List<WalletData> cats = new List<WalletData>();
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "sp_GetWalletsData";
					command.Parameters.AddWithValue("xmlString", xmlString);
					command.Parameters.AddWithValue("cardNumber", cardNumber);
					command.Parameters.AddWithValue("memberId", memberId);
					command.Parameters.AddWithValue("dbName", ContextManager.CurrentOrganization().DBName);
					command.Parameters.AddWithValue("orgId", ContextManager.CurrentOrganization().OrgId);
					System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(command);
					DataTable dt = new DataTable();
					da.Fill(dt);
					foreach (DataRow r in dt.Rows)
					{
						WalletData walletdata =
							new WalletData()
							{
								Balance = Double.Parse(r["Balance"].ToString()),
								DiscountMode = r["DiscountMode"].ToString(),
								DiscountRate = decimal.Parse(r["DiscountRate"].ToString()),
								EmployeeNum = r["EmployeeNum"].ToString(),
								IsLoadAllowed = int.Parse(r["IsLoadAllowed"].ToString()),
								IsLoadMoney = r["IsLoadMoney"].ToString(),
								Last4Digits = r["Last4Digits"].ToString(),
								LoadedThisMonth = r["LoadedThisMonth"].ToString(),
								MaxAmountToLoad = double.Parse(r["MaxAmountToLoad"].ToString()),
								MaxBalance = r["MaxBalance"].ToString(),
								MaxDeposit = r["MaxDeposit"].ToString(),
								MemberID = r["MemberID"].ToString(),
								Name = r["Name"].ToString(),
								OrganizationID = r["OrganizationID"].ToString(),
								QuickLoadMode = int.Parse(r["QuickLoadMode"].ToString()),
								TZ = r["TZ"].ToString(),
								WalletID = r["WalletID"].ToString()
							};

						try
						{
							using (var dbContext = ContextManager.ClubContext())
							{
								var wallet = dbContext.WalletLoadMoney.Where(wlm => wlm.WalletId.ToString() == walletdata.WalletID).SingleOrDefault();
								if (wallet != null)
								{
									walletdata.BackgroundImageName = wallet.BackgroundImageName;
								}
							}
						}
						catch (Exception ex)
						{
							LoggerHelper.Error($"feilad get BackgroundImageName of wallet data in GetWallets, walletid: {walletdata.WalletID}, message: {ex.Message}, StackTrace: {ex.StackTrace}");
						}

						cats.Add(walletdata);
					}
					return cats;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		public void UpdateRequestLoadWallet(long reqId, long? paymentId, int status, string exception)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var request = dbContext.Requests.Where(r => r.RequestId == reqId).SingleOrDefault();
				if (request != null)
				{
					request.RequestStatus = status;
					request.PaymentID = paymentId;
					request.Remark = exception.Length > 0 ? exception : null;
				}
				dbContext.SaveChanges();
			}
		}
		public short GetCardCVV(string cardNumber)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				Common.EF.Club.CardsSkeleton cardsSkeleton = dbContext.CardsSkeleton.FirstOrDefault(x => x.CardNumber == cardNumber);
				return cardsSkeleton?.Cvv ?? 0;
			}
		}

		public async Task<long> AddVerifoneLog(decimal Amount, string Card, int wallet, string Type)
		{
			using (var context = ContextManager.ClubContext())
			{
				var log = new VerifonTransactionLog
				{
					VerifonTransactionDateTime = DateTime.Now,
					CardId = Card,
					AmountReq = decimal.Round(Amount, 2),
					WalletReq = wallet,
					ReqType = Type
				};
				context.VerifonTransactionLog.Add(log);
				await context.SaveChangesAsync();
				return log.TransactionId;
			}
		}

		public async void SavePinCode(string pinCode, ResponseUserDTO currentUser)
		{
			using (var context = ContextManager.ClubContext())
			{
				var userFromDb = await context.AllMembers.FirstOrDefaultAsync(u => u.MemberId.Equals(currentUser.Id));
				if (userFromDb != null)
				{
					context.Attach(userFromDb);
					userFromDb.PinCode = pinCode;
					await context.SaveChangesAsync();
					try
					{
						ContextManager.SetCurrentUserCache(currentUser.IdentityNumber);
					}
					catch (Exception ex) { }
				}
			}
		}
		public async void SetMaxCardClub(string clubId, ResponseUserDTO currentUser)
		{
			using (var context = ContextManager.ClubContext())
			{

				var allMemberProp = await context.AllMembersProperties.FirstOrDefaultAsync(x => x.MemberId.Equals(currentUser.Id));
				if (allMemberProp != null)
				{
					allMemberProp.CreditCardClub = clubId;
					await context.SaveChangesAsync();
					ContextManager.SetCurrentUserCache(currentUser.Id);

				}
			}
		}



		public int GetMaxDepositForMonth(string walletID)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.WalletLoadMoney.AsNoTracking().FirstOrDefault(w => w.WalletId == long.Parse(walletID))?.MaxSumInWalletForMonth ?? 0;
			}
		}

		public int GetAmountToWallet(string walletId)//TODO missing functionallity
		{
			using (var dbContext = ContextManager.ClubContext())
			{

				return 1;
			}
		}

		public int CheckNewCardRequestIsPossible(string memberID)
		{
			var result = -1;
			string newCardBarcode = _configuration.GetConfigByValue<string>(ConfigurationKey.NewCardVariant);

			using (var dbContext = ContextManager.ClubContext())
			{
				bool memberActiveCards = dbContext.Cards.Any(c => c.Idmember == memberID && c.CardType == 3 && (c.CardStatus == 0 || c.CardStatus == 1));
				bool memberNewCardRequest = dbContext.Hanpaka.Any(c => c.MemberId == memberID && c.CskeletonId == null);
				bool variantExist = dbContext.ShopingBasket.Any(c => c.ProductBarcode == newCardBarcode && c.MemberId == memberID);
				bool variantExistsInAtractionOrders = dbContext.Atractionsorders.Any(a => a.MemberId == memberID && a.BarCode.Equals(newCardBarcode) && a.MemberOrderBlance == 0 && a.MemberOrderQuntity > 0);
				if (!memberActiveCards && !memberNewCardRequest && !variantExist && !variantExistsInAtractionOrders)
					result = 0;
				else if (memberActiveCards && !memberNewCardRequest)
					result = 1;
				else if (!memberActiveCards && memberNewCardRequest)
					result = 2;
				else if (variantExist || variantExistsInAtractionOrders)
					result = 3;

			}

			return result;
		}

		public bool BlockCard(string memberID)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					List<Common.EF.Club.Cards> allActiveCards = dbContext.Cards.Where(c => c.Idmember == memberID && c.CardType == 3 && (c.CardStatus == 0 || c.CardStatus == 1)).ToList();
					if (allActiveCards.Count == 0)
						throw new BusinessException("אין כרטיס פעיל למשתמש");
					allActiveCards.ForEach(card =>
					{
						card.CardStatus = 2;
						card.BlockTime = DateTime.Now;
						Common.EF.Club.Requests requests = new Common.EF.Club.Requests
						{
							RequestTime = DateTime.Now,
							RequestSource = 5,
							RequestOp = 0,
							RequestType = 31,
							RequestStatus = 1,
							Id1 = memberID,
							Card1 = card.CardNumber
						};
						dbContext.Requests.Add(requests);
					});

					dbContext.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message);
			}
			return true;
		}

		public bool ActiveateCard(string memberID)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					List<Common.EF.Club.Cards> allActiveCards = dbContext.Cards.Where(c => c.Idmember == memberID && c.CardType == 3 && c.CardStatus == 2).ToList();
					if (allActiveCards.Count == 0)
						throw new Exception("No avaiable cards found");
					allActiveCards.ForEach(card =>
					{
						card.CardStatus = 1;
					});

					dbContext.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return true;
		}

		public bool IsQuickLoadAllowdToSavedCC()
		{
			using (var context = ContextManager.ClubContext())
			{
				var userProp = context.AllMembersProperties.FirstOrDefault(u => u.MemberId.Equals(ContextManager.CurrentUser().Id));
				if (userProp == null)
					return false;
				bool isAllowed = userProp?.PayerTz?.GetTrim().PadLeft(9, '0').Equals(userProp?.MemberId?.GetTrim()) ?? false;
				return isAllowed;
			}
		}

		public CardInfoDTO GetCardActivationAndExpiryDate(string cardNumber)
		{
			CardInfoDTO cardInfo = null;
			using (var dbContext = ContextManager.ClubContext())
			{
				Common.EF.Club.Cards card = dbContext.Cards.FirstOrDefault(x => x.CardNumber == cardNumber);

				Common.EF.Club.CardsSkeleton cardsSkeleton = dbContext.CardsSkeleton.FirstOrDefault(x => x.CardNumber == cardNumber);
				if (cardsSkeleton == null)
					return null;
				return cardInfo = new CardInfoDTO
				{
					ActivationTime = card.ActivationTime,
					ExpiredDate = dbContext.MwcHanpakaRequests.FirstOrDefault(f => f.Hrid == cardsSkeleton.Hrid)?.ExpiredDate,
					CardStatus = card.CardStatus
				};

			}
		}

		public bool IsCardVariant(long categoryId)
		{
			string NewDigitalCardVariant = _configuration.GetConfigByValue<string>(ConfigurationKey.NewDigitalCardVariant).ToString();
			string NewCardVariant = _configuration.GetConfigByValue<string>(ConfigurationKey.NewCardVariant).ToString();
			using (var context = ContextManager.ClubContext())
			{
				var barcode = context.CategoryVariants.FirstOrDefault(x => x.CategoryNumber == categoryId).Barcode;
				return (barcode == NewDigitalCardVariant || barcode == NewCardVariant);
			}
		}

		public string GetOldCardByIdentity(string memberId)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					var oldCard = dbContext.Cards.Where(x => x.Idmember == memberId && x.CardStatus == 2 && x.CardType == 3).OrderByDescending(y => y.BlockTime).FirstOrDefault();

					if (oldCard == null)
						return "";
					else
						return oldCard.CardNumber;
				}
			}
			catch (Exception ex)
			{
				return "";
			}
		}

		public string GetActiveCardByIdentity(string memberId)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					var activeCard = dbContext.Cards.
										Where(x => x.Idmember == memberId && x.CardStatus == (int)ECardStatus.ACTIVE && x.CardType == (int)ECardType.VERIFONE).
										OrderByDescending(y => y.ActivationTime).
										FirstOrDefault();
					if (activeCard == null)
					{
						return null;
					}
					if (string.IsNullOrEmpty(activeCard.CardNumber))
						return "";
					else
						return activeCard.CardNumber;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}


		public CardInfoVerifone GetCardInfoForVerifoneActions(string cardNumber)
		{
			try
			{
				var cardInfoVerifone = new CardInfoVerifone();
				using (var dbContext = ContextManager.ClubContext())
				{
					var cardSkel = dbContext.CardsSkeleton.FirstOrDefault(x => x.CardNumber == cardNumber);
					cardInfoVerifone.CardNumber = cardSkel.CardNumber;
					cardInfoVerifone.SerieID = cardSkel.SerieId;
					cardInfoVerifone.CVV = cardSkel.Cvv;
					cardInfoVerifone.SequentialNum = cardSkel.SequentialNum;
				}
				using (var context = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
				{
					var serie = context.MwcSeries.FirstOrDefault(x => x.SerieId == cardInfoVerifone.SerieID);
					cardInfoVerifone.VerID = serie.VerId;
				}
				return cardInfoVerifone;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public decimal Get14DaysBalance(string cardNumber, int walletId)
		{
			int numOfDay = 365;
			int.TryParse(ContextManager.GetAppConfig().FirstOrDefault(x => x.Key == "Money_NumberOfDaysAllowingCancellat").Value, out numOfDay);
			List<int> activityTypes = new List<int>() { 1000, 2000, 3002 };

			using (var dbContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
			{


				var totalDepositToPeriod = dbContext.MwcMedia.Where(y => y.CardNumber == cardNumber
				&& y.WalletId == walletId
				&& y.TransactionDateTime > DateTime.Now.AddDays(-numOfDay)
				&& activityTypes.Contains(y.ActivityId.GetValueOrDefault())).Sum(s => s.Amount) ?? 0;



				var balabce = dbContext.MwcMedia.Where(y => y.CardNumber == cardNumber
				&& y.WalletId == walletId)
				.Sum(x => x.Amount) ?? 0;

				//If it is negative, it means that there is a charge cancellation from a charge a year ago
				if (totalDepositToPeriod < 0)
					return (decimal)(balabce / 100.0);

				return (decimal)Math.Min((totalDepositToPeriod / 100.0), (balabce / 100.0));

			}
		}

		public (string type, decimal discountRate) GetWalletRefundInfo(int walletId)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var activtypeeCard = dbContext.WalletLoadMoney.FirstOrDefault(w => w.WalletId == walletId);
				if (activtypeeCard.IsLoadMoney == false)
					throw new BusinessException("Load not allowed");
				if (activtypeeCard != null && activtypeeCard.IsLeverageCreadit.HasValue)
				{
					string t = activtypeeCard.IsLeverageCreadit.Value ? "L" : "D";
					decimal d = activtypeeCard.WalletPresentCredit;

					(string type, decimal discountRate) result = (t, d);
					return result;
				}
				return ("Err", -1);
			}
		}

		public decimal GetGlobalSelfDischargeLimit()
		{
			return _dtsOnlineRepo.GetGlobalSelfDischargeLimit();
		}

        public async Task<decimal> GetMemberMonthlySelfDischargeTotal(string cardNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cardNumber)) return 0m;
                var now = DateTime.Now;
                var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
                using (var context = ContextManager.ClubContext())
                {
                    decimal? total = await context.Requests
                        .AsNoTracking()
                        .Where(x => x.Card1.Trim() == cardNumber.Trim()
                                 && x.RequestType == 210
                                 && x.RequestStatus == 1        // ← קריטי
                                 && x.RequestSource == 5
                                 && x.RequestTime >= monthStart
                                 && x.RequestTime <= now)
                        .SumAsync(x => x.Amount);
                    return total ?? 0m;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Error in GetMemberMonthlySelfDischargeTotal, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return 0m;
            }
        }

        #endregion

        #region Authentication
        public (bool, string) ValidateTokenForMember(string memberId, string token, DateTime expiredDate)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				try
				{
					var member = dbContext.AllMembers.FirstOrDefault(m => m.MemberId.Equals(memberId.Trim()));
					if (member == null)
						return (false, "Member not found");
					if (!member.UserToken.Equals(token))
						return (false, "Token is not match");
					if (expiredDate < DateTime.Now)
						return (false, "Token is expired, Please do relogin");
				}
				catch (Exception)
				{
					return (false, "Token validaion failed");
				}
				return (true, "Token is valid");

			}
		}


		public async Task<bool> UpdateLastUpdate(string memberId, DateTime? newDate, int? premiumType = null)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var memberFromDB = await dbContext.AllMembers.FirstOrDefaultAsync(m => m.MemberId.Equals(memberId.Trim()));
				if (memberFromDB != null)
				{
					dbContext.Attach(memberFromDB);
					memberFromDB.LastUpdate = newDate;
					if (premiumType != null)
						memberFromDB.PremiumType = premiumType;
					await dbContext.SaveChangesAsync();
					return true;
				}
				return false;
			}
		}
		public async Task<bool> UpdateLastUpdate(string memberId, DateTime? newDate, int? premiumType = null, int? darga = null)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var memberFromDB = await dbContext.AllMembers.FirstOrDefaultAsync(m => m.MemberId.Equals(memberId.Trim()));
				if (memberFromDB != null)
				{
					dbContext.Attach(memberFromDB);
					memberFromDB.LastUpdate = newDate;
					if (premiumType.HasValue)
						memberFromDB.PremiumType = premiumType;
					if (darga.HasValue)
						memberFromDB.Darga = darga;
					await dbContext.SaveChangesAsync();
					return true;
				}
				return false;
			}
		}

		#endregion

		#region Search
		public List<(long categoryNumber, string categoryName, int sortIndex)> GetSearchData(string str, int selectTop, long category, string script)
		{
			try
			{
				using (var connection = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				{
					List<(long categoryNumber, string name, int sortIndex)> cats = new List<(long categoryNumber, string name, int sortIndex)>();
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = script;
					command.Parameters.AddWithValue(PROC_TEXT_PARAM, str);
					command.Parameters.AddWithValue(PROC_SELECT_TOP_PARAM, selectTop);
					if (category >= 0)
						command.Parameters.AddWithValue(PROC_SUPER_CATEGORY_PARAM, category);
					command.Parameters.AddWithValue(PROC_ORGID_PARAM, ContextManager.CurrentOrganization().OrgId);
					SqlDataAdapter da = new SqlDataAdapter(command);
					DataTable dt = new DataTable();
					da.Fill(dt);
					foreach (DataRow r in dt.Rows)
						cats.Add(((long)r.ItemArray[0], (string)r.ItemArray[1], (int)r.ItemArray[2]));
					return cats;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public List<(long categoryNumber, string categoryName, int sortIndex)> GetSearchAutoComplete(string str, int selectTop)
		{
			return GetSearchData(str, selectTop, -1, AUTO_COMPLETE_SCRIPT);
		}

		public List<(long categoryNumber, string categoryName, int sortIndex)> FullTextSearchLogic(string search, string strForContains, string strForUnion)
		{
			try
			{
				using (var connection = (SqlConnection)ContextManager.ClubContext().Database.GetDbConnection())
				{
					List<(long categoryNumber, string name, int sortIndex)> cats = new List<(long categoryNumber, string name, int sortIndex)>();
					connection.Open();
					var command = connection.CreateCommand();
					command.CommandType = CommandType.StoredProcedure;
					command.CommandText = "FullTextSearchLogic";
					command.Parameters.AddWithValue("SearchText", search);
					command.Parameters.AddWithValue("strForContains", strForContains);
					command.Parameters.AddWithValue("strForUnion", strForUnion);
					SqlDataAdapter da = new SqlDataAdapter(command);
					DataTable dt = new DataTable();
					da.Fill(dt);
					foreach (DataRow r in dt.Rows)
						cats.Add(((long)r.ItemArray[0], (string)r.ItemArray[1], (int)r.ItemArray[2]));
					return cats;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Limitaions/Stock
		public GroupLimitationDTO GetGroupLimitations(long variantGroup)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.VariantGroupLimits.AsNoTracking().Where(p => p.Id == variantGroup).
					Select(s => new GroupLimitationDTO { MonthLimit = s.MonthLimit ?? 999, YearLimit = s.YearLimit ?? 999 }).FirstOrDefault();
			}
		}

		public ProductsVars GetProductsVar(string barcode)
		{
			return GetProductsVar(new List<string> { barcode }).FirstOrDefault();
		}
		public List<ProductsVars> GetProductsVar(List<string> barcode)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ProductsVars.AsNoTracking().Where(p => barcode.Contains(p.FullBarCode)).ToList();
			}
		}
		public List<ShopingBasket> GetShopingBasketByMemberId(string memberId, string barcode)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.ShopingBasket.AsNoTracking().Where(s => s.MemberId == memberId && s.ProductBarcode != barcode && s.Expired > DateTime.Now).ToList();
			}
		}
		public List<WebServiceTransactionDTO> GetMemberTransactions(string MemberId, bool enableBusinessSubTypeLimitations, bool isIgnoreBusinessSubType = false, bool isGetMemberCancelledTransactions = true)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					DateTime date = DateTime.Now.AddYears(-1);

					var memberTransactions = dbContext.WebServiceTransaction.
					Join(dbContext.ProductsVars, transcation => transcation.TtransactionProductId, product => product.FullBarCode,
					(transcation, product) => new { transcation, product })
						.Where(m =>
						(enableBusinessSubTypeLimitations ? m.product.IsIgnoreBusinessSubTypeLimits == isIgnoreBusinessSubType : true) &&
						m.transcation.TtransactionMemberId == MemberId &&
						m.transcation.TtransactionDateTime.HasValue &&
						m.transcation.TtransactionDateTime > date &&
						(isGetMemberCancelledTransactions || Convert.ToInt32(m.transcation.Ttransactionquantity) > 0))

						.Select(m => new WebServiceTransactionDTO()
						{
							Date = m.transcation.TtransactionDateTime ?? DateTime.Now,
							ID = m.transcation.TtransactionId,
							Quantity = m.transcation.Ttransactionquantity,
							BussinesSubTypeID = m.product.BusinessSubTypeId ?? 0,
							ProductId = m.transcation.TtransactionProductId,
							Iscampaign = m.transcation.Iscampaign,
							TransactionGroupLimitsId = m.transcation.GroupLimitsId,
							IsIgnoreBusinessSubTypeLimits = m.product.IsIgnoreBusinessSubTypeLimits

						}).ToList();

					return memberTransactions;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<WebServiceTransactionDTO> GetVariantTransactions(string barCode)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					DateTime date = DateTime.Now.AddYears(-1);

					var transactions = dbContext.WebServiceTransaction.
					Join(dbContext.ProductsVars, transcation => transcation.TtransactionProductId, product => product.FullBarCode,
					(transcation, product) => new { transcation, product })
						.Where(m => m.transcation.TtransactionDateTime.HasValue && m.transcation.TtransactionDateTime > date && m.transcation.TtransactionProductId.Equals(barCode))
						.Select(m => new WebServiceTransactionDTO()
						{
							Date = m.transcation.TtransactionDateTime ?? DateTime.Now,
							ID = m.transcation.TtransactionId,
							Quantity = m.transcation.Ttransactionquantity,
							BussinesSubTypeID = m.product.BusinessSubTypeId ?? 0,
							ProductId = m.transcation.TtransactionProductId
						}).ToList();

					return transactions;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static int? CalcMonthlyLimit(int? monthlyLimit, int? monthlyVaraintLimit)
		{
			int? result = null;

			if (monthlyLimit.HasValue && monthlyVaraintLimit.HasValue)
			{
				if (monthlyLimit.Value > monthlyVaraintLimit.Value)
					result = monthlyVaraintLimit.Value;
				else
					result = monthlyLimit.Value;
			}
			else if (monthlyLimit.HasValue && monthlyVaraintLimit.HasValue == false)
				result = monthlyLimit.Value;
			else if (monthlyLimit.HasValue == false && monthlyVaraintLimit.HasValue)
				result = monthlyVaraintLimit.Value;

			return result;
		}

		/// <summary>
		/// Return Business Limits
		/// </summary>
		/// <param name="bussTypeNames"></param>
		/// <param name="isVariantLimit">refer to MonthlyLimitValue OR MonthlyLimitVariantValue</param>
		/// <returns></returns>
		public List<BusinessSubTypeDTO> GetBussinessSubType(List<BusinessSubTypeNameDTO> bussTypeNames, bool isVariantLimit = false)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				List<int> bussTypeNamesIds = bussTypeNames.Select(r => r.Id).ToList();
				var subTypeLimit = dbContext.BusinessSubTypeSpecificationCurrent.AsNoTracking()
					.Where(r => bussTypeNamesIds.Contains(r.BusinessSubTypeId)).ToList()
					.Select(r => new BusinessSubTypeDTO()
					{
						Id = r.BusinessSubTypeId,
						Name = bussTypeNames.Any(b => b.Id == r.BusinessSubTypeId) ? bussTypeNames.First(b => b.Id == r.BusinessSubTypeId).Name : string.Empty,
						LimitWeekly = r.WeeklyLimitValue,
						LimitMontly = r.MonthlyLimitValue,
						LimitMonthlyVariant = r.MonthlyLimitVariantValue,
						LimitYearly = r.YearlyLimitValue,
					}).ToList();

				return subTypeLimit;
			}
		}

		public List<BusinessSubTypeSpecificationCurrent> GetAllBusinessSubTypes()
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var subTypes = dbContext.BusinessSubTypeSpecificationCurrent.AsNoTracking().ToList();
				return subTypes;
			}
		}
		public List<BusinessSubTypeDTO> GetBussinessSubTypeVariant(List<BusinessSubTypeNameDTO> bussTypeNames)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				List<int> bussTypeNamesIds = bussTypeNames.Select(r => r.Id).ToList();
				var subTypeLimit = dbContext.BusinessSubTypeSpecificationCurrent.AsNoTracking()
					.Where(r => bussTypeNamesIds.Contains(r.BusinessSubTypeId)).ToList()
					.Select(r => new BusinessSubTypeDTO()
					{
						Id = r.BusinessSubTypeId,
						Name = bussTypeNames.Any(b => b.Id == r.BusinessSubTypeId) ? bussTypeNames.First(b => b.Id == r.BusinessSubTypeId).Name : string.Empty,
						LimitWeekly = r.WeeklyLimitValue,
						LimitMontly = r.MonthlyLimitVariantValue,
						LimitYearly = r.YearlyLimitValue,
					}).ToList();

				return subTypeLimit;
			}
		}


		public int OrderQtyByBarcodes(List<string> barcodes)
		{
			var quantity = 0;
			using (var dbContext = ContextManager.ClubContext())
			{
				int? transactionsQuantity = dbContext.WebServiceTransaction.AsNoTracking().Where(x => barcodes.Contains(x.TtransactionProductId) && !string.IsNullOrEmpty(x.Ttransactionquantity))?.Select(x => x.Ttransactionquantity).AsEnumerable()?.Select(x => int.Parse(x))?.Sum();
				quantity = transactionsQuantity.GetValueOrDefault();
			}
			return quantity;
		}

		#endregion

		#region Order 


		public List<WebServiceTransactionDTO> GetOrderTransactionsByOrderGuid(string orderGuid)
		{

			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					var orderId = dbContext.Orders.First(x => x.ExternalGuid == orderGuid).OrderId;

					return dbContext.WebServiceTransaction.AsNoTracking().Where(x => x.OrderId.Value == orderId).Select(x => new WebServiceTransactionDTO
					{
						ID = x.TtransactionId,
						ProductId = x.TtransactionProductId,
						OrderAsmachta = x.TtransactionOrder.Value,
						PaymentId = x.PaymentId.HasValue ? x.PaymentId.Value : 0
					}).ToList();

				}

			}
			catch (Exception ex)
			{
				throw ex;
			}


		}

		public Nofshonit.Common.EF.Club.Orders GetOrderByGuid(string guid)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				return dbContext.Orders.FirstOrDefault(x => x.ExternalGuid == guid);
			}
		}

		public async Task<string> GetConfirmationPageByOrderGuid(string guid)
		{
			var wst = GetOrderTransactionsByOrderGuid(guid);
			var result = string.Empty;
			var emailType = ContextManager.CurrentOrganization().OrgId + 100000;

			if (wst.Any())
			{
				var paymentId = wst.First().PaymentId;

				using (var context = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
				{
					var emailQueueData = await context.EmailQueue.FirstOrDefaultAsync(x => x.PaymentId == paymentId && x.EmailType == emailType);
					if (emailQueueData != null)
						result = emailQueueData.EmailBody;
				}
			}

			return result;
		}

		public List<MemberCodes> GetSmsCodes(string memberId)
		{
			List<MemberCodes> smsCodes = new List<MemberCodes>();
			MemberCodes memberCode = ContextManager.GetObjectFromSession<MemberCodes>(SessionKeys.OTP_KEY(memberId));
			if (memberCode != null) smsCodes.Add(memberCode);
			return smsCodes;
		}

		public void SaveSmsCodeForLogin(string memberId, string smsCode)
		{
			var memberCodes = new MemberCodes
			{
				MemberId = memberId,
				Code = smsCode,
				InsertDate = DateTime.Now,
			};
			ContextManager.SetObjectInSession(SessionKeys.OTP_KEY(memberId), memberCodes);
		}

		public void DeleteSmsCodesByMemberId(string memberId)
		{
			ContextManager.ClearSessionByKey(SessionKeys.OTP_KEY(memberId));
		}

		#endregion

		#region wallet
		public async Task<List<WalletTagChainsData>> GetChainsByWallet(string walletId)
		{
			using (var context = new Common.EF.DTS_Online.DTS_OnlineContext())
			{
				var raw = await context.View_GetBusinessAndTags
					.Where(x => x.WalletID == Convert.ToInt32(walletId))
					.OrderBy(x => x.TagSort).ThenBy(x => x.BusinessTagSort)
					.AsNoTracking()
					.ToListAsync();

				return await BuildWalletTagChainDto(context, walletId, raw);
			}
		}
		#endregion

		#region branches

		public async Task<List<WalletChainBranches>> GetBranchesByWalletAndChain(string walletId, string chainId)
		{
			using (var context = new Common.EF.DTS_Online.DTS_OnlineContext())
			{
				List<WalletChainBranches> list = new List<WalletChainBranches>();
				var result = await context.View_GetBranchesByWalletIds.Where(x => x.WalletID == Convert.ToInt32(walletId) && x.ChainID == Convert.ToInt32(chainId)).OrderBy(x => x.BranchName).ToListAsync();
				foreach (var item in result)
				{
					list.Add(new WalletChainBranches
					{
						WalletID = item.WalletID,
						WalletName = item.WalletName,
						ChainID = item.ChainID,
						ChainName = item.ChainName,
						BranchId = item.BranchId,
						BranchName = item.BranchName,
						BuisnessID = item.BuisnessID,
						StoreAddress = item.StoreAddress,
						StorePhone1 = item.StorePhone1,
						WebSite = item.WebSite


					});
				}
				return list;
			}
		}

		#endregion

		#region Fox
		public async Task<string> GetDtsOrderGuidByFoxOrderGuid(string orderGuid)
		{

			Container.Resolve<Microsoft.AspNetCore.Http.IHttpContextAccessor>().HttpContext.Request.Headers["OrganizationGuid"] = "1F2FAA16-5829-4B65-B152-1FD9834AD7E3";
			using (var context = ContextManager.ClubContext())
			{
				var member = await context.AllMembers.FirstOrDefaultAsync(c => c.MemberFirstName == orderGuid);
				if (member == null) return string.Empty;
				var orderTransfer = context.OrderTransfer.FirstOrDefault(x => x.FromMemberId == member.MemberId);
				if (orderTransfer == null) return string.Empty;
				var order = await context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderTransfer.OrderId);
				if (order == null) return string.Empty;
				return order.ExternalGuid;

			}
		}


		#endregion
		public long AddCard(Common.EF.Club.Cards card)
		{
			using (var context = ContextManager.ClubContext())
			{
				var cardSaved = context.Cards.Add(card);
				context.SaveChanges();
				return 12;
			}
		}
		public Common.EF.Club.CardsSkeleton GetCardSkeleton(string serieId)
		{
			//orgDb.CardsSkeletons.OrderBy(c => c.CSkeletonID).FirstOrDefault(x => x.SerieID == SeriesId && string.IsNullOrEmpty(x.CardID.ToString()));
			using (var context = ContextManager.ClubContext())
			{
				return context.CardsSkeleton.OrderBy(c => c.CskeletonId).FirstOrDefault(x => x.SerieId.ToString() == serieId && string.IsNullOrEmpty(x.CardId.ToString()));
			}
		}
		public bool UpdateCardSkeleton(Common.EF.Club.CardsSkeleton cardSkeleton)
		{
			using (var context = ContextManager.ClubContext())
			{
				context.CardsSkeleton.Attach(cardSkeleton);
				context.SaveChanges();
				return true;
			}
		}
		public async Task<BaseResponse<bool>> UpdateBiometricToken(BiometicsInfoDTO biometicsInfoDTO)
		{
			var response = new BaseResponse<bool>();
			var userFromDb = await GetUserByUserID(ContextManager.CurrentUser().Id);
			if (userFromDb != null)
			{
				using (var context = ContextManager.ClubContext())
				{
					context.Attach(userFromDb);
					userFromDb.BiometricPublicKey = biometicsInfoDTO.publicKey;
					var value = await context.SaveChangesAsync();
					if (value > 0)
					{
						response.Data = true;
						return response;
					}
					throw new Exception("Failed save changes. (UpdateBiometricToken)");
				}
			}
			throw new Exception("Cannot find user. (UpdateBiometricToken)");
		}

		public string getPublicKeyById(AllMembers userFromDb)
		{
			if (userFromDb != null)
			{
				return userFromDb.BiometricPublicKey;
			}
			throw new Exception("Cannot find user. (UpdateUserToken)");
		}
		public string GetCardNumByMemberId(string memberId)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var allMember = dbContext.AllMembersProperties.FirstOrDefault(w => w.MemberId == memberId);
				if (allMember == null)
				{
					return "";
				}
				return allMember.CardNum;
			}
		}

		public DateTime? GetRegisrationDateByMember(string memberId)
		{
			using (var dbContext = ContextManager.ClubContext())
			{
				var allMember = dbContext.AllMembers.FirstOrDefault(w => w.MemberId == memberId);
				if (allMember == null)
				{
					return null;
				}
				return allMember.RegistrationDate;
			}
		}
		//public bool CheckAccountTransactions(string memberId, int years)
		//{

		//    using (var dbContext = ContextManager.ClubContext())
		//    {
		//        var allMwcMedia = dbContext.Payments.FirstOrDefault(y => y.MemberId == memberId
		//                             && y.TimeStamp > DateTime.Now.AddYears(years));



		//        return allMwcMedia == null;
		//    }

		//}


		public bool CheckAccountTransactions(string memberId, int years)
		{
			var cardNumber = GetActiveCardByIdentity(memberId);
			using (var dbContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
			{

				var allMwcMedia = dbContext.MwcMedia.FirstOrDefault(y => y.CardNumber == cardNumber
								  && y.TransactionDateTime > DateTime.Now.AddYears(years));
				using (var clubContext = ContextManager.ClubContext())
				{
					var allWebServiceTransaction = clubContext.WebServiceTransaction.FirstOrDefault(y => y.TtransactionMemberId == memberId
								  && y.TtransactionDateTime > DateTime.Now.AddYears(years));
					return allMwcMedia == null && allWebServiceTransaction == null;

				}
			}
		}

		public List<ProductsVars> GetProductsVars(long benefitID)
		{
			List<ProductsVars> variants = new List<ProductsVars>();
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					if (dbContext.UserPortalCategories.Any(x => x.CategoryNumber == benefitID))
					{
						var barcodes = dbContext.CategoryVariants.Where(x => x.CategoryNumber == benefitID).Select(x => x.Barcode).ToList();
						if (barcodes.Count > 0)
						{
							variants = dbContext.ProductsVars
								.Where(r => (barcodes.Select(x => x).Contains(r.FullBarCode)
								&& !r.DisabledToOrder && (r.PaymentModelId == null || r.PaymentModelId.Value == 1))
								&& (!(!ContextManager.CurrentOrganization().IsNewSubsidy && !string.IsNullOrEmpty(r.VarPriceFormula) && !r.VarPriceFormula.All(Char.IsDigit)))
								&& ((r.LastImplementationDate != null && !(r.LastImplementationDate.Value.Date < DateTime.Now.Date)) || r.LastImplementationDate == null)
								&& (r.UserTypeId == null || r.UserTypeId == 0 || r.UserTypeId == ContextManager.CurrentUser().PremiumType)
								)

								.ToList();
						}
					}
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, $"Error on GetProductsVars, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
				throw ex;
			}
			return variants;
		}

		public List<BusinessSubTypeSpecificationCurrent> GetBusinessSubTypeSpecificationCurrents(List<ProductsVars> products)
		{
			try
			{
				var businessSubTypeList = products.Select(p => p.BusinessSubTypeId.HasValue ? p.BusinessSubTypeId.Value : 0).ToList();
				using (var dbContext = ContextManager.ClubContext())
				{
					var businessSpecificationCurrents = dbContext.BusinessSubTypeSpecificationCurrent.AsNoTracking().Where(r => businessSubTypeList.Contains(r.BusinessSubTypeId)).ToList();
					return businessSpecificationCurrents;
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, $"Error in GetBusinessSubTypeSpecificationCurrents, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
				return null;
			}
		}

		public int GetSumOfTrnasactionsByBarcode(string barcode)
		{
			try
			{
				using (var dbContext = ContextManager.ClubContext())
				{
					return dbContext.WebServiceTransaction.Count(x => x.TtransactionProductId == barcode && x.Ttransactionquantity != "-1");
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, $"Error in GetSumOfTrnasactionsByBarcode, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
				return 0;
			}
		}

		public List<BusinessSubTypeSpecificationByVariants> GetBusinessSubTypeSpecificationByVariants(List<ProductsVars> products)
		{
			try
			{
				var FullBarCodeList = products.Select(p => p.FullBarCode).ToList();
				using (var dbContext = ContextManager.ClubContext())
				{
					var businessSpecificationByVariants = dbContext.BusinessSubTypeSpecificationByVariants.AsNoTracking().Where(r => FullBarCodeList.Contains(r.BarCode)).ToList();
					return businessSpecificationByVariants;
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, $"Error in GetBusinessSubTypeSpecificationByVariants, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
				throw ex;
			}
		}
		public async Task<List<WalletTagChainsData>> GetWalletChainNearMeFromDb(string walletId, decimal lat, decimal lon)
		{
			var list = new List<WalletTagChainsData>();

			try
			{
				using (var context = new Common.EF.DTS_Online.DTS_OnlineContext())
				{
					var pLat = new SqlParameter("@Lat", SqlDbType.Float) { Value = (double)lat };
					var pLon = new SqlParameter("@Lon", SqlDbType.Float) { Value = (double)lon };

					var raw = context.Set<Common.EF.DTS_Online.View_GetBusinessAndTags>()
						.FromSqlRaw("EXEC dbo.GetBusinessAndTagsNearMe @WalletID, @Lat, @Lon",
							new SqlParameter("@WalletID", walletId),
							pLat,
							pLon)
						.AsNoTracking()
						.AsEnumerable()
						.ToList();

					list = await BuildWalletTagChainDto(context, walletId, raw);

					return list;
				}
			}
			catch (Exception ex)
			{
				return list;
			}

		}
		private async Task<List<WalletTagChainsData>> BuildWalletTagChainDto(Common.EF.DTS_Online.DTS_OnlineContext context, string walletId, List<Common.EF.DTS_Online.View_GetBusinessAndTags> source)
		{
			var list = new List<WalletTagChainsData>();

			// שליפה של ערים/אזורים
			var subBranchCityRegion = await GetSubBranchDictByWallet(context, walletId);

			foreach (var item in source)
			{
				// איתור ערים ואזורים
				var business = subBranchCityRegion
					.FirstOrDefault(x => x.BusinessId.ToString() == item.BuisnessID);

				string subCities = business?.SubBranchCities;
				string subRegions = business?.SubBranchRegions;

				// בדיקה האם הקבוצה (תגית) כבר קיימת
				var group = list.FirstOrDefault(x => x.TagName == item.TagName);

				if (group == null)
				{
					group = new WalletTagChainsData
					{
						TagId = item.TagID,
						TagName = item.TagName,
						TagDescription = item.TagDescription,
						TagSort = item.TagSort,
						TagType = item.TagType,
						TagImg = item.TagImageBase64,
						SubBranchCities = subCities,
						SubBranchRegions = subRegions,

						WalletChainData = new List<WalletChainData>()
					};

					list.Add(group);
				}

				// הוספת הרשת תחת התגית
				group.WalletChainData.Add(new WalletChainData
				{
					ChainID = item.ChainID.ToString(),
					ChainName = item.ChainName,
					ErrorID = "0",
					LogoURL = item.LogoURL,
					IsWebOnline = item.IsWebOnline,
					WebSite = item.WebSite,
					SearchKeyWords = item.SearchKeyWords,
					SubBranchRegions = subRegions,
					SubBranchCities = subCities,

					// ⭐ חשוב: DistanceKm יופיע רק אם הגיע מה-SP
					//DistanceKm = item.DistanceKm
				});
			}

			return list;
		}

		public async Task<List<WalletChainBranches>> GetWalletChainBranchesNearMe(string walletId, string chainId, decimal lat, decimal lon)
		{
			List<WalletChainBranches> result = new List<WalletChainBranches>();

			try
			{
				using (var context = new Common.EF.DTS_Online.DTS_OnlineContext())
				{
					var pWallet = new SqlParameter("@WalletID", walletId);
					var pChain = new SqlParameter("@ChainID", chainId);

					var pLat = new SqlParameter("@Lat", SqlDbType.Float) { Value = (double)lat };

					var pLon = new SqlParameter("@Lon", SqlDbType.Float) { Value = (double)lon };

					result = await context.Set<WalletChainBranches>()
					   .FromSqlRaw(@"
                    EXEC [dbo].[GetWalletChainBranchesNearMe]
                        @WalletID,
                        @ChainID,
                        @Lat,
                        @Lon",
						   pWallet, pChain, pLat, pLon)
					   .AsNoTracking()
					   .ToListAsync();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"❌ Failed to load sub-branch data for WalletID={walletId} ,Chain={chainId}: {ex.Message}");

			}

			return result;
		}

		private async Task<List<SubBranchCityRegionData>> GetSubBranchDictByWallet(Common.EF.DTS_Online.DTS_OnlineContext context, string walletId)
		{
			var cacheKey = string.Format(CacheKeys.SubBranchDictByWallet, walletId);
			var cached = Cache.Get(cacheKey) as List<SubBranchCityRegionData>;

			if (cached != null)
			{
				return cached;
			}
			var list = new List<SubBranchCityRegionData>();

			try
			{

				var pWallet = new SqlParameter("@WalletID", walletId);

				list = await context.Set<SubBranchCityRegionData>()
					.FromSqlRaw(@"EXEC [dbo].[GetSubBranchDictByWallet] @WalletID", pWallet).AsNoTracking().ToListAsync();

				Cache.Set(cacheKey, list, TimeSpan.FromDays(1));

				return list;


			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"❌ Failed to load sub-branch data for WalletID={walletId}: {ex.Message}");
			}

			return list;
		}
		public Common.EF.Club.Payments GetPaymentRowByPaymentId(long paymentId)
		{
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					return orgDb.Payments.FirstOrDefault(p => p.PaymentId == paymentId);
				}
			}
			catch
			{
				return null;
			}
		}
		public bool IsEventimBenefitByCategoryNumber(long categoryNumber)
		{
			try
			{
				UserPortalCategory category = new UserPortalCategory();
				using (var orgDb = ContextManager.ClubContext())
				{
					category = orgDb.UserPortalCategories.FirstOrDefault(x => x.CategoryNumber == categoryNumber);
				}
				if (category == null) return false;
				using (var context = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
				{
					return context.Business.FirstOrDefault(x => x.BuisnessId.Equals(category.BusinessID))?.IsTicketsHub == true;
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in IsEventimBenefitByCategoryNumber, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
				throw;
			}
		}

		public bool IsEventimBenefitByBarcode(string barcode)
		{
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					var productsVar = orgDb.ProductsVars.FirstOrDefault(p => p.FullBarCode.Equals(barcode) && p.TicketsHubEventId != null);
					return productsVar != null;
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in IsEventimBenefit, error message: {ex.Message}, stacktrace: {ex.StackTrace}");
				return false;
			}
		}

		public List<AllOrdersHistory> GetHistoryForMember(MemberHistoryDTO memberHistoryDTO)
		{
			List<AllOrdersHistory> allOrdersVariants = new List<AllOrdersHistory>();
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					allOrdersVariants = orgDb.AllOrdersHistory.AsNoTracking().Where(row => row.MemberID == ContextManager.CurrentUser().Id && row.OrderDate >= memberHistoryDTO.FromDate && row.OrderDate <= memberHistoryDTO.ToDate)
						.OrderBy(row => row.TTransactionDateTime).ToList();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetHistoryForMember, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return allOrdersVariants;
		}

		public List<v_wAllOrders> GetAllOrdersForMember(MemberHistoryDTO memberHistoryDTO)
		{
			List<v_wAllOrders> allOrders = new List<v_wAllOrders>();
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					allOrders = orgDb.v_wAllOrders.AsNoTracking().Where(row => row.MemberId == ContextManager.CurrentUser().Id && row.OrderDate.Value >= memberHistoryDTO.FromDate && row.OrderDate.Value <= memberHistoryDTO.ToDate)
						.ToList();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetAllOrdersForMember, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return allOrders;
		}
		public List<v_wAllOrders> GetAllOrdersByOrderId(int orderId)
		{
			List<v_wAllOrders> allOrders = new List<v_wAllOrders>();
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					allOrders = orgDb.v_wAllOrders.AsNoTracking().Where(row => row.OrderId.Value == orderId)
						.ToList();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetAllOrdersForMember, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return allOrders;
		}

		public AtractionsOrders GetAttractionsOrderByAsmachta(long orderAsmchta)
		{
			AtractionsOrders order = null;
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					order = orgDb.AtractionsOrders.FirstOrDefault(o => o.MemberOrderAsmchta == orderAsmchta);
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetAllOrdersForMember, orderAsmchta:{orderAsmchta}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return order;
		}

		public AtractionsOrders GetAttractionsOrderByUniqueOrderIdentity(long uniqueOrderIdentity)
		{
			AtractionsOrders order = null;
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					order = orgDb.AtractionsOrders.FirstOrDefault(o => o.UniqueOrderIdentity == uniqueOrderIdentity);
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetAttractionsOrderByUniqueOrderIdentity, UniqueOrderIdentity:{uniqueOrderIdentity}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return order;
		}

		public bool isHaveTsarchanutVariant(List<string> barcodes)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.ProductsVars.Any(x => barcodes.Contains(x.FullBarCode) && x.BusinessSubTypeId.Value == 26);
			}
		}

		public bool CheckAllBenefitIdExists(List<long> benefitIds)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.UserPortalCategories.Where(w => benefitIds.Contains(w.CategoryNumber)).All(c => benefitIds.Contains(c.CategoryNumber));
			}
		}

		public bool CheckAvailableGiftCard(int SerieId)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.CardsSkeleton.Any(x => x.SerieId == SerieId && x.CardId == null);
			}
		}

		public Common.EF.Club.Orders InsertNewOrder(Common.EF.Club.Orders order)
		{
			using (var context = ContextManager.ClubContext())
			{
				context.Orders.Add(order);
				context.SaveChanges();
				return order;
			}
		}
		public MemberAddress GetMemberAddressForTzarchanut(PurchaseRequestDTO purchaseRequestDTO)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					return context.MemberAddresses.Where(m => m.MemberId == ContextManager.CurrentUser().Id
					&& m.ApartmentNumber == purchaseRequestDTO.ApartmentNumber
					&& m.HouseNumber == purchaseRequestDTO.HouseNumber
					&& m.StreetName == purchaseRequestDTO.StreetName
					&& m.CityId == purchaseRequestDTO.City
					&& m.Entrance == purchaseRequestDTO.Entrance
					&& m.Mailbox == purchaseRequestDTO.Mailbox).FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public MemberAddress GetMemberAdressForRegularBarCodes(PurchaseRequestDTO purchaseRequestDTO)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					return context.MemberAddresses.Where(m => m.MemberId == purchaseRequestDTO.MemberId
					&& m.ApartmentNumber == purchaseRequestDTO.ApartmentNumber
					&& m.HouseNumber == purchaseRequestDTO.HouseNumber
					&& (m.StreetName == purchaseRequestDTO.StreetName || m.StreetName == ContextManager.CurrentUser().Address)).FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public MemberAddress GetMemberAddress(Common.EF.Club.AllMembers member, string ApartmentNumber)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					return context.MemberAddresses.Where(m => m.MemberId == ContextManager.CurrentUser().Id
					&& m.ApartmentNumber == ApartmentNumber
					&& m.HouseNumber == member.HouseNumber
					&& (m.StreetName == member.StreetName || m.StreetName == member.Address)
					&& m.CityId == member.City).FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public CouponsStocksDetails GetCouponsStockType(int stockId)
		{
			using (var context = new DTS_OnlineContext())
			{
				return context.CouponsStocksDetails.FirstOrDefault(type => type.StockId == stockId);
			}
		}
		public long AddMemberAdress(MemberAddress memberAddress)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					var res = context.MemberAddresses.Add(memberAddress);
					context.SaveChanges();
					return memberAddress.MemberAddressId;
				}
			}
			catch (Exception ex)
			{
				return -1;
			}
		}
		public Common.EF.Club.Orders GetOrderByOrderId(int orderId)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.Orders.FirstOrDefault(o => o.OrderId == orderId);
			}
		}
		public List<AtractionsOrders> GetAtractionsOrdersByOrderId(int orderId)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.AtractionsOrders.Where(ao => ao.OrderId == orderId).ToList();
			}
		}
		public void RemoveCouponsStockAtrOrders(CouponsStockAtrorders coupon)
		{
			using (var context = ContextManager.ClubContext())
			{
				context.CouponsStockAtrorders.Remove(coupon);
				context.SaveChanges();
			}
		}
		public CouponsStockAtrorders GetCouponsStock(long MemberOrderAsmachta)
		{
			using (var context = ContextManager.ClubContext())
			{
				return context.CouponsStockAtrorders.FirstOrDefault(atr => atr.Asmachta == MemberOrderAsmachta);
			}
		}
		public bool DoVariantsRollBack(Common.EF.Club.Orders order, List<AtractionsOrders> attractionsOrders)
		{
			List<WebServiceTransaction> wst = new List<WebServiceTransaction>();
			using (var context = ContextManager.ClubContext())
			{
				wst = context.WebServiceTransaction.Where(x => x.OrderId == order.OrderId).ToList();
				#region remove products of type Attractions, Movies, Spa, Tzimers for this orderId: 

				if (attractionsOrders != null && attractionsOrders.Any())
				{
					context.AtractionsOrders.RemoveRange(attractionsOrders);
					DtsLoggger.Logger.Info($"DoVariantsRollBack. removed Attractions for orderId: {order.OrderId}.");
				}

				List<Moviesorders> moviesOrders = context.Moviesorders.Where(attr => attr.OrderId == order.OrderId).ToList();
				if (moviesOrders != null && moviesOrders.Any())
				{
					context.Moviesorders.RemoveRange(moviesOrders);
					DtsLoggger.Logger.Info($"DoVariantsRollBack. removed Movies for orderId: {order.OrderId}.");
				}

				List<Spaorders> spaOrders = context.Spaorders.Where(attr => attr.OrderId == order.OrderId).ToList();
				if (spaOrders != null && spaOrders.Any())
				{
					context.Spaorders.RemoveRange(spaOrders);
					DtsLoggger.Logger.Info($"DoVariantsRollBack. removed Spa for orderId: {order.OrderId}.");
				}

				List<Tzimersorders> tzimersOrders = context.Tzimersorders.Where(attr => attr.OrderId == order.OrderId).ToList();
				if (tzimersOrders != null && tzimersOrders.Any())
				{
					context.Tzimersorders.RemoveRange(tzimersOrders);
					DtsLoggger.Logger.Info($"DoVariantsRollBack. removed Tzimers for orderId: {order.OrderId}.");
				}

				#endregion


				if (wst.Any())
					context.WebServiceTransaction.RemoveRange(wst);
				if (order != null)
					context.Orders.Remove(order);

				context.SaveChanges();
			}
			return true;
		}
		public bool UpdateCreateOrderByEvent(int dtsOrderId, int ticktsHubOrderId, List<TicketsHubRepository.EF.TicketsHub.OrderTickets> orderTickets, string eventGuid)
		{
			using (var context = ContextManager.ClubContext())
			{
				var order = context.Orders.FirstOrDefault(x => x.OrderId == dtsOrderId);
				if (order != null)
				{
					order.TicketHubOrderId = ticktsHubOrderId;
					order.EventsGuid = eventGuid;
				}
				var attractionOrders = context.AtractionsOrders.Where(x => x.OrderId.HasValue && x.OrderId.Value == dtsOrderId).ToList();
				if (attractionOrders.Count(c => orderTickets.Select(s => s.VariantFullBarcode.Trim()).Contains(c.BarCode.Trim())) != orderTickets.Count)
				{
					/*string ordersJsonInfo = GeneralFunctions.JsonSerializeObject(new { orderTickets, attractionOrders });
                    Logger.Error("Error on Ticket amount not equal to order amount - order failed! TicktsHubOrderId:{0} OrdersObjects=>  {1}", ticktsHubOrderId, ordersJsonInfo);*/
					LoggerHelper.Error("Error on Ticket amount not equal to order amount - order failed!");
					throw new BusinessException("ההזמנה לא נקלטה- לא סופק EVENTSGUID להזמנה מסוג מופעים והצגות");
				}
				foreach (var orderTicket in orderTickets)
				{
					var attractionOrder = attractionOrders.FirstOrDefault(x => x.BarCode == orderTicket.VariantFullBarcode.Trim() && (x.TicketHubTicketId == null || x.TicketHubTicketId == orderTicket.OrderTicketId));
					if (attractionOrder != null)
					{
						attractionOrder.TicketHubTicketId = orderTicket.OrderTicketId;
					}
					else
					{
						DtsLoggger.Logger.Info($"EventsFunctions.UpdateOrdersByEvent' warning! attractionOrder is not found! by dtsOrderId:{dtsOrderId}, orderTicket.Barcode:{orderTicket.Barcode}");
						return false;
					}
				}
				context.SaveChanges();
			}
			return true;
		}
		public bool UpdateAddressForAtractionsOrders(string memberId, string productBarCode, string orderId, long memberAddressId)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					var atractionsOrders = context.AtractionsOrders.Where(ao => ao.OrderId.ToString() == orderId && ao.MemberID == memberId && ao.BarCode == productBarCode && ao.MemberAddressId == null).ToList();
					foreach (var atractionsOrder in atractionsOrders)
					{
						atractionsOrder.MemberAddressId = (int)memberAddressId;
					}
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public bool UpdateMemberAddress(MemberAddress memberAddress)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					var res = context.MemberAddresses.Attach(memberAddress);
					context.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public string GetCardByMemberId(string memberId)
		{
			try
			{
				using (var context = ContextManager.ClubContext())
				{
					Common.EF.Club.Cards card = context.Cards.FirstOrDefault(x => x.Idmember.Equals(memberId) && x.CardStatus.Equals((int)ECardStatus.ACTIVE));
					if (card != null)
						return card.CardNumber;
					else
					{
						LoggerHelper.Error($"Warning MembershipFunctions.GetMemberCard' Member Card is not found!  dtsId:{memberId}");
						return string.Empty;
					}
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error(ex, $"Error in GetCardByMemberId, error message: {ex.Message}, stackTrace: {ex.StackTrace}");
				return string.Empty;
			}
		}

		public AtractionsOrders GetAttractionsOrder(long orderAsmchta)
		{
			AtractionsOrders order = null;
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					order = orgDb.AtractionsOrders.FirstOrDefault(o => o.MemberOrderAsmchta == orderAsmchta);
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetAllOrdersForMember, orderAsmchta:{orderAsmchta}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return order;
		}

		public List<AtractionsOrders> GetAttractionsOrderByOrderId(int orderId)
		{
			List<AtractionsOrders> list = null;
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					list = orgDb.AtractionsOrders.Where(o => o.OrderId == orderId).ToList();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetAllOrdersForMember, orderId:{orderId}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return list;
		}

		public void InsertInvoiceManagementRequests(string request, int orgId)
		{
			try
			{
				using (var dbContext = new Nofshonit.Common.EF.DTS_Online.DTS_OnlineContext())
				{
					Nofshonit.Common.EF.DTS_Online.InvoiceManagementRequests invoiceRequest = new Nofshonit.Common.EF.DTS_Online.InvoiceManagementRequests()
					{
						StatusId = 3,
						DateAdded = DateTime.Now,
						OrgId = orgId,
						InvoiceRequest = request,
						Attempts = 0,
						Exception = null,
					};

					dbContext.InvoiceManagementRequests.Add(invoiceRequest);
					dbContext.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in InsertInvoiceManagementRequests, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
		}
		public Common.EF.Club.WalletLoadMoney GetWalletById(string id)
		{
			using (var context = ContextManager.ClubContext())
			{
				var wallet = context.WalletLoadMoney.FirstOrDefault(w => w.WalletId.Equals(long.Parse(id)));
				if (wallet == null)
				{
					return null;
				}
				return wallet;
			}
		}
		public async Task<string> GenerateUrl(string longurl)
		{
			var DTSOnlineContext = string.Format(_configuration.GetConnectionStringByValue<string>("DTSOnlineContext"), ContextManager.CurrentOrganization().DBName);
			using (var connection = new SqlConnection(DTSOnlineContext))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "[BLShortUrl].[dbo].[SP_Generate_ShortURL]";
				command.Parameters.Add(new SqlParameter("@OrganizationId", ContextManager.CurrentOrganization().OrgId));
				command.Parameters.Add(new SqlParameter("@LongURL", longurl));
				command.Parameters.Add(new SqlParameter("@memberId", ContextManager.CurrentUser().IdentityNumber));
				try
				{
					using (SqlDataReader rdr = command.ExecuteReader())
					{
						if (!rdr.HasRows)
							return string.Empty;
						while (rdr.Read())
						{
							var shortUrlFromProcedure = (string)rdr["ShortURL"];
							var error = (string)rdr["Error"];
							if (!string.IsNullOrEmpty(shortUrlFromProcedure))
								return shortUrlFromProcedure;
							else
								return string.Empty;
						}
						throw new Exception("Any problem with Get_Available_ShortURL procedure. (GetAvailableShortURL)");
					}
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}
		public async Task<BarcodePopupDetails> GetPopupBarcodeDetails(string asmachta)
		{
			BarcodePopupDetails barcodePopupDetails = new BarcodePopupDetails();
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					AllOrdersHistory order = orgDb.AllOrdersHistory.Where(o => o.OrderAsmchta.ToString() == asmachta && o.MemberID == ContextManager.CurrentUser().IdentityNumber).FirstOrDefault();
					if (order != null)
					{
						barcodePopupDetails.CardNumber = order.CardNumber;
						barcodePopupDetails.ShortNameVar = order.shortNameVar;
						barcodePopupDetails.CategoryName = order.CategoryName;
						barcodePopupDetails.LastImplementationDate = order.LastImplementationDate;
						barcodePopupDetails.DigitalCodeType = order.DigitalCodeType;
					}
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetPopupBarcodeDetails, asmachta:{asmachta}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return barcodePopupDetails;
		}
		public async Task<List<WebServiceTransaction>> GetWebServiceTransactionsByMemberId(string memberId)
		{
			List<WebServiceTransaction> transactions = new List<WebServiceTransaction>();
			try
			{
				using (var orgDb = ContextManager.ClubContext())
				{
					transactions = orgDb.WebServiceTransaction.Where(t => t.TtransactionMemberId == memberId).ToList();
				}
			}
			catch (Exception ex)
			{
				LoggerHelper.Error($"Error in GetWebServiceTransactionsByMemberId, memberId: {memberId}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
			}
			return transactions;
		}

	}
}
