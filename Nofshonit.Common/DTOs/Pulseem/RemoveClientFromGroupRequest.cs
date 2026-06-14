using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class RemoveClientFromGroupRequest
    {
        public List<int> GroupIDs { get; set; }
        public List<string> Cellphone { get; set; }
        public List<string> Email { get; set; }
    }
}
