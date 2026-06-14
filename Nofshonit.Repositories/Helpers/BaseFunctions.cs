using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Nofshonit.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Repositories.Helpers
{
    public class BaseFunctions
    {
        public static IMemoryCache _memoryCache;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public BaseFunctions(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }
        public BaseFunctions()
        {

        }

        //public CacheManager CacheManager = new CacheManager(_memoryCache);
    }
}
