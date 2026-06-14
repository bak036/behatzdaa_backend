using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Cards
{
    
    public class CardWalletsDTO
    {
        public Properties1 properties { get; set; }
        public List<Row1> rows { get; set; }
    }

    public class Properties1
    {
        public List<WalletData> Data { get; set; }
    }

    public class WalletData
    {
        public string MemberID { get; set; }
        public string TZ { get; set; }
        public string EmployeeNum { get; set; }
        public string WalletID { get; set; }
        public string OrganizationID { get; set; }
        public string Name { get; set; }
        public int QuickLoadMode { get; set; }
        public string Last4Digits { get; set; }
        public string LoadedThisMonth { get; set; }
        public int IsLoadAllowed { get; set; }
        public string DiscountMode { get; set; }
        public decimal DiscountRate { get; set; }
        public string MaxBalance { get; set; }
        public string MaxDeposit { get; set; }
        public string IsLoadMoney { get; set; }
        public double Balance { get; set; }
        public double MaxAmountToLoad { get; set; }
        public string BackgroundImageName { get; set; }
    }

    public class Row1
    {
        public string ErrorID { get; set; }
    }


}
