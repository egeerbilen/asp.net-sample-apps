using RabbitMQ.Client;
using System.Text;

// Fanout Exchange Nedir: Mesajları bağlı olan tüm kuyruklara gönderen bir exchange türüdür.
// RabbitMQ bağlantısı için bir fabrika oluşturuluyor
var factory = new ConnectionFactory() { HostName = "localhost" };

// Bağlantıyı ve kanalı oluşturmak ve kullanmak için using blokları kullanılıyor
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    // Fanout Exchange'i tanımla
    channel.ExchangeDeclare(exchange: "fanout_exchange", type: ExchangeType.Fanout);

    // Sonsuz bir döngü içinde kullanıcıdan mesaj al
    while (true)
    {
        Console.Write("Enter message: ");
        string message = Console.ReadLine();

        // Mesajı byte dizisine çevir
        var body = Encoding.UTF8.GetBytes(message);

        // Mesajı Fanout Exchange'e gönder
        channel.BasicPublish(exchange: "fanout_exchange",
                             routingKey: "", // Fanout exchange'e gönderilen mesajlarda routingKey kullanılmaz
                             basicProperties: null,
                             body: body);

        // Kullanıcıya gönderilen mesajı ekrana yazdır
        Console.WriteLine($" [x] Sent: {message}");
    }
}
