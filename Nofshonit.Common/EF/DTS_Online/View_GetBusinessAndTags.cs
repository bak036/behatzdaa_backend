using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nofshonit.Common.EF.DTS_Online
{
    public class View_GetBusinessAndTags
    {
        public int WalletID { get; set; }
        public int ChainID { get; set; }
        public string LogoURL { get; set; }
        public string BuisnessID { get; set; }
        public int TagID { get; set; }
        public string TagName { get; set; }
        public string ChainName { get; set; }
        public int TagType { get; set; }
        public int? OrganizationID { get; set; }
        public int TagSort { get; set; }
        public byte BusinessTagSort { get; set; }
        public string TagDescription { get; set; }
        public string TagImageBase64 { get; set; }
        public string SearchKeyWords { get; set; }
        public string WebSite { get; set; }
        public bool IsWebOnline { get; set; }


    }
}
