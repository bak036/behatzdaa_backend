using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class OrganizationOps
    {
        public int Opid { get; set; }
        public string OpuserName { get; set; }
        public string OpuserPassword { get; set; }
        public DateTime PasswordChanged { get; set; }
        public string OprealId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Status { get; set; }
        public bool IsUpdate { get; set; }
        public bool? IsLoadFiles { get; set; }
        public bool IsCamp { get; set; }
        public bool IsUpdateCamp { get; set; }
        public bool? IsTradeManager { get; set; }
        public byte? CanApprove { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? LoginErrCount { get; set; }
        public int? ChangePasswordErrCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public byte? OfficeFormat { get; set; }
        public string Xmlparams { get; set; }
        public bool IsMembersNode { get; set; }
        public bool IsRequestNode { get; set; }
        public bool IsRealizationNode { get; set; }
        public bool IsCustomerRelationsNode { get; set; }
        public bool IsBenefitsNode { get; set; }
        public bool IsBusinessInformationNode { get; set; }
        public bool IsReportsNode { get; set; }
        public bool IsFilesNode { get; set; }
        public bool IsSupportNode { get; set; }
        public bool? IsMultiClub { get; set; }
        public bool? AllowCampaignReset { get; set; }
        public bool? AllowGeneric { get; set; }
        public bool? AllowCrm { get; set; }
        public bool? IsCrmManager { get; set; }
        public int? Optype { get; set; }
        public bool? IsSupport { get; set; }
        public bool? IsCustomerServiceManager { get; set; }
        public bool IsRecoveryOrder { get; set; }
        public bool? IsB2bManagment { get; set; }
        public bool IsAgent { get; set; }
    }
}
