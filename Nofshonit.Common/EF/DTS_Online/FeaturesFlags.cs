using System;
using System.Collections.Generic;

namespace Nofshonit.Common.EF.DTS_Online
{
    public partial class FeaturesFlags
    {
        public bool? NofshonitSite { get; set; }

        public bool? Histadrut { get; set; }

        public int FeatureKey { get; set; }

        public string FeatureName { get; set; }

        public object this[string propertyName]
        {
            get
            {
                return this.GetType().GetProperty(propertyName).GetValue(this, null);
            }
            set
            {
                this.GetType().GetProperty(propertyName).SetValue(this, value, null);
            }
        }
    }
}
