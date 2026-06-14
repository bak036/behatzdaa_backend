using NLog;
using NLog.Config;
using Nofshonit.Services.Tests.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit.Abstractions;

namespace Nofshonit.Services.Tests.Helpers
{
    public static class TestOutputHelpers
    {
        public const string DefaultLoggerName = "Test";

        private static int _loggerId;

        public static string AddTestOutputHelper(
            ITestOutputHelper testOutputHelper,
            string loggerName,
            bool addNumericSuffix)
        {

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(assemblyFolder + @"\nlog.config");

            
            var targets = LogManager.Configuration.AllTargets
                .OfType<TestOutputTarget>();

            if (string.IsNullOrWhiteSpace(loggerName))
                loggerName = DefaultLoggerName;

            if (addNumericSuffix)
                loggerName += Interlocked.Increment(ref _loggerId);

            foreach (var target in targets)
                target.Add(testOutputHelper, loggerName);

            return loggerName;
        }

        public static void RemoveTestOutputHelper(string loggerName)
        {
            var targets = LogManager.Configuration.AllTargets
                .OfType<TestOutputTarget>();

            foreach (var target in targets)
                target.Remove(loggerName);
        }
    }
}
