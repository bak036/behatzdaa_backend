using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Linq;

namespace Nofshonit.Repositories.Helpers
{

    public class LogsFunctions : BaseFunctions
    {
        public LogsFunctions()
        {

        }

        public string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        public Common.EF.DTS_Logs.DataCenter InitialRequestLog(string MethodName, string IpAddress, DateTime StartDate, string XmlRequest)
        {
            try
            {
                // Logger.Info($"MethodName:{MethodName}Address: {IpAddress}, StartDate: {StartDate.ToString()}, XmlRequest: {XmlRequest.Replace("{", "(").Replace("}", ")")} ");
                Common.EF.DTS_Logs.DataCenter log = new Common.EF.DTS_Logs.DataCenter();
                log.Command = MethodName;
                log.DtsServiceId = 11; // 11 = לאומי אפליקציה
                log.Ip = IpAddress;
                log.TimeStamp = StartDate;
                log.Request = XmlRequest;

                return log;
            }
            catch (Exception ex)
            {
                //Logger.Error($"Error in InitialRequestLog, Error message: {ex.Message}");
                throw ex;
            }
        }


        public void FinalResponseLog(Common.EF.DTS_Logs.DataCenter log, DateTime endDate, string XmlResponse, bool IsSucess, Common.DTOs.GeneralDTOs.OrganizationDetailsDTO Org)
        {
            try
            {
                //Logger.Info($"MethodName:{log.Command}, XmlResponse: {XmlResponse.Replace("{", "(").Replace("}", ")")}, IsSucess: {IsSucess}");

                if (Org == null)
                    log.OrganizationId = 102;
                else
                    log.OrganizationId = Org.OrgId;

                log.Response = XmlResponse;
                log.IsException = !IsSucess;
                TimeSpan d = endDate - log.TimeStamp;
                log.Milliseconds = d.Milliseconds;
                log.Seconds = d.Seconds;

                SaveLog(log);
            }
            catch (Exception ex)
            {
              //  Logger.Error($"Error in FinalResponseLog, Error message: {ex.Message}");
                if (ex.InnerException != null)
                {
                  //  Logger.Error($"Inner message: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                      //  Logger.Error($"Inner inner message: {ex.InnerException.InnerException.Message}");
                    }
                }

                throw ex;
            }
        }


        //public void FinalResponseLogOpenCase(Common.EF.DTS_Logs.DataCenter log, DateTime endDate, string XmlResponse, bool IsSucess, OpenServiceCaseRequest req)
        //{
        //    try
        //    {
        //        if (req == null)
        //            log.RequestId = 0;
        //        else
        //            log.Response = req.MemberId;

        //        log.Response = XmlResponse;
        //        log.IsException = !IsSucess;
        //        TimeSpan d = endDate - log.TimeStamp;
        //        log.Milliseconds = d.Milliseconds;
        //        log.Seconds = d.Seconds;

        //        SaveLog(log);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Logger.Error($"Error in FinalResponseLogOpenCase, Error message: {ex.Message}");
        //        throw ex;
        //    }
        //}

        public void SaveLog(Common.EF.DTS_Logs.DataCenter log)
        {
            try
            {
                using (var context = new Common.EF.DTS_Logs.DTS_LogsContext())
                {
                    context.DataCenter.Add(log);
                    var save = context.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                //Logger.Error($"Error in SaveLog, Error message: {ex.Message}");
                throw ex;
            }
        }


        /// <summary>
        /// Get the log info of the change log for Header, Description and ShortDescription
        /// </summary>
        //public static List<Common.DTOs.GeneralDTOs.FieldsExternalEditDTO> GetChangeLog(List<long> CategoriesNumberInts)
        //{
        //    try
        //    {


        //        var logBenefitChanges = new List<Common.DTOs.GeneralDTOs.FieldsExternalEditDTO>();

                
        //        var logDB = new Common.EF.DTS_Logs.DTS_LogsContext();
        //        DateTime startCheckTime = DateTime.Now.AddDays(new Repositories.Helpers.GeneralFunctions().GetDaysCheckForBenefits() * -1);

        //        // take the transcation join by the type of the benfits id that changed in the last 24 hours
        //        var logsOfBenefits = logDB.EntityChangeLog.
        //       Join(logDB.EntityLogTypes, entity => entity.EntityLogTypeId, type => type.EntityLogTypeId,
        //       (entity, type) => new { entity, type })
        //           .Where(m => m.entity.Timestamp > startCheckTime && CategoriesNumberInts.Contains(m.entity.ItemId))
        //           .Select(m => new EntityLogsAlias()
        //           {
        //               CategoryNumber = m.entity.ItemId,
        //               Alias = m.type.Alias
        //           }).ToList();


        //        foreach (var item in CategoriesNumberInts)
        //        {
        //            var change = new FieldsExternalEdit();
        //            change.CategoryNumber = item;
        //            change.IsHeaderChanged = logsOfBenefits.Any(r => r.CategoryNumber == item && r.Alias == "ContentEditor.Settings.CategoryName");
        //            change.IsDescriptionChanged = logsOfBenefits.Any(r => r.CategoryNumber == item && r.Alias == "ContentEditor.CategoryDescription");
        //            change.IsShortDescriptionChanged = logsOfBenefits.Any(r => r.CategoryNumber == item && r.Alias == "ContentEditor.ShortCategoryDescription");

        //            logBenefitChanges.Add(change);
        //        }

        //        return logBenefitChanges;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Logger.Error($"Error in GetChangeLog, Error message: {ex.Message}");
        //        throw ex;
        //    }
        //}



    }
}
