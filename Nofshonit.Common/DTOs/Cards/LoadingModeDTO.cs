using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.Cards
{
    public class LoadingModeDTO
    {
        public int IsLoadAllowed { get; set; }
        public int QuickLoadMode { get; set; }
        public string Last4Digits { get; set; }
    }
}
