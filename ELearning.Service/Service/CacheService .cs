using ELearning.Data.Abstractions.ResultPattern;
using ELearning.Data.Errors;
using ELearning.Service.IService;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ELearning.Service.Service;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<Result> SetCacheAsync<T>(string key, T data, TimeSpan expiration)
    {
        try
        {
            if (data == null)
            {
                return Result.Failure(CashErrors.NullData);
            }

            var serializedData = JsonConvert.SerializeObject(data);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _cache.SetStringAsync(key, serializedData, options);
            return Result.Success();
        }
        catch (Exception ex)
        {
            // Log exception if necessary
            return Result.Failure(CashErrors.ErrorConnectingToRedis);
        }
    }

    public async Task<Result<T>> GetCacheAsync<T>(string key)
    {
        try
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedData))
            {
                return Result.Failure<T>(CashErrors.NotFound);
            }

            var deserializedData = JsonConvert.DeserializeObject<T>(cachedData);
            if (deserializedData == null)
            {
                return Result.Failure<T>(CashErrors.DeserializationFailed);
            }

            return Result.Success(deserializedData);
        }
        catch (Exception ex)
        {
            // Log exception if necessary
            return Result.Failure<T>(CashErrors.ErrorConnectingToRedis);
        }
    }

    public async Task<Result> RemoveCacheAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            return Result.Success();
        }
        catch (Exception ex)
        {
            // Log exception if necessary
            return Result.Failure(CashErrors.ErrorConnectingToRedis);
        }
    }
}
