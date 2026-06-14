using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class DtsNewsletter
    {
        public int Id { get; set; }
        public string LinkCode { get; set; }
        public string XmlDetails { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImageName { get; set; }
        public bool? IsLoadMoney { get; set; }
        public int? OrgId { get; set; }
        public int? OrderId { get; set; }
        public string CardNumber { get; set; }
        public DateTime? OpenDate { get; set; }
        public string ActivationCode { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string CustomText { get; set; }
    }
}
