static async Task<int> MyAsyncMethod()
{
    Console.WriteLine("Asenkron metot başlıyor...");

    // Bir süre bekleyen asenkron bir iş yap
    await Task.Delay(2000);

    Console.WriteLine("Asenkron metot tamamlandı.");

    // Bir sonuç döndür
    return 42;
}


// Asenkron metodu çağırarak Task oluştur
Task<int> task = MyAsyncMethod();

// Task tamamlandığında bekle ve sonucu al
int result = await task;

// Sonucu ekrana yazdır
Console.WriteLine("Task tamamlandı. Sonuç: " + result);
