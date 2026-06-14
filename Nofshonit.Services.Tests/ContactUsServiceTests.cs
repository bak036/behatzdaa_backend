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
using Nofshonit.Services.ContactUs;

namespace Nofshonit.Services.Tests
{
	public class ContactUsServiceTests
	{
		private ContactUsService CreateDefaultContactUsService()
		{
			return new ContactUsService();
		}

		[Fact]
		public void GetCrmTypesTest()
		{
			// Arrange
			var contactUsService = CreateDefaultContactUsService();

			// Act
			var result = contactUsService.GetCrmTypes();

			// Assert
			Assert.True(result.Count == 17);
		}

		[Fact]
		public void OpenServiceCaseRequestTest()
		{
			// Arrange
			var contactUsService = CreateDefaultContactUsService();

			// Act
			var result = contactUsService.OpenServiceCaseRequest(
				new ContactUsDTO
				{
					IdentityNumber= "314056649",
					Description = "TEST",
					Subject = "test",
					InputEmail = "testtest@gmail.com",
					CrmSubjectId = 1,
					FullName = "TestName",
				}).Result;

			// Assert
			Assert.NotNull(result);
		}
	}
}
