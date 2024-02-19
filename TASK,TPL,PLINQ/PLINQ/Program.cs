
//LINQ: Seri olarak çalışır, yani sorgular tek bir iş parçacığında sırayla işlenir.
//PLINQ: Paralel olarak çalışır, yani sorgular aynı anda birden fazla iş parçacığında çalışabilir.
// Unutmayın ki PLINQ kullanmanın her zaman performans artışı getirmediği durumlar olabilir ve dikkatlice kullanılmalıdır. Küçük veri setlerinde ve basit sorgularda, paralel işleme ek bir yük getirebilir. Büyük veri setleri ve karmaşık sorgular için daha uygun olabilir.

// Veri kümesi oluştur
string[] words = { "apple", "orange", "banana", "strawberry", "grape" };

// PLINQ kullanarak paralel sorgu
var parallelQuery = from word in words.AsParallel()
                    where word.Length >= 6
                    select word.ToUpper();

// Sonuçları ekrana yazdır
Console.WriteLine("Paralel PLINQ Sonuçları:");
foreach (var result in parallelQuery)
{
    Console.WriteLine(result);
}

// Seri LINQ sorgusu
var sequentialQuery = from word in words
                      where word.Length >= 6
                      select word.ToUpper();

// Sonuçları ekrana yazdır
Console.WriteLine("\nSeri LINQ Sonuçları:");
foreach (var result in sequentialQuery)
{
    Console.WriteLine(result);
}

Console.WriteLine("Bitti.");
Console.ReadLine();