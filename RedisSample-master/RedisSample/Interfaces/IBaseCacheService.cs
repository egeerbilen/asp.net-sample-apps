namespace RedisSample.Interfaces;

public interface IBaseCacheService
{
    // Belirtilen anahtarla ilişkilendirilmiş öğenin değerini asenkron olarak alır.
    // key parametresi, öğenin anahtarını temsil eder.
    Task<string> GetValueAsync(string key);

    // Belirtilen anahtar ve değeri asenkron olarak Redis önbelleğine ekler.
    // key parametresi, öğenin anahtarını temsil eder.
    // value parametresi, öğenin değerini temsil eder.
    Task<bool> SetValueAsync(string key, string value);

    // Belirtilen anahtarla ilişkilendirilmiş öğeyi Redis önbelleğinden alır veya ekler.
    // key parametresi, öğenin anahtarını temsil eder.
    // action parametresi, öğe önbellekte bulunamadığında çağrılacak olan asenkron işlemi temsil eder.
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class;

    // Belirtilen anahtarla ilişkilendirilmiş öğeyi Redis önbelleğinden alır veya ekler.
    // key parametresi, öğenin anahtarını temsil eder.
    // action parametresi, öğe önbellekte bulunamadığında çağrılacak olan işlemi temsil eder.
    // GetOrAdd fonksiyonu ile cache'den veri çekildiğinde veri null ise aynı zamanda set işlemi de yapılarak kod tekrarı önlenir.
    T GetOrAdd<T>(string key, Func<T> action) where T : class;

    // Belirtilen anahtarla ilişkilendirilmiş öğeyi Redis önbelleğinden silen metot.
    // key parametresi, silinecek öğenin anahtarını temsil eder.
    Task Clear(string key);

    // Tüm Redis veritabanlarını temizleyen metot.
    void ClearAll();
}