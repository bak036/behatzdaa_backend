using System;
using System.Collections.Generic;
using System.Linq;
using Nofshonit.Common.Constants;
using Nofshonit.Common.DTOs.Enums;
using Nofshonit.Common.EF.DTS_Online;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;

namespace Nofshonit.Common.FeaturesManagement
{
    public static class Feature
    {
        
        private static ICustomContainer Container
        {
            get
            {
                return ContainerManager.Container;
            }
        }

        private static ICacheManager Cache
        {
            get
            {
                return Container.Resolve<ICacheManager>();
            }
        }
        public static bool HasPermission(ELoginType loginType)
        {
            var feature = GetFeatureByLoginType(loginType);

            if (feature == null)
                return true;
            var currentOrganization = Container.Resolve<IContextManager>().CurrentOrganization();
            var featuresFlags = Cache.Get(CacheKeys.FeaturesFlags) as List<FeaturesFlags>;
            if (featuresFlags == null)
            {
                using (var context = new DTS_OnlineContext())
                {
                    //featuresFlags = context.FeaturesFlags.ToList();
                    Cache.Set(CacheKeys.FeaturesFlags, featuresFlags);
                }
            }
            var featureFromCache = featuresFlags.FirstOrDefault(f => f.FeatureKey == (int)feature);
            if (featureFromCache != null)
                return (bool)featureFromCache[currentOrganization.DBName];

            throw new Exception("There is a problem with Features list from Cache OR from DB");
        }

        public static EFeature? GetFeatureByLoginType(ELoginType loginType)
        {
            switch (loginType)
            {
                case ELoginType.Facebook:
                    return EFeature.FacebookLogin;
                case ELoginType.Google:
                    return EFeature.GoogleLogin;
                default:
                    return null;
            }
        }
    }
}
