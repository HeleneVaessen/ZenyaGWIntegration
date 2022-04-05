
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace ZenyaGoogle.Controllers
{
    [Route("[controller]")]
    public class DocsController : Controller
    {
        
        private DriveService driveService;
        
        public DocsController(DriveService driveService)
        {
          
            this.driveService = driveService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestGetDocuments()
        {
            //var valid  =await GoogleJsonWebSignature.ValidateAsync(tokenProvider.GoogleToken);
            //use dependency injection
            
            var request = new FilesResource.ListRequest(driveService);
            var result =request.Execute();
            return Ok(result);
        }
    }
}
