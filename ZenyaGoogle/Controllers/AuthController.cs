using Microsoft.AspNetCore.Mvc;

namespace ZenyaGoogle.Controllers
{
    [Route("[action]")]
    public class AuthController : Controller
    {
       
        [HttpPost("[action]")]
        public IActionResult ConnectAccounts()
        {
            
            return Ok();
        }
    }
}
