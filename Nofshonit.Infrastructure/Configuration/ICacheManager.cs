using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Nofshonit.Infrastructure.Configuration
{
    public interface ICacheManager
    {
        object Get(object key);
        bool Exists(object key);
        void Set(object key, object obj);
        void Set(object key, object obj, TimeSpan duration);
        void Remove(object key);
        void Init(IMemoryCache memoryCache);
        int GenerateKey(object[] objArr, [CallerMemberName] string memberName = "");
    }
}
