using FluentValidationDemo.Model;
using FluentValidationDemo.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FluentValidationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        [HttpPost("add")]
        public IActionResult Add(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            return StatusCode(StatusCodes.Status200OK, "Model is valid!");
        }
    }
}
