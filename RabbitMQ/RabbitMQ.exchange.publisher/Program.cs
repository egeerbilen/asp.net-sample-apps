using RabbitMQ.Client;
using System.Text;

// RabbitMQ bağlantısı için bir fabrika oluşturuluyor
var factory = new ConnectionFactory() { HostName = "localhost" };

// Bağlantıyı ve kanalı oluşturmak ve kullanmak için using blokları kullanılıyor
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    // Direct Exchange'i tanımla
    channel.ExchangeDeclare(exchange: "direct_exchange", type: ExchangeType.Direct);

    // Sonsuz bir döngü içinde kullanıcıdan mesaj ve routing key al
    while (true)
    {
        Console.Write("Enter message: ");
        string message = Console.ReadLine();

        Console.Write("Enter routing key (info, warning, error): ");
        string routingKey = Console.ReadLine();

        // Mesajı byte dizisine çevir
        var body = Encoding.UTF8.GetBytes(message);

        // Mesajı Direct Exchange'e belirtilen routing key ile gönder
        channel.BasicPublish(exchange: "direct_exchange",
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body);

        // Kullanıcıya gönderilen mesajı ve kullanılan routing key'i ekrana yazdır
        Console.WriteLine($" [x] Sent: {message} with routing key: {routingKey}");
    }
}
