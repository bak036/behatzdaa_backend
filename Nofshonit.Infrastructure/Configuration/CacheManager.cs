using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nofshonit.Infrastructure.Configuration
{
    public class CacheManager : ICacheManager
    {
        private IMemoryCache _memoryCache;
        public CacheManager()
        {
            
        }
        void ICacheManager.Init(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object Get(object key)
        {
            return _memoryCache.Get(key);
        }
        public void Set(object key, object obj)
        {
            TimeSpan ts = TimeSpan.ParseExact(ContainerManager.Container.Resolve<IConfigurationManager>().GetConfigByValue<string>("CacheDuration"), "c", null);
            _memoryCache.Set(key, obj, ts );

        }

        public void Set(object key, object obj, TimeSpan duration)
        {

            _memoryCache.Set(key, obj, duration);

        }

        public void Remove(object key )
        {
            _memoryCache.Remove(key);

        }

        public bool Exists(object key)
        {
            return _memoryCache.Get(key) != null;
        }

        /// <summary>
        /// The method returns a cache key for a specific method
        /// objArr = OrganizationId + All input data
        /// </summary>
        /// <param name="objArr"></param>
        /// <returns></returns>
        public int GenerateKey( object[] objArr,[CallerMemberName] string memberName = "")
        {
           return $"{memberName}{JsonConvert.SerializeObject(objArr)}".GetHashCode();
        }
    }
}
