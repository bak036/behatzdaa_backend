using System;

namespace Nofshonit.Common.DTOs.Cards
{
    public class CardInfoDTO
    {
        public DateTime? ActivationTime { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public byte CardStatus { get; set; }

    }
}
