using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class SlinkRequests
    {
        public int RequestId { get; set; }
        public int? FileId { get; set; }
        public DateTime InsertDate { get; set; }
        public int UserId { get; set; }
        public int OrgId { get; set; }
        public int CardAmount { get; set; }
        public string Validity { get; set; }
        public int PopulationId { get; set; }
        public string EmailName { get; set; }
        public int SerieId { get; set; }
        public int? ProductType { get; set; }
        public int? WalletId { get; set; }
        public int? ReasonId { get; set; }
        public int? SumOfLoad { get; set; }
        public string ImageName { get; set; }
        public string BillingFile { get; set; }
        public string VariantId { get; set; }
        public decimal DiscountPercent { get; set; }
        public string MessageSender { get; set; }
        public string MessageBody { get; set; }
        public DateTime? MessageSendTime { get; set; }
        public int RequestStatus { get; set; }
        public bool FileApprove { get; set; }
        public string FileApprovedBy { get; set; }
        public DateTime? FileApproveDate { get; set; }
        public string ActivationCode { get; set; }
    }
}
