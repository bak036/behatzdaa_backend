using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class WalletLoadMoney
    {
        public int Id { get; set; }
        public decimal WalletPresentCredit { get; set; }
        public long? WalletId { get; set; }
        public bool? Active { get; set; }
        public bool? IsLoadMoney { get; set; }
        public int? WalletPresentMore { get; set; }
        public bool? IsLeverageCreadit { get; set; }
        public bool? IsLeverageOrg { get; set; }
        public bool? IsLevarageMore { get; set; }
        public string CheckLeumiRespone { get; set; }
        public bool? IsFreeTextCharge { get; set; }
        public int? MaxSumInWalletForMonth { get; set; }
        public int? MaxWeeklyDeposit { get; set; }
        public int? MaxDailyDeposit { get; set; }
        public string BackgroundImageName { get; set; }
    }
}
