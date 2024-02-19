using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Security_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataProtectionExampleController : ControllerBase
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<DataProtectionExampleController> _logger;

        public DataProtectionExampleController(IDataProtectionProvider dataProtectionProvider, ILogger<DataProtectionExampleController> logger)
        {
            _protector = dataProtectionProvider.CreateProtector("ApiPurpose");
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Veriyi þifreleme
                string originalText = "Hello, ASP.NET Data Protection in API!";
                string encryptedText = _protector.Protect(originalText);

                // Þifrelenmiþ veriyi çözme
                string decryptedText = _protector.Unprotect(encryptedText);

                // Sonuçlarý loglama
                _logger.LogInformation($"Original Text: {originalText}");
                _logger.LogInformation($"Encrypted Text: {encryptedText}");
                _logger.LogInformation($"Decrypted Text: {decryptedText}");

                return Ok(new { OriginalText = originalText, EncryptedText = encryptedText, DecryptedText = decryptedText });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
