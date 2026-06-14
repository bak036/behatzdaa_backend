using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class AddClientsResponse
    {
        public string sessionId { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public ClientsUploadSummary clientsUploadSummary { get; set; }
    }
}
