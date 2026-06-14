using System;
using Nofshonit.Common.EF.Club;

namespace Nofshonit.Common.EF
{
    /// <summary>
    /// Context factory interface
    /// </summary>
    public interface IClubContextFactory
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        ClubContext DbContext {get;}
    }

    //public class ClubContextFactory : IClubContextFactory
    //{
    //    public ClubContext DbContext()
    //    {
    //        var org = new OrganizationFunctions().GetOrgDetailsByGuid();
    //        if (org == null) throw new Exception();

    //        var dbName = org.DBName;
    //        var orgConnStr = GetAppSettings().GetConnectionString("ClubContext");
    //        var clubConnStr = string.Format(orgConnStr, dbName);

    //        return new Common.EF.Club.ClubContext(clubConnStr);
    //    }
    //}
}