using System;
using System.Collections.Generic;
using System.Text;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;

namespace Nofshonit.Common.Utils
{
    public static class LastUpdateMemberUtil
    {

        private static int updateDetailsMonthExpiry = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<int>(ConfigurationKey.UpdateDetailsMonthExpiry);

        private static int passwordMonthExpiry = ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<int>(ConfigurationKey.UpdatePasswordMonthExpiry);

        public static ENeedUpdateOrFinishRegistration? LastUpdateMemberGenerator(DateTime? lastUpdateMember, DateTime? lastUpdatePasswod, string userPassword )
        {            
            if (lastUpdateMember == null)
                return ENeedUpdateOrFinishRegistration.FinishRegistration;



            if (GetMonths(lastUpdateMember, DateTime.Now) >= updateDetailsMonthExpiry)
                return ENeedUpdateOrFinishRegistration.NeedUpdate;

            return null;
        }

        public static ENeedUpdateOrFinishRegistration? LastUpdateMemberGenerator(DateTime? lastUpdateMember)
        {
            if (lastUpdateMember == null   )
                return ENeedUpdateOrFinishRegistration.FinishRegistration;

            if (GetMonths(lastUpdateMember, DateTime.Now) >= updateDetailsMonthExpiry)
                return ENeedUpdateOrFinishRegistration.NeedUpdate;

            return null;
        }


        private static int GetMonths(DateTime? fromDate, DateTime toDate)
        {
            if (fromDate >= toDate)
                return 0;

            var months = ((toDate.Year * 12) + toDate.Month) - ((fromDate.Value.Year * 12) + fromDate.Value.Month);
            if (toDate.Day >= fromDate.Value.Day)
                months++;

            return months;
        }
    }
}
