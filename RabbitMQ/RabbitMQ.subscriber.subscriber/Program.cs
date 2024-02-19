using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// RabbitMQ bağlantısı için bir fabrika oluşturuluyor
var factory = new ConnectionFactory() { HostName = "localhost" };

// Bağlantıyı ve kanalı oluşturmak ve kullanmak için using blokları kullanılıyor
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    // Fanout Exchange'i tanımla
    channel.ExchangeDeclare(exchange: "fanout_exchange", type: ExchangeType.Fanout);

    // Oluşturulan kuyruğun adını al
    var queueName = channel.QueueDeclare().QueueName;

    // Kuyruğu Fanout Exchange'e bağla
    channel.QueueBind(queue: queueName, exchange: "fanout_exchange", routingKey: "");

    // Konsola beklemeye başlandığını yazdır
    Console.WriteLine(" [*] Waiting for messages. To exit press Ctrl+C");

    // Bir olay tüketici (consumer) oluştur
    var consumer = new EventingBasicConsumer(channel);

    // Mesaj alındığında gerçekleşecek olayı tanımla
    consumer.Received += (model, ea) =>
    {
        // Mesajın içeriğini al
        var body = ea.Body;

        // Mesaj içeriğini stringe çevir
        var message = Encoding.UTF8.GetString(body.ToArray());

        // Alınan mesajı konsola yazdır
        Console.WriteLine($" [x] Received: {message}");
    };

    // Mesajları tüketmeye başla
    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

    // Konsol uygulamasının kapatılmamasını sağlamak için bir giriş bekler
    Console.ReadLine();
}
