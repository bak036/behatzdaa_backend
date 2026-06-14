using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class Nets
    {
        public int NetsCardId { get; set; }
        public int NetId { get; set; }
        public string NetName { get; set; }
        public string NetShortName { get; set; }
        public string NetDb { get; set; }
        public string NetOrdersData { get; set; }
        public string CardTamplate { get; set; }
        public string IdcodeTemplate { get; set; }
        public bool? OnlineActive { get; set; }
        public string Rem { get; set; }
        public string NetLoginData { get; set; }
        public string NetOrdersWebServiceTransaction { get; set; }
        public bool? IsImplementActive { get; set; }
        public bool? IsAtractionsActive { get; set; }
        public int? HomeNet { get; set; }
        public bool? IsTitanActive { get; set; }
        public bool? PaymentCard { get; set; }
        public byte? Cvv { get; set; }
        public bool? IsSogodActive { get; set; }
        public bool? IsFrsactive { get; set; }
        public byte? StartIndexCardNumber { get; set; }
        public byte? LengthCardNumber { get; set; }
        public bool? Ivrorder { get; set; }
        public bool? IsMultiCard { get; set; }
        public bool? IslcCard { get; set; }
        public bool? IsGazitActive { get; set; }
        public byte? SortGazit { get; set; }
        public byte? SortAtractions { get; set; }
        public bool? IsatrCamp { get; set; }
        public bool? IsPraxell { get; set; }
    }
}
