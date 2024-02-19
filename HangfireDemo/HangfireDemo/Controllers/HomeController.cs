using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HomeController(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        [Route("FireAndForgetJob")]
        public string FireAndForgetJob()
        {
            // Bu kod örneği, Hangfire kütüphanesini kullanarak "Fire and Forget" (Çalıştır ve Unut) tipinde bir arka plan görevini başlatan bir ASP.NET Core metodu göstermektedir.
            // "Fire and Forget" görevler, bir işlemi başlatır ve işlemin sonucunu beklemeden hemen devam eder. Bu görevler, genellikle uzun süren veya bağımsız olarak yürütülebilen işlemleri başlatmak için kullanılır.

            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!")); // Bu satır, Hangfire'in Enqueue metodu aracılığıyla bir "Fire and Forget" görevini başlatır. 

            var jobId1 = _backgroundJobClient.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!")); // Bu satırda, bir _backgroundJobClient nesnesi üzerinden de aynı görevi başlatılır. 

            return $"Job ID: {jobId}. Welcome user in Fire and Forget Job Demo!";
        }

        [HttpGet]
        [Route("DelayedJob")]
        public string DelayedJob()
        {
            // belirli bir süre sonra bir görevi çalıştırmak istediğiniz durumlarda kullanışlıdır. Örneğin, bir e-ticaret uygulamasında, kullanıcıya ödeme yapma işlemi sonrasında bir teşekkür e-postası göndermek için gecikmeli bir görev kullanılabilir.
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine("Welcome user in Delayed Job Demo!"), TimeSpan.FromSeconds(60));

            return $"Job ID: {jobId}. Welcome user in Delayed Job Demo!";
        }

        [HttpGet]
        [Route("ContinuousJob")]
        public string ContinuousJob()
        {
            // Bu kod parçası, birinci görevin tamamlanmasına bağlı olarak ikinci bir görevin başlatılmasını gösterir. Bu tür bir yapı, bir işlemin adım adım gerçekleştirilmesi ve farklı görevlerin sırayla çalıştırılması gerektiği durumlar için kullanışlı olabilir.
            var parentjobId = BackgroundJob.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!"));

            BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("Welcome Sachchi in Continuos Job Demo!"));

            return "Welcome user in Continuos Job Demo!";
        }

        [HttpGet]
        [Route("RecurringJob")]
        public string RecurringJobs()
        {
            // Bu kod parçası, belirli bir zaman aralığına göre periyodik olarak çalışması gereken işlemleri gösterir. Örneğin, saat başı veya günün belirli saatlerinde belirli bir işlemin otomatik olarak gerçekleştirilmesi gibi senaryolarda kullanışlıdır.
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Welcome user in Recurring Job Demo!"), Cron.Hourly);

            return "Welcome user in Recurring Job Demo!";
        }

        [HttpGet]
        [Route("BatchesJob")]
        public string BatchesJob()
        {
            // Bu kod örneği, Hangfire kütüphanesini kullanarak "Batches Job" (Toplu İş) senaryosunu açıklamak üzere tasarlanmıştır, ancak not olarak belirtildiği gibi bu seçenek yalnızca Hangfire Pro sürümünde mevcuttur. 
            // İlgili koddan bağımsız olarak, "Batches Job" genellikle bir dizi işlemi toplu halde yürütmek ve bu işlemleri bir bütün olarak yönetmek istediğiniz durumlarda kullanışlıdır. Örneğin, birden çok görevin belirli bir sırayla veya aynı anda başlatılması gereken karmaşık bir senaryo düşünün.

            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Batch Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Batch Job 2"));
            //});

            return "Welcome user in Batches Job Demo!";
        }

        [HttpGet]
        [Route("BatchContinuationsJob")]
        public string BatchContinuationsJob()
        {
            // BatchJob.ContinueBatchWith metodu, bir önceki "Batch Job" işleminin tamamlanmasından sonra bir devam işini başlatmak için kullanılır. Ancak, bu özellik yalnızca Hangfire Pro sürümü kullanılarak erişilebilir
            // Bu tür bir senaryo, bir grup görevin sırayla veya paralel olarak çalıştırılmasını gerektiren karmaşık iş akışlarında kullanışlı olabilir. 

            //var batchId = BatchJob.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Batch Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Batch Job 2"));
            //});

            //BatchJob.ContinueBatchWith(batchId, x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Last Job"));
            //});

            return "Welcome user in Batch Continuations Job Demo!";
        }
    }
}
