using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Nofshonit.Repositories.Helpers
{
    public class HelperFunctions : BaseFunctions
    {
   
        public HelperFunctions()
        {

        }

        /// <summary>
        /// Easy way to get access to the appsettings.json 
        /// </summary>
        /// <returns></returns>
        public IConfigurationRoot GetAppSettings()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            var builder = new ConfigurationBuilder()
            .SetBasePath(appRoot)
            .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        public Common.EF.Club.ClubContext GetClubContext()
        {
            var org = new OrganizationFunctions().GetOrgDetailsByGuid();
            if (org == null) throw new Exception();

            var dbName = org.DBName;
            var orgConnStr = GetAppSettings().GetConnectionString("ClubContext");
            var clubConnStr = string.Format(orgConnStr, dbName);

            return new Common.EF.Club.ClubContext(clubConnStr);
        }

        public class Logs
        {

        }
    }
}
