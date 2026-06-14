using System;
using System.Collections.Generic;

namespace TicketsHubRepository.EF.TicketsHub
{
    public partial class Setup
    {
        public int OrganizationId { get; set; }
        public bool IsTicketsHub { get; set; }
        public string CssFilePath { get; set; }
        public bool IsCoins { get; set; }
        public string CoinsNickName { get; set; }
        public string JsFileForIntegrationAfterOrder { get; set; }
        public int DaysBeforeShowToAllowCancel { get; set; }
    }
}
