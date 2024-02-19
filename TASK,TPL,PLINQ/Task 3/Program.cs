var task1 = Task.Factory.StartNew(() =>
{
    Thread.Sleep(2000);
    Console.WriteLine("Task 1 completed");
});
Console.WriteLine("Before task1.Wait()");
// task1 tamamlanana kadar main thread bekleyecek.
task1.Wait();
Console.WriteLine("After task1.Wait()");
var task2 = Task.Factory.StartNew(() =>
{
    Thread.Sleep(3000);
    Console.WriteLine("Task 2 completed.");
});
var task3 = Task.Factory.StartNew(() =>
{
    Thread.Sleep(5000);
    Console.WriteLine("Task 3 completed.");
});
Console.WriteLine("Before Task.WaitAll(task2,task3)");
// task2 ve task3 tamamlanana kadar main thread bekleyecek.
Task.WaitAll(task2, task3);
Console.WriteLine("After Task.WaitAll(task2,task3)");

var task4 = Task.Factory.StartNew(() =>
{
    Thread.Sleep(1500);
    Console.WriteLine("Task 4 completed");
});
var task5 = Task.Factory.StartNew(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("Task 5 completed");
});
// task4 veya task5 ten herhangi biri tamamlanana kadar main thread bekleyecek.
Task.WaitAny(task4, task5);
Console.WriteLine("End of program");
Console.ReadKey();