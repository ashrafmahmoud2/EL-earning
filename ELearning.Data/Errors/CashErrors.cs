using ELearning.Data.Abstractions.ResultPattern;
using Microsoft.AspNetCore.Http;

namespace ELearning.Data.Errors;

public record CashErrors
{
    public static readonly Error ErrorConnectingToRedis =
        new("Redis.ErrorConnectingToRedis", "Error connecting to Redis.", StatusCodes.Status500InternalServerError);

    public static readonly Error NullData =
        new("Cache.NullData", "Cannot cache a null object.", StatusCodes.Status400BadRequest);

    public static readonly Error NotFound =
        new("Cache.NotFound", "Cache entry not found.", StatusCodes.Status404NotFound);

    public static readonly Error DeserializationFailed =
        new("Cache.DeserializationFailed", "Failed to deserialize cached data.", StatusCodes.Status500InternalServerError);
}



