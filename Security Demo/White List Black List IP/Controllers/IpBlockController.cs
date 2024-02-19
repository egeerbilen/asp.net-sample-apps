using IpAddressBlocking;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace White_List_Black_List_IP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IpBlockController : ControllerBase
    {
        [HttpGet("unblocked")]
        public string Unblocked()
        {
            return "Unblocked access";
        }

        [ServiceFilter(typeof(IpBlockActionFilter))]
        [HttpGet("blocked")]
        public string Blocked()
        {
            return "Blocked access";
        }

    }
}
