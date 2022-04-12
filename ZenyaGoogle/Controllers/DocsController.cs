
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using ZenyaClient;

namespace ZenyaGoogle.Controllers
{
    [Route("[controller]")]
    public class DocsController : Controller
    {

        private DriveService _driveService;
        private DocsService _docsService;
        private IDocumentsClient _documentsClient;
        public DocsController(DriveService driveService, DocsService docsService, IDocumentsClient documentsClient)
        {
            _driveService = driveService;
            _docsService = docsService;
            _documentsClient = documentsClient;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestGetDocuments()
        {
            //var valid  =await GoogleJsonWebSignature.ValidateAsync(tokenProvider.GoogleToken);
            //use dependency injection

            var request = new FilesResource.ListRequest(_driveService);
            var result = request.Execute();
            //var req2= new DocumentsResource.GetRequest(_docsService, result.Files.FirstOrDefault()?.Id);
            //var result2 = req2.Execute();

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDocument(string docid, string? revisionId)
        {
            //var valid  =await GoogleJsonWebSignature.ValidateAsync(tokenProvider.GoogleToken);
            //use dependency injection

            //if (revisionId is  null)
            //{
            //    var req2 = new DocumentsResource.GetRequest(_docsService, docid);
            //    return Ok(req2.Execute());
                
            //}

            var revisions = new RevisionsResource.ListRequest(_driveService, docid).Execute();
            revisionId = revisions.Revisions.FirstOrDefault()?.Id;
            
            var request = new RevisionsResource.GetRequest(_driveService, docid, revisionId);
            var result2 = request.Execute();

            return Ok(result2);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConnectDocuments(string zenyaDocId, string googleDocId)
        {
            var revisions = new RevisionsResource.ListRequest(_driveService, googleDocId).Execute();
            var revisionId = revisions.Revisions.OrderByDescending(x=>x.ModifiedTime).FirstOrDefault()?.Id;

            var request = new RevisionsResource.GetRequest(_driveService, googleDocId, revisionId);
            var revision = request.Execute();
            
            var updatedRevision = new RevisionsResource.UpdateRequest(_driveService, new Revision()
            {
                Id = revisionId,
                Published = true,
                PublishAuto = true
            }, googleDocId, revisionId).Execute();
            return Ok(updatedRevision);
        }
    }
}
