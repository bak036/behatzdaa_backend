using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs
{
	public class BarcodePopupDetails
	{
        public string CategoryName { get; set; }
        public string ShortNameVar { get; set; }
        public int DigitalCodeType { get; set; }
        public string CardNumber { get; set; }
        public DateTime? LastImplementationDate { get; set; }
    }
}
