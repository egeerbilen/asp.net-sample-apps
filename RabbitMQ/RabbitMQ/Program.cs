using RabbitMQ.Client;
using System.Text;

// direk olarak mesajları kuyuğa gönderiyoruz

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://localhost:5672/%2F");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.QueueDeclare("hello-queue", true, false, false); // false diyerek benim kuyruğum memory de olsun dedik. True ise fiziksel olarak kaydedilir rabbitmq ya restart atsak bile durur
// farklı kanallardan bağlanmak için 3. property false olarak verildi
// subscriber gittiğinde de kuyruk silinmesin diye false olarak verdik

// ----------- 1 --------------
//string message = "Hello Worl! EGEEEE";

//var messageBody = Encoding.UTF8.GetBytes(message);

//channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

//Console.WriteLine("Message Gonderildi");


// ----------- 2 --------------

foreach (var item in Enumerable.Range(1, 50))
{
    string message = "Message:" + item;

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

    Console.WriteLine("Message Gonderildi" + item);
}


Console.ReadLine();