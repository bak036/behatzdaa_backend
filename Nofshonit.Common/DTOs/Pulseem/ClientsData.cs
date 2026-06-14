using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class ClientsData
    {
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; } = "";
        public string telephone { get; set; } = "";
        public string cellphone { get; set; } = "";
        public string birthDate { get; set; }
        public bool needOptin { get; set; }
        public bool overwrite { get; set; }
    }
}
