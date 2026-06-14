using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Business
{
    public class BusinessSubBranchDTO
    {
        public long BusinessId { get; set; }
        public string SubBusinessName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public DateTime LastLogin { get; set; }
        public string TerminalNo { get; set; }
        public string TerminalNoDts { get; set; }
        public string SubBusinessEmail { get; set; }
        public DateTime? PasswordChanged { get; set; }
        public string BusinessNumber { get; set; }
        public DateTime? InsertDate { get; set; }
        public string OpenHours { get; set; }
        public int? BusinessModeId { get; set; }
        public string BusinessUniqueNumber { get; set; }
        public string BusinessTaxName { get; set; }
        public string GpsPointerLat { get; set; }
        public string GpsPointerLon { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone1 { get; set; }
        public string StoreFax { get; set; }
        public int? StoreCityId { get; set; }
        public string StoreKesher { get; set; }
        public string StoreEmail { get; set; }
        public string StorePhone2 { get; set; }
        public string WebSite { get; set; }
        public string StoreZipCode { get; set; }
        public string StoreStreetNumber { get; set; }
        public bool? IsBusinessOwner { get; set; }
        public int? BranchId { get; set; }
        public string Region { get; set; }
        public int RowNumberId { get; set; }
    }
}
