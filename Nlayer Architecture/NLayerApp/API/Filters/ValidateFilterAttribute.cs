using Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NLayer.API.Filters
{

    public class ValidateFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Fluent validayion kullanmasanda ModelState.IsValid üzerinden de kontrol ede biliriz fluent validation ile bu entegre
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList(); // hatalar burada tek tek alınır ve sadece ErrorMessage hata mesajlarını getirir ve listeye çevirir

                // client hatası olurda 4XX lü durumlar döner
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors)); 
                // response nin içinde hata mesajları varsa BadRequestObjectResult döner
            }
        }
    }
}
