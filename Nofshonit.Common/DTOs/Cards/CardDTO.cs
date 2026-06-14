using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Cards
{
    public class CardDTO
    {
        public int ErrorID { get; set; }
        public int MaxAmountToLoad { get; set; }
        public double Balance { get; set; }
       public string CardNumber { get; set; }
        public double moneyPercentageCancellationCommission { get; set; }
        public double numberOfDaysAllowingCancellation { get; set; }
        public double moneyMaxCommissionAmount { get; set; }
        public List<WalletDTO> Wallets { get; set; }

        public CardInfoDTO CardInfo { get; set; }

    }
}
