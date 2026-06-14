using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class ConfirmationTemplates
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Template { get; set; }
        public string Subject { get; set; }
        public string EmailFrom { get; set; }
        public string Cc { get; set; }
        public int? EmailTypeId { get; set; }
    }
}
