

using Xunit;
using Nofshonit.Services.SearchService;
using System.Collections.Generic;

namespace Nofshonit.Services.Tests
{
    public class SearchServiceTest
    {
		private SearchService.SearchService GetService()
		{
			return new SearchService.SearchService();
		}

        [Theory]
        [MemberData(nameof(PowerOfAcTestData))]
        public void GetAutoCompleteResults(string text, int selectTop)
        {
			// Act
			var result = GetService().GetAutoCompleteResults(text, selectTop);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [MemberData(nameof(PowerOfTestData))]
        public void GetSearchData(string text, int selectTop, long superCategory, string region)
        {
			// Act
			var result = GetService().GetSearchData(text, selectTop, superCategory, region);

            // Assert
            Assert.NotNull(result); //Change to relevant output
        }


        public static IEnumerable<object[]> PowerOfAcTestData()
        {
            yield return new object[] { "ילדים", 10 };
            yield return new object[] { "אב", 10 };
            yield return new object[] { "רו", 10 };
            yield return new object[] { "ספא", 10 };
            yield return new object[] { "גד", 10 };
            yield return new object[] { "אהב", 10 };
            yield return new object[] { "צימ", 10 };
            yield return new object[] { "מלו", 10 };
        }

        public static IEnumerable<object[]> PowerOfTestData()
        {
            yield return new object[] {"ילדים", 50, 0, ""};
            yield return new object[] { "פארק", 150, 0, "" };
            yield return new object[] { "מיטה", 80, 0, "" };
            yield return new object[] { "ספא", 90, 0, "" };
        }
    }
}
