using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Business
    {
        public Business()
        {
            TagsBusiness = new HashSet<TagsBusiness>();
        }

        public string BuisnessId { get; set; }
        public short StoreType { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public int? StoreCityId { get; set; }
        public string StorePhone1 { get; set; }
        public string StorePhone2 { get; set; }
        public string StoreFax { get; set; }
        public string StoreKesher { get; set; }
        public string StoreEmail { get; set; }
        public string StoreUserName { get; set; }
        public string StorePassword { get; set; }
        public int? StoreSubnet { get; set; }
        public bool? Active { get; set; }
        public bool ActiveInWeb { get; set; }
        public bool LipsReader { get; set; }
        public bool? BalanceInWeb { get; set; }
        public string ForeignTerminal { get; set; }
        public string BusinessImg { get; set; }
        public byte? GeneralMimush { get; set; }
        public string LogoForPrinting { get; set; }
        public int LoginErrCount { get; set; }
        public DateTime PasswordChanged { get; set; }
        public DateTime? LastLogin { get; set; }
        public string TempStorePassword { get; set; }
        public short? Parking { get; set; }
        public short? CrippleAccess { get; set; }
        public short? Toilet { get; set; }
        public short? Restaurant { get; set; }
        public short? Tables { get; set; }
        public short? EquipmentRenting { get; set; }
        public short? Challenging { get; set; }
        public short? AboveAge { get; set; }
        public string LocationExplain { get; set; }
        public string BusinessDescription { get; set; }
        public bool AllowSoGood { get; set; }
        public short? BusinessSubTypeId { get; set; }
        public string BusinessDescriptionText { get; set; }
        public string ArrivalMapImage { get; set; }
        public decimal? MarketingCommission { get; set; }
        public byte? CinemaId { get; set; }
        public string StoreEmailBookkeeping { get; set; }
        public decimal? LiedPrice { get; set; }
        public bool? IsLied { get; set; }
        public bool? HaveSubBranch { get; set; }
        public short? RealizationType { get; set; }
        public string EmailCc { get; set; }
        public DateTime? LastReportDate { get; set; }
        public bool SendReports { get; set; }
        public bool AutoImplementaionAfterReport { get; set; }
        public bool? VatBusiness { get; set; }
        public DateTime? UpdateLoginErrCount { get; set; }
        public string TerminalNumber { get; set; }
        public int? MainTerminalNumberId { get; set; }
        public string Massage { get; set; }
        public DateTime? StoreCloseDate { get; set; }
        public DateTime? StoreStartDate { get; set; }
        public long? Idprovider { get; set; }
        public string TerminalUserName { get; set; }
        public string TerminalPassword { get; set; }
        public string BusinessNumber { get; set; }
        public DateTime? InsertDate { get; set; }
        public string WebSite { get; set; }
        public string OpenHours { get; set; }
        public int? BusinessModeId { get; set; }
        public string BusinessUniqueNumber { get; set; }
        public string BusinessTaxName { get; set; }
        public string GpsPointerLat { get; set; }
        public string GpsPointerLon { get; set; }
        public string StoreZipCode { get; set; }
        public string StoreStreetNumber { get; set; }
        public bool? IsBusinessOwner { get; set; }
        public bool? IsVerifoneChain { get; set; }
        public int? ChainId { get; set; }
        public string Region { get; set; }
        public bool? IsTicketsHub { get; set; }
        public string BusinessNumberNew { get; set; }
        public string Description { get; set; }
        public string MustToKnow { get; set; }
        public byte? SortOrderBusiness { get; set; }
        public byte? Kosher { get; set; }
        public string Logo { get; set; }

        public virtual BusinessModes BusinessMode { get; set; }
        public virtual MwcVpayChains Chain { get; set; }
        public virtual TitanCinemas Cinema { get; set; }
        public virtual Providers IdproviderNavigation { get; set; }
        public virtual ICollection<TagsBusiness> TagsBusiness { get; set; }
    }
}
