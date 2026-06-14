using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class SetClientStatusResponse
    {
        public int emailStatusChangesTotal { get; set; }
        public int cellphoneStatusChangesTotal { get; set; }
        public List<EmailStatusChangesList> emailStatusChangesList { get; set; }
        public List<CellphoneStatusChangesList> cellphoneStatusChangesList { get; set; }
        public string sessionId { get; set; }
        public string status { get; set; }
        public object message { get; set; }
    }
}
