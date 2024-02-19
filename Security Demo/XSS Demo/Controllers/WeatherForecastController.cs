using Microsoft.AspNetCore.Mvc;
using System.Web;
using XSS_Demo.Model;

namespace XSS_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpPost]
        public IActionResult SaveData([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Veritaban�na kaydetme i�lemleri

            return Ok("Veri ba�ar�yla kaydedildi.");
        }

        [HttpGet]
        public IActionResult GetUserData()
        {
            string userData = "<script>alert('XSS attack!');</script>";
            var qs = "userID=16555&gameID=60&score=4542.122&time=343114";
            // Xss
            string html = HttpUtility.HtmlEncode(userData);
            string javaScript = HttpUtility.JavaScriptStringEncode(userData);
            string Url = HttpUtility.UrlEncode(userData);
            var qsresult = HttpUtility.ParseQueryString(qs);

            // �rne�in, encode edilmi� kullan�c� ad�n� d�nd�r
            return Ok($"Encoded Username: {html} - {javaScript} - {Url} - {qsresult}");
        }


    }
}
