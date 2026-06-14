using System;
using System.Collections.Generic;
using System.Text;
using Nofshonit.Common.DTOs.GeneralDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Nofshonit.Repositories.Helpers
{

    public class OrganizationFunctions : BaseFunctions
    {

        public OrganizationFunctions()
        {

        }
        /// <summary>
        /// List of all organiztion with Database name and Organization Guid
        /// </summary>
        private  List<OrganizationDetailsDTO> OrganiztionDetailsList
        {
            get
            {
                try
                {
                    //var model = (List<OrganizationDetailsDTO>)CacheManager.Get("OrganiztionDetialsList");

                    //if (model == null)
                    //{
                    //    using (Common.EF.DTS_Online.DTS_OnlineContext db = new Common.EF.DTS_Online.DTS_OnlineContext())
                    //    {
                    //        model = db.Organizations.AsNoTracking().Select(m => new OrganizationDetailsDTO
                    //        {
                    //            OrgId = m.OrganizationId,
                    //            DBName = m.Dbname,
                    //            OrganizationGuid = string.IsNullOrEmpty(m.OrganizationGuid) ? string.Empty : m.OrganizationGuid.Trim(),
                    //            Password = m.Password,
                    //            IsNewSubsidy = m.IsNewSubsidy
                    //        }).ToList();
                    //    }

                    //    //CacheManager.Set("OrganiztionDetialsList", model);

                    //}
                    //return model;
                    return null;
                }

                catch (Exception ex)
                {
                  //  Logger.Error($"Error in OrganiztionDetialsList, error message: {ex.Message}");
                    throw ex;
                }
            }
        }

        public OrganizationDetailsDTO GetOrgDetailsByGuid()
        {
            var guid = _httpContextAccessor.HttpContext.Request.Headers["OrgGuid"];
            var orgDetails = OrganiztionDetailsList.FirstOrDefault(x => x.OrganizationGuid == guid);
            if (orgDetails == null) throw new Exception("No orgDetails, probably wrong GUID");
            return orgDetails;
        }

        /// <summary>
        /// Get a List of all Organizations (clubs) from db or cache
        /// </summary>
        /// <returns></returns>
        //public List<Common.EF.DTS_Online.Organizations> GetAllOrganizations()
        //{
        //    var result = new List<Common.EF.DTS_Online.Organizations>();

        //    using (var context = new EF.DTS_Online.DTS_OnlineContext())
        //    {
        //        result = context.Organizations.ToList();
        //    }

        //    return result;
        //}


        /// <summary>
        /// Creates a DBContext for a club DB
        /// </summary>
        /// <returns></returns>
        public Common.EF.Club.ClubContext GetClubContextByGuid()
        {
            var guid = _httpContextAccessor.HttpContext.Request.Headers["OrgGuid"];
            var orgDetails = OrganiztionDetailsList.FirstOrDefault(x => x.OrganizationGuid == guid);
            if (orgDetails == null) throw new Exception();

            var dbName = orgDetails.DBName;
        
            var originalconnStr = new HelperFunctions().GetAppSettings().GetConnectionString("ClubContext");
            var clubConnStr = string.Format(originalconnStr, dbName);

            return new Common.EF.Club.ClubContext(clubConnStr);

        }

        /// <summary>
        /// Get the list of all organizion to get the database name
        /// </summary>
        /// <returns></returns>
        public OrganizationDetailsDTO GetOrganiztion(string orgGuide)
        {
            try
            {
                var org = OrganiztionDetailsList.FirstOrDefault(r => r.OrganizationGuid.Equals(orgGuide));
                if (org == null)
                    return null;
                else
                {
                    return org;
                }
            }
            catch (Exception ex)
            {
                //Logger.Error($"Error in GetOrganiztion by orgGuide, error message: {ex.Message}");
                throw ex;
            }
        }

        /// <summary>
        /// Get the list of all organizion to get the database name
        /// </summary>
        /// <returns></returns>
        public OrganizationDetailsDTO GetOrganiztion(int dtsId)
        {
            try
            {
                var org = OrganiztionDetailsList.FirstOrDefault(r => r.OrgId.Equals(dtsId));
                if (org == null)
                    return null;
                else
                {
                    return org;
                }
            }
            catch (Exception ex)
            {
                //Logger.Error($"Error in GetOrganiztion by dtsId, error message: {ex.Message}");
                throw ex;
            }
        }

        public  bool? CheckCouponStock(int stockId)
        {
            bool? result = null;
            try
            {
                using (Common.EF.DTS_Online.DTS_OnlineContext db = new Common.EF.DTS_Online.DTS_OnlineContext())
                {
                    result = db.CouponsStock.Any(x => x.CouponStatus == false && x.StockId == stockId);
                }
            }
            catch (Exception ex)
            {
               // Logger.Error($"Error in CheckCouponStock'  stockId:{stockId}, error message: {ex.Message}");
            }
            return result;
        }
    }
}
