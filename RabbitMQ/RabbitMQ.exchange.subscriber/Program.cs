using RabbitMQ.Client.Events;
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

    // Kullanıcıdan belirli bir routing key girmesini iste
    Console.Write("Enter routing key (info, warning, error): ");
    string routingKey = Console.ReadLine();

    // Geçici, dayanıklı olmayan, özel bir kuyruk oluştur ve adını al
    var queueName = channel.QueueDeclare().QueueName;

    // Kuyruğu Direct Exchange'e belirtilen routing key ile bağla
    channel.QueueBind(queue: queueName,
                      exchange: "direct_exchange",
                      routingKey: routingKey);

    // Belirli bir routing key ile mesaj beklediğini belirten bir mesaj yazdır
    Console.WriteLine($" [*] Waiting for messages with routing key '{routingKey}'. To exit press Ctrl+C");

    // Bir olay tüketici (consumer) oluştur
    var consumer = new EventingBasicConsumer(channel);

    // Mesaj alındığında gerçekleşecek olayı tanımla
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());

        // Alınan mesajı ekrana yazdır
        Console.WriteLine($" [x] Received: {message}");
    };

    // Mesajları tüketmeye başla
    channel.BasicConsume(queue: queueName,
                         autoAck: true, // Mesajları otomatik onayla (acknowledge) ve kuyruktan kaldır
                         consumer: consumer);

    // Konsol uygulamasının kapatılmamasını sağlamak için bir giriş bekler
    Console.ReadLine();
}
