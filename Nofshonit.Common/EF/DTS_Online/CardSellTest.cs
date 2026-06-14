using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CardSellTest
    {
        public int CardSell { get; set; }
        public string CardSellComment { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }

        public virtual CardTest Card { get; set; }
        public virtual UserTest User { get; set; }
    }
}
