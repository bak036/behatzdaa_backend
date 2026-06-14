using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Nofshonit.Common.EF.DTS_Online
{
    [Table("View_GetBranchesByWalletId")]
    public class View_GetBranchesByWalletId
    {
        [Key]
        public Guid Id { get; set; }
        public int WalletID { get; set; }

        public string WalletName { get; set; }

        public int ChainID { get; set; }

        public string ChainName { get; set; }

        public int? BranchId { get; set; }

        public string BranchName { get; set; }

        public string BuisnessID { get; set; }

        public string StoreAddress { get; set; }

        public string StorePhone1 { get; set; }

        public string WebSite { get; set; }

    }
}
