using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Nofshonit.Infrastructure.Communication
{
    public interface IHttpContextHelper
    {
        string GetHeaders();

        string GetGuidFromHeader();

        string GetClientIpAddress();
        //void SetCurrentOrganization();
        //string GetCurrentOrganization();
    }

    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextHelper(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetGuidFromHeader()
        {
            return _httpContextAccessor.HttpContext.Request.Headers["orgGuid"].ToString();
        }

        public string GetClientIpAddress()
        {
            return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public string GetHeaders()
        {
            throw new NotImplementedException();
        }

        //public void SetCurrentOrganization(string organization)// should be DTO
        //{
        //    _httpContextAccessor.HttpContext.Items["Organization"] = organization;
        //}

        //public string GetCurrentOrganization()
        //{
        //    var org = _httpContextAccessor.HttpContext.Items["Organization"];

        //    if (org == null)
        //    {
        //        var org = new OrganizationFunctions().GetOrgDetailsByGuid();
        //        if (org == null) throw new Exception();

        //        var dbName = org.DBName;
        //        var orgConnStr = GetAppSettings().GetConnectionString("ClubContext");
        //        var clubConnStr = string.Format(orgConnStr, dbName);

        //        return new Common.EF.Club.ClubContext(clubConnStr);
        //        // get the org from DB and save on the request
        //    }
        //}
    }
}
