using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CardTest
    {
        public CardTest()
        {
            CardSellTest = new HashSet<CardSellTest>();
        }

        public int CardId { get; set; }
        public string CoomentA { get; set; }
        public string CoomentB { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public virtual ICollection<CardSellTest> CardSellTest { get; set; }
    }
}
