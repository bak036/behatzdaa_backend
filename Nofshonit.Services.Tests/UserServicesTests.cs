using Nofshonit.Common.Interfaces;
using Nofshonit.Common;
using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.Club;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Nofshonit.Infrastructure;
using Nofshonit.Services.Base;
using Nofshonit.Services.ShopingBasketService;
using Nofshonit.Services.UsersService;
using Nofshonit.Common.Infrastructure;

namespace Nofshonit.Services.Tests
{
    public class UserServicesTests
    {
		private UserService CreateDefaultUserServices()
		{
            return new UserService();
		}

		[Fact]
		public void AuthenticateTest()
		{
			// Arrange
			var userService = CreateDefaultUserServices();

			// Act
			var result = userService.Authenticate(
				new AuthenticateUserRequestDTO
				{
					IdentityNumber = "314327164",
					Password = "123456789",
					LoginType = Common.DTOs.Enums.ELoginType.MemberIdAndPassword,
					AuthenticateOrJoin = Common.DTOs.Enums.EAuthenticateOrJoin.Authenticate
				},true).Result;

			// Assert
			Assert.True(result.Status);
		}

		[Fact]
		public void GetAllMembersTest()
		{
			// Arrange
			var userService = CreateDefaultUserServices();

			// Act
			var result = userService.GetAllMembers();

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetMemberTest()
		{
			// Arrange
			var userService = CreateDefaultUserServices();

			// Act
			var result = userService.GetCurrentUser().Data;

			// Assert
			Assert.Equal("314327164", result.Id);
		}

		[Theory]
		[InlineData("314327164")]
		public void RecoverPasswordTest(string memberId)
		{
			// Arrange
			var userService = CreateDefaultUserServices();

			// Act
			var result = userService.RecoverPassword(memberId).Result;

			// Assert
			Assert.NotNull(result.Data);
		}

		//[Fact]
		//public void UpdateMemberTest()
		//{
		//	// Arrange
		//	var userService = CreateDefaultUserServices();

  //          // Act
  //          var result = userService.UpdateMember(
  //              new UpdateMamberUserDTO
  //              {
  //                  Id = "314327164",
  //                  FirstName = "BarUnitTestName",
  //                  LastName = "BarUnitTestName",
  //                  NewOrUpdate = Common.DTOs.Enums.ENewOrUpdate.Update,
  //                  Email="BarUnitTest@Email.com",
  //                  PhoneNumber = "054723103",
  //                  CityName = "דגניא א'",
  //                  StreetName = "דגניא א'",
  //                  HouseNumber = "1",

  //              }).Result;

		//	// Assert
		//	Assert.True(result.Status);
		//}

        //[Fact]
        //public void UpdatePasswordTest()
        //{
        //    // Arrange
        //    var userService = CreateDefaultUserServices();

        //    // Act
        //    var forgetPasswordToken = userService.RecoverPassword("314327164").Result.Data;
        //    forgetPasswordToken = forgetPasswordToken.Split(new char[] { '?', '&','=' })[2];
        //    var result = userService.UpdatePassword(
        //        new UserPasswordInfoDTO
        //        {
        //            // need to switch if already activated once
        //            CurrentPassword = "123456",
        //            NewOrUpdate = Common.DTOs.Enums.ENewOrUpdate.Update,
        //            NewPassword = "123456789",
        //            ForgetPasswordToken = "forgetPasswordToken",
        //        }).Result;

        //    // Assert
        //    Assert.True(result.Status);
        //}
    }
}
