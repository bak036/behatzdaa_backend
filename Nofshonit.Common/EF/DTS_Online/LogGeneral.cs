using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class LogGeneral
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? FEnvId { get; set; }
        public int? FUserTypeId { get; set; }
        public int? Opid { get; set; }
        public string MemberId { get; set; }
        public int? OrganizationId { get; set; }
        public string Details { get; set; }
        public int? FTypeId { get; set; }
        public int? FTypeSubId { get; set; }
        public int? ResultsCode { get; set; }
        public string Ip { get; set; }
        public string Url { get; set; }
        public string StrDetails { get; set; }
        public string XmlDetails { get; set; }
        public DateTime DtCreatedDate { get; set; }

        public virtual LogGeneralEnv FEnv { get; set; }
        public virtual LogGeneralType FType { get; set; }
        public virtual LogGeneralTypeSub FTypeSub { get; set; }
        public virtual LogGeneralUserType FUserType { get; set; }
        public virtual LogGeneralResultsCode ResultsCodeNavigation { get; set; }
    }
}
