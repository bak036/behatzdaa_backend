using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Nofshonit.Services.Tests.Helpers;
using Xunit.Abstractions;

namespace Nofshonit.Services.Tests.Extensions
{
    public static class NLogTestOutputExtensions
    {
        public static ILogger GetNLogLogger(
          this ITestOutputHelper testOutputHelper)
        {
            return testOutputHelper.GetNLogLogger(string.Empty, true);
        }

        public static ILogger GetNLogLogger(
            this ITestOutputHelper testOutputHelper,
            string loggerName,
            bool addNumericSuffix = false)
        {
            var addedName = TestOutputHelpers.AddTestOutputHelper(
                testOutputHelper,
                loggerName,
                addNumericSuffix);

            return LogManager.GetLogger(addedName);
        }

        public static void RemoveTestOutputHelper(this ILogger logger)
        {
            TestOutputHelpers.RemoveTestOutputHelper(logger.Name);
        }
    }
}
