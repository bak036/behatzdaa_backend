using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Cards
{
    public static class LoadWalletHelper
    {
        public static Dictionary<string /*CardNumber_WalletID*/, DateTime> DicCardsInLoadProcess = new Dictionary<string, DateTime>();
        public static Dictionary<string /*CardNumber_WalletID*/, DateTime> DicCardsInLoadProcessForCancel = new Dictionary<string, DateTime>();

    }
}
