using Nofshonit.Common.DTOs.Business;
using System;
using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Product
{
    public class VariantDTO
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime EndDate { get; set; }
        public Boolean IsEmpty { get; set; }
        public Boolean IsSendToFriend { get; set; }
        public int OrderLimit { get; set; }
        public int MonthlyLimit { get; set; }
        public int YearlyLimit { get; set; }
        public int GeneralLimit { get; set; }
        public decimal IrgunPrice { get; set; }

        public decimal? Price { get; set; }
        public int BenefitTypeId { get; set; }
        public int? GiftCardValue { get; set; }
        public int RedimTypeId { get; set; }
        public string RedimTypeName { get; set; }
        public decimal KupaPrice { get; set; }
        public List<BusinessDTO> Business { get; set; }

        public bool? IsCampaign { get; set; }
        public int? BusinessSubTypeId { get; set; }
        public bool isExternalCoupon { get; set; }
    }

}
