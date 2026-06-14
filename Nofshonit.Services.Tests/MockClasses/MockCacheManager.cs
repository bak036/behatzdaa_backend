using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Nofshonit.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nofshonit.Services.Tests.MockClasses
{
	public class MockCacheManager : ICacheManager
	{
		private IMemoryCache _memoryCache;
		public MockCacheManager()
		{
			Init((new MemoryCache(new MemoryCacheOptions())));
		}
		void Init(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public object Get(object key)
		{
			return _memoryCache.Get(key);
		}

		public void Set(object key, object obj)
		{
			_memoryCache.Set(key, obj);
		}

		public void Set(object key, object obj, TimeSpan duration)
		{
			_memoryCache.Set(key, obj, duration);
		}
		public void Remove(object key)
		{
			_memoryCache.Remove(key);

		}
		public bool Exists(object key)
		{
			return _memoryCache.Get(key) != null;
		}

		void ICacheManager.Init(IMemoryCache memoryCache)
		{
			_memoryCache = (new MemoryCache(new MemoryCacheOptions()));
		}

		public int GenerateKey(object[] objArr ,[CallerMemberName] string memberName = "")
		{
			return $"{memberName}{JsonConvert.SerializeObject(objArr)}".GetHashCode();
		}

	}
}
