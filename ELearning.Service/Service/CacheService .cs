using ELearning.Service.IService;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Service.Service;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetCacheAsync<T>(string key, T data, TimeSpan expiration)
    {
         if (data == null)
    {
        throw new ArgumentNullException(nameof(data), "Cannot cache a null object.");
    }

        var serializedData = JsonConvert.SerializeObject(data);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        await _cache.SetStringAsync(key, serializedData, options);
    }

    public async Task<T> GetCacheAsync<T>(string key)
    {
        var cachedData = await _cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cachedData))
        {
            return default;
        }
        return JsonConvert.DeserializeObject<T>(cachedData);
    }

    public async Task RemoveCacheAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

}

