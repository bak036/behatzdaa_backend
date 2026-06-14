using Nofshonit.Common.Constants;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nofshonit.Common.Infrastructure;

namespace Nofshonit.Common.Infrastructure
{
	public static class MessagesUtil
	{
        private static int currOrgId = ContainerManager.Container.Resolve<IContextManager>().CurrentOrganization().OrgId;

		private static ICacheManager Cache 
		{
			get
			{
				return ContainerManager.Container.Resolve<ICacheManager>();
			}
		}

		public static string GetCreditGurdfriendlyErrorMessage(string chargeResult)
		{
			switch (chargeResult)
			{
				case "-1":
					return "לא ניתן לבצע תשלום שם המשתמש או הסיסמא לביצוע הסליקה חסרים";
				case "003":
					return " (003)התקשר לחברת האשראי";
				case "004":
					return "סירוב מחברת האשראי";
				case "006":
					return "מספר תעודת זהות או CVV שגויים";
				case "033":
					return "מספר הכרטיס אינו תקין";
				case "034":
					return "אין אישור לסוג עסקה או לסוג כרטיס זה במסוף זה";
				case "036":
					return "כרטיס פג תוקף";
				case "038":
					return "סכום העיסקה גדול מתקרה לכרטיס";
				case "039":
					return "מספר כרטיס אשראי לא תקין";
				case "405":
					return " (405)נא לפנות למנהל המערכת ולמסור את קוד התשובה";
				default:
					return " החיוב נכשל אנא בדוק שכל השדות נכונים ונסה שנית. קוד שגיאה " + chargeResult;
			}
		}
		public static List<Messages> GetMessagesByID(List<int> idList)
		{
			// Try To get from Cache
			// If Not exists then go to DTS_Online To get the table.
			// Return the itmes.
			List<Messages> messagesListToReturn = new List<Messages>();

			var messages = Cache.Get(CacheKeys.Messages) as List<Messages>;

			if (messages == null)
			{
				using (var context = new DTS_OnlineContext())
				{
					messages = context.Messages.ToList();
					Cache.Set(CacheKeys.Messages, messages);
				}
			}

			foreach (int id in idList)
			{
				var messagesFromCache = messages.FirstOrDefault(m => m.MessageId==id);
				if (messagesFromCache != null)
					messagesListToReturn.Add(messagesFromCache);
			}

			return messagesListToReturn;
		}

		public static List<Messages> GetMessagesByContext(string messageContext)
		{
			List<Messages> messagesListToReturn = new List<Messages>();

			var messages = Cache.Get(CacheKeys.Messages) as List<Messages>;

			if (messages == null)
			{
				using (var context = new DTS_OnlineContext())
				{
					messages = context.Messages.ToList();
					Cache.Set(CacheKeys.Messages, messages);
				}
			}


			messagesListToReturn = messages.FindAll(m => m.MessageContext != null 
													&& m.MessageContext.Equals(messageContext)
													&& m.OrganizationId == currOrgId);
			return messagesListToReturn;
		}

		public static List<Messages> GetMessagesByKey(List<int> keyList)
		{
			List<Messages> messagesListToReturn = new List<Messages>();

			var messages = Cache.Get(CacheKeys.Messages) as List<Messages>;

			if (messages == null)
			{
				using (var context = new DTS_OnlineContext())
				{
					messages = context.Messages.ToList();
					Cache.Set(CacheKeys.Messages, messages);
				}
			}

            foreach (int key in keyList)
            {
                var messagesFromCache = messages.FindAll(m => m.MessageKey == key
                                                         && m.OrganizationId == currOrgId);

                if (messagesFromCache != null)
                    messagesListToReturn.AddRange(messagesFromCache);
            }

            return messagesListToReturn;
		}
	}
}
