namespace Nofshonit.Common.DTOs.Cards
{
    public class PayerDataDTO
    {
        public string WalletID { set; get; }
        public float AmountToCharge { set; get; }
        public float AmountToLoad { set; get; }
        public string PinCode { set; get; }
        public string PayerCardTZ { set; get; }
        public string PayerCardNumber { set; get; }
        public int PayerCardCVV { set; get; }
        public string PayerCardExpiresMonth { set; get; }
        public string PayerCardExpiresYear { set; get; }
        public bool SaveForQuickLoad { set; get; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"&WalletID={WalletID}&Amount=&AmountToCharge={AmountToCharge}&AmountToLoad={AmountToLoad}" +
                 $"&PayerCardTZ={PayerCardTZ}&PayerCardNumber={PayerCardNumber}&SaveForQuickLoad={SaveForQuickLoad}" +
                 $"&PayerCardCVV={PayerCardCVV}&PayerCardExpiresMonth={PayerCardExpiresMonth}&PayerCardExpiresYear={PayerCardExpiresYear}";
        }
    }
}
