static void Counter(CancellationToken token)
{
    int count = 1;
    while (true)
    {
        /* 
        if (token.IsCancellationRequested)
        { token.ThrowIfCancellationRequested() ile aynı işi yapar.
            throw new OperationCanceledException();
        }
        */
        // IsCancellationRequested propertysini kontrol edip döngünün bozulması mümkün ancak OperationCanceledException fırlatmadığımız sürece Task tamamlanmış olarak kabul edilir.
        token.ThrowIfCancellationRequested();
        Console.WriteLine(count++);
    }
}




var cancellationTokenSource = new CancellationTokenSource();
var task = new Task(() =>
{
    Counter(cancellationTokenSource.Token);
}, cancellationTokenSource.Token);
task.Start();

Console.ReadKey();
// Bu satırdan sonra token cancel edilip task içerisinden OperationCanceledException fırlatılacak, ancak exception Main thread üzerinde fırlamadığı için kullanıcıya henüz birşey yansımayacak.
cancellationTokenSource.Cancel();

try
{
    // Task içerisinde fırlayan exceptionlar .Wait methodu çalıştığında Main threade yansır. Ve bu exceptionlar AggregateException içerisine toplanır.
    task.Wait();
}
catch (AggregateException ex)
{
    // AggregateException içerisindeki Handle ile her exception durumu ayrı ayrı yönetilebilinir.
    // OperationCanceledException olması durumunda geriye handle edildi anlamına gelen true döndürüyoruz.
    // OperationCanceledException fırlaması durumunda kullanıcıya birşey yansımayacak ancak diğer exceptionları handle etmiyoruz.
    ex.Handle((exeption) =>
    {
        if (exeption is OperationCanceledException)
        {
            Console.WriteLine("İşlem iptal edildi.");
            return true;
        }
        return false;
    });
}
Console.ReadKey();