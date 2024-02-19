using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    public class HangfireController : Controller
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public HangfireController(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        [Route("IFireAndForgetJob")]
        public string FireAndForgetJob()
        {
            // Bu kod örneği, Hangfire kütüphanesini kullanarak "Fire and Forget" (Çalıştır ve Unut) tipinde bir arka plan görevini başlatan bir ASP.NET Core metodu göstermektedir.
            // "Fire and Forget" görevler, bir işlemi başlatır ve işlemin sonucunu beklemeden hemen devam eder. Bu görevler, genellikle uzun süren veya bağımsız olarak yürütülebilen işlemleri başlatmak için kullanılır.
            var jobId = _backgroundJobClient.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!"));

            return $"Job ID: {jobId}. Welcome user in Fire and Forget Job Demo!";
        }

        [HttpGet]
        [Route("IDelayedJob")]
        public string DelayedJob()
        {
            // belirli bir süre sonra bir görevi çalıştırmak istediğiniz durumlarda kullanışlıdır. Örneğin, bir e-ticaret uygulamasında, kullanıcıya ödeme yapma işlemi sonrasında bir teşekkür e-postası göndermek için gecikmeli bir görev kullanılabilir.
            var jobId = _backgroundJobClient.Schedule(() => Console.WriteLine("Welcome user in Delayed Job Demo!"), TimeSpan.FromSeconds(60));

            return $"Job ID: {jobId}. Welcome user in Delayed Job Demo!";
        }

        [HttpGet]
        [Route("IContinuousJob")]
        public string ContinuousJob()
        {
            // Bu kod parçası, birinci görevin tamamlanmasına bağlı olarak ikinci bir görevin başlatılmasını gösterir. Bu tür bir yapı, bir işlemin adım adım gerçekleştirilmesi ve farklı görevlerin sırayla çalıştırılması gerektiği durumlar için kullanışlı olabilir.
            var parentjobId = _backgroundJobClient.Enqueue(() => Console.WriteLine("Welcome user in Fire and Forget Job Demo!"));

            BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("Welcome Sachchi in Continuos Job Demo!"));

            return "Welcome user in Continuos Job Demo!";
        }

        [HttpGet]
        [Route("IRecurringJob")]
        public string RecurringJobs()
        {
            // Bu kod parçası, belirli bir zaman aralığına göre periyodik olarak çalışması gereken işlemleri gösterir. Örneğin, saat başı veya günün belirli saatlerinde belirli bir işlemin otomatik olarak gerçekleştirilmesi gibi senaryolarda kullanışlıdır.
            _recurringJobManager.AddOrUpdate("jobId", () => Console.WriteLine("Welcome user in Recurring Job Demo!"), Cron.Daily);

            return "Welcome user in Recurring Job Demo!";
        }
    }
}
