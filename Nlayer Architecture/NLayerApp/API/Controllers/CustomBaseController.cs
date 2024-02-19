using Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        // Bu bir Endpoint değil bunubelirtmek için NonAction koyuyoruz
        // NonAction olmazsa swager bunu bir end point olarak algılar ve endpoint olarak algıladığında getir veya post olmadığı için hata verir
        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            // 204 No Content geriye birşey döniyeceksin anlamına geliyor
            // aşağıda objectresult a null verdik status koda ise response dan gelen status kodu olsun
            if (response.StatusCode == 204) return new ObjectResult(null) { StatusCode = response.StatusCode };

            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
}
