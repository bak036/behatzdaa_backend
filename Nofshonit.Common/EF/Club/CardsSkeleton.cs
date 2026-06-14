using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class CardsSkeleton
    {
        public long CskeletonId { get; set; }
        public int RcnId { get; set; }
        public int SerieId { get; set; }
        public string CardNumber { get; set; }
        public int SequentialNum { get; set; }
        public short Cvv { get; set; }
        public short Track2Cvv { get; set; }
        public int? Hrid { get; set; }
        public long? CardId { get; set; }
        public bool? SlinkUsed { get; set; }
        public int? Amount { get; set; }
    }
}
