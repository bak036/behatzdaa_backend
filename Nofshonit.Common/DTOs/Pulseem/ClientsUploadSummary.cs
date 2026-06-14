using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Common.DTOs.Pulseem
{
    public class ClientsUploadSummary
    {
        public int totalValidUploadedRecords { get; set; }
        public int totalDuplicates { get; set; }
        public int totalInvalidOrEmptyAddresses { get; set; }
        public int totalRecords { get; set; }
        public int invalidOrEmptyEmails { get; set; }
        public int existingEmails { get; set; }
        public int duplicateEmails { get; set; }
        public int invalidOrEmptyCellphones { get; set; }
        public int existingCellphones { get; set; }
        public int duplicateCellphones { get; set; }
    }

}
