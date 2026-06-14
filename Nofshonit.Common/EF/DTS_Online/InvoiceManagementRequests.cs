using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.EF.DTS_Online
{
	public partial class InvoiceManagementRequests
	{
		public int ID { get; set; }
		public DateTime DateAdded { get; set; }
		public int OrgId { get; set; }
		public int StatusId { get; set; }
		public string Exception { get; set; }
		public int Attempts { get; set; }
		public string InvoiceRequest { get; set; }
	}
}
