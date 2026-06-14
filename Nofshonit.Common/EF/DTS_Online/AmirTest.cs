using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class AmirTest
    {
        public long SmsId { get; set; }
        public DateTime DateAdded { get; set; }
        public int? OrganizationId { get; set; }
        public string MemberId { get; set; }
        public long? CouponId { get; set; }
        public string SenderName { get; set; }
        public string Subscribers { get; set; }
        public string Message { get; set; }
        public int MessageLengh { get; set; }
        public byte DeliveryDelayInMinutes { get; set; }
        public byte ExpirationDelayInMinutes { get; set; }
        public byte? SmsType { get; set; }
        public byte SeveralAttempts { get; set; }
        public byte Priority { get; set; }
        public bool SmsSend { get; set; }
        public DateTime? SendTime { get; set; }
        public string ResultStatus { get; set; }
        public DateTime? ResultTime { get; set; }
        public DateTime? SendDate { get; set; }
        public int? SmsQueueUsersId { get; set; }
        public string SenderGuid { get; set; }
        public DateTime? GuidTime { get; set; }
    }
}
