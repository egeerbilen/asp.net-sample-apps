﻿
static int Sum(IEnumerable<int> source)
{
    int sum = 0;
    foreach (var item in source)
    {
        sum += item;
    }
    return sum;
}

var firstCollection = Enumerable.Range(1, 7000);
var secondCollection = Enumerable.Range(7000, 14000);
var thirdCollection = Enumerable.Range(14000, 21000);
// StartNew methodu int tipi ile çalıştığında method içerideki action' ın geriye integer bir değer dönmesini bekler.
var task1 = Task.Factory.StartNew<int>(() =>
{
    Console.WriteLine("firstCollectinon calculating on Task {0}", Task.CurrentId);
    return Sum(firstCollection);
});
// Action geriye zaten bir integer değer döndürdüğü için int tipini tanımlamak zorunda değiliz.
var task2 = Task.Factory.StartNew(() =>
{
    Console.WriteLine("secondCollection calculating on Task {0}", Task.CurrentId);
    return Sum(secondCollection);
});
// Lambda ile methodları aşağıdaki gibi çalıştırıp sonucunu alabiliriz.
var task3 = Task.Factory.StartNew(() => Sum(thirdCollection));

Console.WriteLine("Sum of firstCollection:{0}", task1.Result);
Console.WriteLine("Sum of secondCollection:{0}", task2.Result);
Console.WriteLine("Sum of thirdCollection:{0}", task3.Result);
Console.ReadKey();
