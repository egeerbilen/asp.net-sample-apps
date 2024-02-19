
static void DoWork(int id, int milliseconds)
{
    Console.WriteLine($"Başlatılan İş {id}");

    // Belirtilen süre boyunca işlemi simüle ediyoruz
    Thread.Sleep(milliseconds);

    Console.WriteLine($"İş {id} tamamlandı");
}


// İki adet Task oluşturuluyor
Task task1 = Task.Run(() => DoWork(1, 500));
Task task2 = Task.Run(() => DoWork(2, 2000));

// İki Task'in de tamamlanmasını bekliyoruz
Task.WaitAll(task1, task2);

Console.WriteLine("İşlemler tamamlandı.");