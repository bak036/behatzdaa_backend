namespace Nofshonit.Common.DTOs.Cards
{
    public class WalletDTO
    {

        public string WalletName { get; set; }
        public string WalletID { get; set; }
        public double WalletBalance { get; set; }
        public string LoadedThisMonth { get; set; }
        public LoadingModeDTO LoadingMode { get; set; }
        public string DiscountMode { get; set; }
        public decimal DiscountRate { get; set; }
        public int AmountToWallet { get; set; }

        public string MaxBalance { get; set; }
        public int MaxDepositForMonth { get; set; }
        public string MaxDeposit { get; set; }
        public double MaxAmountToLoad { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string IsLoadMoney { get; set; }
        public bool IsSpecialPaidWallet { get; set; }

    }
}
