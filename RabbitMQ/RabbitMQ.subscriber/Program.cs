using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
//dotnet run ile command propt a girersen uygulama çalışır

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://localhost:5672/%2F");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// publisher gerçekden bu aşşağıdaki kuruğu oluşturduğundan eminseniz aşşağıdaki kodu sileneiliriz yoksa queue lmadığı için hata verir
//channel.QueueDeclare("hello-queue", true, false, false); // false diyerek benim kuyruğum memory de olsun dedik. True ise fiziksel olarak kaydedilir rabbitmq ya restart atsak bile durur
// farklı kanallardan bağlanmak için 3. property false olarak verildi
// subscriber gittiğinde de kuyruk silinmesin diye false olarak verdik

var consumer = new EventingBasicConsumer(channel);


// ------------ 1 ------------
//channel.BasicConsume("hello-queue", true, consumer);
////2.property true ise rabbitmq subscribe bir message gönderdiğinde doğru da olsa yanlışta olsa kuruktan siler false yaparsam rabitmq ya sen bu mesajı silme ben gelen mesajı doğru bir şekilde işlersem seni haberdar ederim
//// gerçek hayatta false kullanırız

//consumer.Received += (sender, args) =>
//{
//    var message = Encoding.UTF8.GetString(args.Body.ToArray());

//Console.WriteLine(message);
//};




// ------------ 2 ------------

channel.BasicQos(0, 10, false); // false olursa her bir subscriber için kaçar tane yollayacağını söyülüyoruz. true dersek bölüp yollar toplamda kaç tane subsciber varsa (metodun 2. elemanı/subscribe) 
// 1 tanesini işlerken 9 tanesi cache de bekliyecek hiç rabbit mq ya gitmeden cacheden okuyacak
channel.BasicConsume("hello-queue", true, consumer);

consumer.Received += (sender, args) =>
{
    var message = Encoding.UTF8.GetString(args.Body.ToArray());
    Thread.Sleep(1000);
    Console.WriteLine("gelen Mesaj:" + message);

    channel.BasicAck(args.DeliveryTag, false); // ilgili mesaj işledikden sonra kuyruktan sil dedim
    // true ise memory de işlenmiş ama rabbitmq ya gitmemiş başka mesajlar da varsa onu haerdar eder
};






Console.ReadLine();