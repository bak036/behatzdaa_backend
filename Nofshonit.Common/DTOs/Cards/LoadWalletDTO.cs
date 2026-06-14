namespace Nofshonit.Common.DTOs.Cards
{
    public class LoadWalletDTO
    {

        public int IsLoadAllowed { set; get; }
        public int QuickLoadMode { set; get; }
        public int Last4Digits { set; get; }
        public double MaxAmountToLoad { set; get; }
        public string DiscountMode { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
