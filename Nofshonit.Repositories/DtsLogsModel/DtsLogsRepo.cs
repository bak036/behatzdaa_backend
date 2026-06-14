using DtsLoggger;
using Newtonsoft.Json;
using Nofshonit.Common.EF.DTS_Logs;
using System;
using System.Text;
using System.Threading.Tasks;
using Nofshonit.Common.EF.DTS_Online;

namespace Nofshonit.Repositories.DtsLogsModel
{
    class DtsLogsRepo : BaseRepo, IDtsLogsRepo
	{
		public void AddLog(ApiLogs log)
		{
            try
            {
                string curLog = JsonConvert.SerializeObject(log, Formatting.Indented).Replace("\"", " ").Replace(@"\"," ").Replace("{","").Replace("}","");              
                Logger.Info(curLog);
            }
            catch (Exception ex)
            {

            }
			//using (DTS_LogsContext _dbContext = new DTS_LogsContext())
			//{
			//	try
			//	{
			//		_dbContext.ApiLogs.Add(log);
			//		_dbContext.SaveChanges();
   //             }
			//	catch(Exception e)
			//	{
   //                 using (EventLog eventLog = new EventLog("Application"))
   //                 {
   //                     eventLog.Source = "Application";
   //                     eventLog.WriteEntry("Nofshonit Backend Logger Exception: " + e.ToString(), EventLogEntryType.Information, 101, 1);
   //                 }
   //             }
			//}

		}
        public bool AddReferralHistory(ReferralHistory log)
        {
            try
            {
                using (var context = new DTS_LogsContext())
                {
                    context.ReferralHistory.Add(log);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
