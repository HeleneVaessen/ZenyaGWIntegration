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
    }
}
