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

while (true)
{
    // Kullanıcıdan mesaj alınıyor
    Console.Write("Mesaj gönder: ");
    string message = Console.ReadLine();

    // Mesajın özellikleri (properties) oluşturuluyor
    var properties = channel.CreateBasicProperties();

    // Mesaj başlıkları (headers) oluşturuluyor
    var headers = new Dictionary<string, object>
    {
        { "type", "important" },
        { "priority", 5 }
    }; // Ek başlık (header) ekleme, ihtiyaca göre düzenleyebilirsiniz 

    properties.Headers = headers;

    // Mesajın içeriği UTF-8 formatında alınıyor
    var body = Encoding.UTF8.GetBytes(message);

    // Mesaj headers_exchange'e gönderiliyor
    channel.BasicPublish("headers_exchange", "", properties, body);
    Console.WriteLine($" [x] Gönderilen Mesaj: {message}");
}
