using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class ClubCreditCardLog
    {
        public int Id { get; set; }
        public short? CodeType { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Last4Digit { get; set; }
        public string CardOwnerId { get; set; }
        public int? LcClubId { get; set; }
        public string MemberId { get; set; }
        public bool? IsUpdateFromFile { get; set; }
    }
}
