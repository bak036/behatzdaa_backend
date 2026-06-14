using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CrmDataTempToDelete
    {
        public int CrmId { get; set; }
        public DateTime DateCreate { get; set; }
        public int? OpIdCreate { get; set; }
        public int? OpIdCurrent { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public int OrgId { get; set; }
        public int CrmStatusId { get; set; }
        public int CrmSourceId { get; set; }
        public int CrmSeverityId { get; set; }
        public int? CrmSubjectId { get; set; }
        public string CrnEssence { get; set; }
        public string CrmDescription { get; set; }
        public string CrmResult { get; set; }
        public string PopulationType { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? CrmTypeId { get; set; }
        public bool? NotMember { get; set; }
        public string EmployeeNum { get; set; }
        public bool? HistorySource { get; set; }
    }
}
