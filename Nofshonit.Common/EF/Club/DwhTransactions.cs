using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.Club
{
    public partial class DwhTransactions
    {
        public int RowId { get; set; }
        public long? RecordId { get; set; }
        public string TableName { get; set; }
        public string DataBaseName { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string TerminalId { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public DateTime? OtherTransactionDateTime { get; set; }
        public double? Quantity { get; set; }
        public double? TransferPrice { get; set; }
        public double? CostPriceDiscount { get; set; }
        public double? SalePrice { get; set; }
        public double? SalePriceDiscount { get; set; }
        public double? NetSalePrice { get; set; }
        public double? CancellationFee { get; set; }
        public DateTime? Expired { get; set; }
        public long? ReferenceNumber { get; set; }
        public long? ReferenceGroupNumber { get; set; }
        public DateTime? Created { get; set; }
        public int? CreditCardHolder { get; set; }
        public long? CreditCardId { get; set; }
        public int? RegistrationNumber { get; set; }
        public string MerchantId { get; set; }
        public string BranchId { get; set; }
        public double? CostPrice { get; set; }
        public double? Vat { get; set; }
    }
}
