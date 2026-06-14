using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nofshonit.Common;
using Nofshonit.Common.EF;
using Nofshonit.Infrastructure.EF;

namespace Nofshonit.Repositories.ClubModel
{
    public class ClubContextFactory : Nofshonit.Common.EF.IClubContextFactory
    {
        private readonly HttpContext httpContext;
        public Nofshonit.Common.EF.Club.ClubContext DbContext { get; set; }
        public ClubContextFactory(
            IHttpContextAccessor httpContentAccessor
            )
        {
            httpContext = httpContentAccessor.HttpContext;
            var guid = httpContext.Request.Headers["OrgGuid"];
            DbContext = Helpers.GetClubContextByGuid(guid);
        }
    }
}

            httpContext = httpContentAccessor.HttpContext;
            DbContext = new Helpers.HelperFunctions().GetClubContext();