using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Serilog;

namespace Nofshonit.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureLogging(logging =>
                                logging.AddFilter("System", LogLevel.Debug)
                                .AddFilter<DebugLoggerProvider>("Microsoft", LogLevel.Trace))
                .UseStartup<Startup>()
                .UseUrls("http://localhost:80");
    }
}
