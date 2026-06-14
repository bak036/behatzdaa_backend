using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class CardTypes
    {
        public byte CardType { get; set; }
        public string CardDescription { get; set; }
        public bool Gcdisplay { get; set; }
        public byte SortOrder { get; set; }
    }
}
