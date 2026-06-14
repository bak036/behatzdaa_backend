using System;
using System.Collections.Generic;

namespace Nofshonit.Common.DTOs.Product
{



    public class VariantsByBenefitResponse
    {

        public int Status { get; set; }


        public string ErrorMessage { get; set; }


        public int? ErrorId { get; set; }


        public string MemberId { get; set; }


        public string BenefitId { get; set; }


        public string DtsId { get; set; }


        public List<Variant> Variants { get; set; } = new List<Variant>();




        public class Variant
        {

            public string Name { get; set; }


            public string BarCode { get; set; }


            public DateTime? ExpireDate { get; set; }

            public DateTime? EndDate { get; set; }

            public int IsEmpty { get; set; }


            public int IsSendToFriend { get; set; }


            public int OrderLimit { get; set; }


            public decimal? Price { get; set; }


            public int? Coins { get; set; }


            public int BenefitTypeId { get; set; }


            public int? GiftCardValue { get; set; }


            public int? GiftCardMinCoins { get; set; }


            public int? GiftCardMaxCoins { get; set; }


            public int RedimTypeId { get; set; }


            public string RedimTypeName { get; set; }


            public int KupaPrice { get; set; }


            public int IsFavorite { get; set; }

            public bool IsCampaign { get; set; }

            public int? BusinessSubTypeID { get; set; }
            public string MemberMonthlyLimitFormula { get; set; }
            public string MemberYearlyLimitFormula { get; set; }
            public string MemberGeneralLimitFormula { get; set; }
            public string IrgunPriceFormula { get; set; }
            public bool isExternalCoupon { get; set; }
        }

        public class Event
        {
            public string Name { get; set; }

            public string VenueName { get; set; }

            public int VenueId { get; set; }

            public DateTime? EventDate { get; set; }

            public string EventTime { get; set; }

            public int EventId { get; set; }

            public DateTime? ExpireDate { get; set; }

            public int IsSendToFriend { get; set; }

            public int OrderLimit { get; set; }

            public int BenefitTypeId { get; set; }

            public int RedimTypeId { get; set; }

            public string RedimTypeName { get; set; }

            public string IframeUrl { get; set; }
        }
    }
}
