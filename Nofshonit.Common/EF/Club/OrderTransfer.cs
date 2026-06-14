using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class OrderTransfer
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid TransferGuid { get; set; }
        public DateTime? TimeToSend { get; set; }
        public bool IsTransferSend { get; set; }
        public string FromMemberId { get; set; }
        public string FromMemberName { get; set; }
        public string ToMemberId { get; set; }
        public string ToMemberMobilePhone { get; set; }
        public string ToMemberEmail { get; set; }
        public string Blessing { get; set; }
        public string MediaImgUrl { get; set; }
        public long CategoryNumber { get; set; }
        public bool IsOpen { get; set; }
        public DateTime? OpenDate { get; set; }
        public bool MovedOn { get; set; }
        public string ToMemberName { get; set; }
    }
}
