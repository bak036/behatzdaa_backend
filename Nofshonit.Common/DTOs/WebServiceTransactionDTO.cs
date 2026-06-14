using System;

namespace Nofshonit.Common.DTOs
{
    public class WebServiceTransactionDTO
    {

        public long ID { get; set; }
        public DateTime? Date { get; set; }
        public string Quantity { get; set; }
        public short? BussinesSubTypeID { get; set; }
        public int QuantityInt
        {
            get
            {
                {
                    if (string.IsNullOrEmpty(Quantity))
                        return 0;
                    else
                        return int.Parse(Quantity.Trim());
                }
            }
        }
        //fullbarcode
        public string ProductId { get; set; }
        public long OrderAsmachta { get; set; }

        public long? PaymentId { get; set; }
        public bool? Iscampaign { get; set; }
        public int? TransactionGroupLimitsId { get; set; }

        public bool? IsIgnoreBusinessSubTypeLimits { get; set; }

    }
}
