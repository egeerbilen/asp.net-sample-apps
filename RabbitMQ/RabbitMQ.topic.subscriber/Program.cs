using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// RabbitMQ bağlantısı için bir fabrika oluşturuluyor
var factory = new ConnectionFactory() { HostName = "localhost" };

// Bağlantıyı ve kanalı oluşturmak ve kullanmak için using blokları kullanılıyor
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    // Topic Exchange'i tanımla
    channel.ExchangeDeclare(exchange: "topic_exchange", type: ExchangeType.Topic);

    // Kullanıcıdan belirli bir yönlendirme anahtarı deseni (örneğin, animal.*) girmesini iste
    Console.Write("Enter routing key pattern (example, animal.*): ");
    string routingKeyPattern = Console.ReadLine();

    // Geçici, dayanıklı olmayan, özel bir kuyruk oluştur ve adını al
    var queueName = channel.QueueDeclare().QueueName;

    // Kuyruğu Topic Exchange'e belirtilen routing key pattern ile bağla
    channel.QueueBind(queue: queueName,
                      exchange: "topic_exchange",
                      routingKey: routingKeyPattern);

    // Belirli bir routing key pattern ile mesaj beklediğini belirten bir mesaj yazdır
    Console.WriteLine($" [*] Waiting for messages with routing key pattern '{routingKeyPattern}'. To exit press Ctrl+C");

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


// 1. İlk olarak, yukarıda verilen Publisher kodunu çalıştırın. Bu program size bir mesaj ve bir yönlendirme anahtarı (routing key) deseni girmenizi isteyecek.
// Örneğin:
// - Mesaj: "Hello, RabbitMQ!"
// - Yönlendirme Anahtarı Deseni: "animal.*"

// 2. Ardından, Subscriber kodunu iki farklı konsol penceresinde çalıştırın. Her biri sizden bir yönlendirme anahtarı deseni girmenizi isteyecek.
// Örneğin:
// - İlk Abone Yönlendirme Anahtarı Deseni: "animal.*"
// - İkinci Abone Yönlendirme Anahtarı Deseni: "animal.rabbit"

// 3. Son olarak, Publisher kodunu çalıştırdığınız konsol penceresine gidin ve birkaç farklı mesaj ve yönlendirme anahtarı deseni daha girebilirsiniz.

// Her iki abone, belirttikleri yönlendirme anahtarı desenlerine uyan mesajları almalıdır. Örneğin, ilk abone "animal.*" desenini belirttiyse
// ve ikinci abone "animal.rabbit" desenini belirttiyse, "animal.rabbit" yönlendirme anahtarına sahip mesajları sadece ikinci abone almalıdır.
// Diğer taraftan, "animal.dog" gibi bir mesaj "animal.*" deseni ile eşleştiği için her iki abone de almalıdır.

// animal.rabbit.white: "animal" konusundaki "rabbit" türündeki "white" renkteki hayvanlara yönlendirme yapar.
// animal.*.brown: "animal" konusundaki herhangi bir türdeki "brown" renkteki hayvanlara yönlendirme yapar.
// animal.#: "animal" konusundaki tüm hayvanlara yönlendirme yapar.