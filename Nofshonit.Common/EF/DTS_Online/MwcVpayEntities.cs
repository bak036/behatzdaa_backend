using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class MwcVpayEntities
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public int ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int WalletId { get; set; }
        public bool Checked { get; set; }
        public string Linked { get; set; }
    }
}
