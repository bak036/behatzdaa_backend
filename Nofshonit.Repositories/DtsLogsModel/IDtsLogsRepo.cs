using Nofshonit.Common.DTOs;
using Nofshonit.Common.EF.DTS_Logs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Repositories.DtsLogsModel
{
	public interface IDtsLogsRepo
	{
		void AddLog(ApiLogs log);
        bool AddReferralHistory(ReferralHistory log);
    }
}
