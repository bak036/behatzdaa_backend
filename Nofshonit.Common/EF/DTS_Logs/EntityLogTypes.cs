using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Logs
{
    public partial class EntityLogTypes
    {
        public EntityLogTypes()
        {
            EntityChangeLog = new HashSet<EntityChangeLog>();
        }

        public int EntityLogTypeId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string EntityIdDescription { get; set; }

        public virtual ICollection<EntityChangeLog> EntityChangeLog { get; set; }
    }
}
