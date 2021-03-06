﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Feign.Discovery;
using Microsoft.Extensions.Caching.Distributed;

namespace Feign.Cache
{
    class CacheProvider : ICacheProvider
    {
        public CacheProvider(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        IDistributedCache _distributedCache;

        public T Get<T>(string name)
        {
            var instanceData = _distributedCache?.Get(name);
            if (instanceData != null && instanceData.Length > 0)
            {
                return DeserializeFromCache<T>(instanceData);
            }
            return default(T);
        }

        public async Task<T> GetAsync<T>(string name)
        {
            var instanceData = await _distributedCache?.GetAsync(name);
            if (instanceData != null && instanceData.Length > 0)
            {
                return DeserializeFromCache<T>(instanceData);
            }
            return default(T);
        }

        public void Set<T>(string name, T value, TimeSpan? expirationTime)
        {
            _distributedCache?.Set(name, SerializeForCache(value), new DistributedCacheEntryOptions
            {
                //SlidingExpiration = expirationTime
                AbsoluteExpirationRelativeToNow = expirationTime
            });
        }

        public async Task SetAsync<T>(string name, T value, TimeSpan? expirationTime)
        {
            await _distributedCache?.SetAsync(name, SerializeForCache(value), new DistributedCacheEntryOptions
            {
                //SlidingExpiration = expirationTime
                AbsoluteExpirationRelativeToNow = expirationTime
            });
        }



        private static List<SerializableServiceInstance> MapToSerializable(IList<IServiceInstance> instances)
        {
            var inst = instances.Select(i => new SerializableServiceInstance(i));
            return inst.ToList();
        }

        private static byte[] SerializeForCache(object data)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, data);
                return stream.ToArray();
            }
        }

        private static T DeserializeFromCache<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return (T)new BinaryFormatter().Deserialize(stream);
            }
        }

    }
}
