using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class PraxellMedia
    {
        public long RepTransId { get; set; }
        public DateTime? ServerDataTime { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public string TransactionId { get; set; }
        public byte? ActivityId { get; set; }
        public int? Amount { get; set; }
        public string Currency { get; set; }
        public byte? DecimalCurrency { get; set; }
        public string OrderId { get; set; }
        public string CardNumber { get; set; }
        public string CardSerialNumber { get; set; }
        public string CardIssuerSn { get; set; }
        public string CardIssuerPnumber { get; set; }
        public string CardSeriesNumber { get; set; }
        public string CardBatchNumber { get; set; }
        public string ActionNetworkId { get; set; }
        public string ActionVendorPnumber { get; set; }
        public string ActionStoreId { get; set; }
        public string ActionStorePnumber { get; set; }
        public string ActionCashierId { get; set; }
        public string ActionTicketId { get; set; }
        public string ActionType { get; set; }
        public DateTime? CardExpirationDate { get; set; }
        public DateTime? VoidTransactionServerDateTime { get; set; }
        public DateTime? VoidTransactionPosDate { get; set; }
        public string VoidTransactionId { get; set; }
        public string VoidTransactionOrderId { get; set; }
        public string VoidTransactionActionNetworkId { get; set; }
        public string VoidTransactionActionVendorPnumber { get; set; }
        public string VoidTransactionActionStoreId { get; set; }
        public string VoidTransactionActionStorePnumber { get; set; }
        public string VoidTransactionActionCashierId { get; set; }
        public string VoidTransactionActionTicketId { get; set; }
        public string VoidTransactionActionType { get; set; }
        public long? FileId { get; set; }
    }
}
