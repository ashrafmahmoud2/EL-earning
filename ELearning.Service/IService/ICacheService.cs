using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Service.IService;
public interface ICacheService
{
    Task SetCacheAsync<T>(string key, T data, TimeSpan expiration);
    Task<T> GetCacheAsync<T>(string key);
    Task RemoveCacheAsync(string key);
}

