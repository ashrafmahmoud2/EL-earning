using ELearning.Data.Abstractions.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Service.IService;
public interface ICacheService
{
    Task<Result> SetCacheAsync<T>(string key, T data, TimeSpan expiration);
    Task<Result<T>> GetCacheAsync<T>(string key);
    Task<Result> RemoveCacheAsync(string key);
}

