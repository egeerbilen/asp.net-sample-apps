using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

// RabbitMQ bağlantı bilgilerini içeren bir factory oluşturuluyor
var factory = new ConnectionFactory() { HostName = "localhost" };

// RabbitMQ bağlantısı oluşturuluyor
using var connection = factory.CreateConnection();
// Kanal (channel) oluşturuluyor
using var channel = connection.CreateModel();

// Exchange tipi headers olarak ayarlanıyor
channel.ExchangeDeclare("headers_exchange", ExchangeType.Headers);

// Rastgele oluşturulan kuyruk ismi
var queueName = channel.QueueDeclare().QueueName;

// Başlık (header) filtresi ayarlama
var headers = new Dictionary<string, object>
{
    { "type", "important" },
    { "priority", 5 }
};

// Kuyruğu headers_exchange ile bağlama ve başlık (header) filtresi belirleme
channel.QueueBind(queueName, "headers_exchange", "", headers);

Console.WriteLine(" [*] Mesajları bekliyor...");

// Consumer oluşturuluyor
var consumer = new EventingBasicConsumer(channel);

// Mesaj alındığında gerçekleşecek olaya abone olma
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Alınan Mesaj: {message}");
};

// Kuyruğa abone olma
channel.BasicConsume(queueName, true, consumer);

Console.WriteLine(" Çıkmak için herhangi bir tuşa basın.");
Console.ReadLine();
