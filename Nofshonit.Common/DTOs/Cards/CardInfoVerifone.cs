using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Cards
{
    public class CardInfoVerifone
    {
        public string CardNumber { get; set; }
        public short CVV { get; set; }
        public int? VerID { get; set; }
        public int SerieID { get; set; }
        public int SequentialNum { get; set; }

    }
}
