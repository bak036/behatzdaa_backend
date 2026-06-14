using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs
{
	public class LogDTO
	{
        public string QueryParams { get; set; }

        public string Body { get; set; } 

        public string Response { get; set; }

        public string ClientIp { get; set; } = string.Empty;

		public string ServerIp { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

		public string Exception { get; set; }

        public string MethodName { get; set; } = string.Empty;

        public int OrganizationId { get; set; } = 0;

        public int LogType { get; set; }
	}
}
