using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs
{
    public class GetAutoCompleteResultsDTO
    {
        public long categoryNumber { get; set; }
        public string categoryName { get; set; }
        public string concatForSearch { get; set; } 

    }
}
