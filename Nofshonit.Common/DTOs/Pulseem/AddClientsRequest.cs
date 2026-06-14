using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class AddClientsRequest
    {
        public List<ClientsData> clientsData { get; set; }
        public List<int> groupIds { get; set; }
    }
}
