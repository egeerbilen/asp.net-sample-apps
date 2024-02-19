using Core.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    public static class UseCustomExceptionHandler // bir extension metot yazman için class ve metot static olmak zorunda
    {
        // Global exception handler
        // uygulamaya bir request geldiğinde ve ya response döndüğünde Middlewares e girer

        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                // freame workün bize sağlamış olduğu bir modeldir UseExceptionHandler ama biz kendi modelimizi dönmek için config ile birlikte aşağıda giriyoruz
                config.Run(async context => // Run sonlandırıcı bir middileware dır yani burada yazdığımız koddan sonra artık akış buradan geriye dönecek daha ileriye gitmeyecek controller metotlara kadar gitmeden buradan geri dönecek
                {
                    // default gelen middaleware içinde sonlandırıcı bir middelware yazıyruz exception varsa daha ileri gitmeyecek geri dönecek
                    context.Response.ContentType = "application/json"; // resonse ContentType'ın kodun tipini belrledik

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>(); // hatayı verecek interface burada belirtiliyor -> uygulamada fırlatılan hatayı alıyoruz

                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400, //client taraflı ise 
                        NotFoundExcepiton => 404, 
                        _ => 500 
                        // default olarak 500 ata 500 hatası genelde db ile ilgili hatadır ve bunu client e dönmeye gerek yokdur broyı loglayıp 500 is ortak bir hata mesajı alta eklene bilir
                    };
                    context.Response.StatusCode = statusCode;


                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);


                    await context.Response.WriteAsync(JsonSerializer.Serialize(response)); // bu tipi geri gönmek için json a serialize etmemiz gerek freme work burada otomatik olarak JsonSerialize etmediği için biz JsonSerialze diyoruz

                });
            });
        }
    }
}
