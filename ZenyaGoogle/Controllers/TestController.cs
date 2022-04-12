using System.Text;
using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Mvc;
using ZenyaClient;

namespace ZenyaGoogle.Controllers
{
    /// <summary>
    /// use this for testing stuff
    /// </summary>
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly IUsersClient _userClient;
        
        public TestController(IUsersClient userClient)
        {
            _userClient = userClient;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCurrentZenyaUser()
        {
            var user = await _userClient.GetCurrentUserAsync();
            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDoc()
        {
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLogin()
        {
            var html=System.IO.File.ReadAllText("./assets/login.html");
         
            return Content(html, "text/html", Encoding.UTF8);
        }
    }
}
