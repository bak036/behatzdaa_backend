using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Cards
{
    public class LoadWalletToFuncDto
    {
        public string CardID { get; set; }
        public string CardExpiration { get; set; }
        public string CardNum { get; set; }
        public string PayerTZ { get; set; }
        public string LoadedThisMonth { get; set; }
        public string WalletLoadMoneyId { get; set; }
        public bool IsLeverage { get; set; }
        public double DiscountRate { get; set; }
        public double MaxInMonth { get; set; }
        public string IDMember { get; set; }
        public string Cvv { get; set; }
        public string SerieID { get; set; }
        public string CardsPrefix { get; set; }
        public string OrganizationID { get; set; }
        public string SeriesName { get; set; }
        public string OrganizationAllowedCardNumberByTZ { get; set; }
        public string OrganizationName { get; set; }
        public string DBName { get; set; }
        public string MaxSumInCard { get; set; }
        public string MaxSumInCardForMonth { get; set; }
        public string PaymentTerminalUserName { get; set; }
        public string PaymentTerminalPassword { get; set; }
        public string LoadMoneyTerminalNumber { get; set; }
        public string TradeSitePaymentTerminalNumber { get; set; }
        public double maxInMonth { get; set; }
        public double totalChargedCurrentMonth { get; set; }
    }
}
