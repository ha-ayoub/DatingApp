using DatingApp.Application.Common.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace DatingApp.Infrastructure.Services.Cache;

public class RedisCacheService(IConnectionMultiplexer redis) : ICacheService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var val = await _db.StringGetAsync(key);
        return val.HasValue ? JsonSerializer.Deserialize<T>((string)val!) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry ?? TimeSpan.FromHours(1));
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
        => _db.KeyDeleteAsync(key).ContinueWith(_ => { });
}
