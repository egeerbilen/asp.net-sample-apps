using RedisSample.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisSample.Services;

public class BaseCacheService : IBaseCacheService // ICacheService önbellekleme işlemlerini gerçekleştiren bir servisin temel metodlarını ve özelliklerini tanımlar. 
{
    // IConnectionMultiplexer, Redis sunucusuna bağlantı yönetimini sağlayan bir arabirimdir.
    private readonly IConnectionMultiplexer _redisCon;

    // IDatabase, Redis önbellek servisine erişim sağlayan bir arabirimdir.
    private readonly IDatabase _cache;
    private TimeSpan ExpireTime => TimeSpan.FromDays(1); // expire süresi ekledik 1 gün

    // redisCon parametresi, Redis sunucusuna bağlantıyı yöneten IConnectionMultiplexer nesnesini alır.
    public BaseCacheService(IConnectionMultiplexer redisCon)
    {
        // _redisCon alanına parametre olarak gelen IConnectionMultiplexer nesnesi atanır.
        _redisCon = redisCon;

        // _cache alanına, _redisCon üzerinden alınan veritabanını (IDatabase) atanır.
        _cache = redisCon.GetDatabase(1); // GetDatabase() boş olursa default olarak db0 ı alacaktır
    }

    // Verilen anahtarla ilişkilendirilmiş öğeyi Redis önbelleğinden silen metot.
    // key parametresi, silinecek öğenin anahtarını temsil eder.
    public async Task Clear(string key)
    {
        // _cache üzerinden KeyDeleteAsync metodu kullanılarak Redis önbelleğinden öğe silinir.
        await _cache.KeyDeleteAsync(key);
    }

    // Tüm Redis veritabanlarını temizleyen metot.
    public void ClearAll()
    {
        // _redisCon üzerinden tüm endpoints (uç noktalar) alınır.
        var endpoints = _redisCon.GetEndPoints(true);

        // Her bir endpoint için ilgili sunucu alınır ve tüm veritabanları temizlenir.
        foreach (var endpoint in endpoints)
        {
            var server = _redisCon.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }

    // Belirtilen anahtarla ilişkilendirilmiş öğeyi Redis önbelleğinden alır veya ekler.
    // key parametresi, öğenin anahtarını temsil eder.
    // action parametresi, öğe önbellekte bulunamadığında çağrılacak olan asenkron işlemi temsil eder.
    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
    {
        // _cache üzerinden StringGetAsync metodu kullanılarak öğe alınır.
        var result = await _cache.StringGetAsync(key);

        // Eğer öğe önbellekte bulunamazsa, action fonksiyonu çağrılarak öğe oluşturulur ve önbelleğe eklenir.
        if (result.IsNull)
        {
            result = JsonSerializer.SerializeToUtf8Bytes(await action());
            await SetValueAsync(key, result);
        }

        // Öğe deserialize edilerek döndürülür.
        return JsonSerializer.Deserialize<T>(result);
    }

    // Belirtilen anahtarla ilişkilendirilmiş öğenin değerini asenkron olarak alır.
    // key parametresi, öğenin anahtarını temsil eder.
    public async Task<string> GetValueAsync(string key)
    {
        // _cache üzerinden StringGetAsync metodu kullanılarak öğe değeri alınır.
        return await _cache.StringGetAsync(key);
    }

    // Belirtilen anahtar ve değeri asenkron olarak Redis önbelleğine ekler.
    // key parametresi, öğenin anahtarını temsil eder.
    // value parametresi, öğenin değerini temsil eder.
    public async Task<bool> SetValueAsync(string key, string value)
    {
        // _cache üzerinden StringSetAsync metodu kullanılarak öğe önbelleğe eklenir.
        return await _cache.StringSetAsync(key, value, ExpireTime);
    }

    // Belirtilen anahtarla ilişkilendirilmiş öğeyi Redis önbelleğinden alır veya ekler.
    // key parametresi, öğenin anahtarını temsil eder.
    // action parametresi, öğe önbellekte bulunamadığında çağrılacak olan işlemi temsil eder.
    public T GetOrAdd<T>(string key, Func<T> action) where T : class
    {
        // _cache üzerinden StringGet metodu kullanılarak öğe alınır.
        var result = _cache.StringGet(key);

        // Eğer öğe önbellekte bulunamazsa, action fonksiyonu çağrılarak öğe oluşturulur ve önbelleğe eklenir.
        if (result.IsNull)
        {
            result = JsonSerializer.SerializeToUtf8Bytes(action());
            _cache.StringSet(key, result, ExpireTime);
        }

        // Öğe deserialize edilerek döndürülür.
        return JsonSerializer.Deserialize<T>(result);
    }
}