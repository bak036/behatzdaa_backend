using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class SetClientStatusRequest
    {
        public List<string> cellphoneAndEmailList { get; set; }
        public int newStatus { get; set; }
        public int emailOrCellphone { get; set; }
    }
}
