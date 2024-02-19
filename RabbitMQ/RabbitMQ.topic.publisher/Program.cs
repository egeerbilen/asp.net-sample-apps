using RabbitMQ.Client;
using System.Text;

// RabbitMQ bağlantısı için bir fabrika oluşturuluyor
var factory = new ConnectionFactory() { HostName = "localhost" };

// Bağlantıyı ve kanalı oluşturmak ve kullanmak için using blokları kullanılıyor
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    // Topic Exchange'i tanımla
    channel.ExchangeDeclare(exchange: "topic_exchange", type: ExchangeType.Topic);

    // Sonsuz bir döngü içinde kullanıcıdan mesaj ve routing key pattern (örneğin, animal.*) al
    while (true)
    {
        Console.Write("Enter message: ");
        string message = Console.ReadLine();

        Console.Write("Enter routing key pattern (example: animal.*): ");
        string routingKey = Console.ReadLine();

        // Mesajı byte dizisine çevir
        var body = Encoding.UTF8.GetBytes(message);

        // Mesajı Topic Exchange'e belirtilen routing key pattern ile gönder
        channel.BasicPublish(exchange: "topic_exchange",
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body);

        // Kullanıcıya gönderilen mesajı ve kullanılan routing key pattern'i ekrana yazdır
        Console.WriteLine($" [x] Sent: {message} with routing key pattern: {routingKey}");
    }
}
