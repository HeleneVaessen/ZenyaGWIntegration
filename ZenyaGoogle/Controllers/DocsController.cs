
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ZenyaClient;

namespace ZenyaGoogle.Controllers
{
    [Route("[controller]")]
    public class DocsController : Controller
    {

        private DriveService _driveService;
        private DocsService _docsService;
        private IDocumentsClient _documentsClient;
        private IDocumentQuickCodeClient _documentQuickCodeClient;
        private IMemoryCache _cache;
        public DocsController(DriveService driveService, DocsService docsService, IDocumentsClient documentsClient, IMemoryCache cache, IDocumentQuickCodeClient documentQuickCodeClient)
        {
            _driveService = driveService;
            _docsService = docsService;
            _documentsClient = documentsClient;
            _cache = cache;
            _documentQuickCodeClient = documentQuickCodeClient;
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
        /// <summary>
        /// Connect a google doc with a zenya doc. Enter quickcode or zenya doc id.
        /// </summary>
        /// <param name="zenyaDocId">Ex: d30e6730-1bb2-4a66-b0f5-22d5668b5d59</param>
        /// <param name="quickCode">Ex: 25-HN-RY</param>
        /// <param name="googleDocId">Ex: 1xjgAHQhKNAR2h8wgbHZ5EtVbedVHZqnmeNCXiyGSm9o</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> ConnectDocuments(string? zenyaDocId,string? quickCode, string googleDocId)
        {

            var revisions = new RevisionsResource.ListRequest(_driveService, googleDocId).Execute();
            var revisionId = revisions.Revisions.OrderByDescending(x=>x.ModifiedTime).FirstOrDefault()?.Id;
            if (!string.IsNullOrWhiteSpace(quickCode))
            {
                var quickCodeInfo = await _documentQuickCodeClient.GetQuickCodeAsync(quickCode);
                zenyaDocId = quickCodeInfo.Document_id.ToString();
            }
            var request = new RevisionsResource.GetRequest(_driveService, googleDocId, revisionId)
            {
                Fields = "*"
            };

            var revision = request.Execute();
            var zenyadoc = await _documentsClient.GetDocumentDataAsync(new Guid(zenyaDocId));
            if(revision is null || zenyadoc is null) return NotFound();

            var updatedRevision = new RevisionsResource.UpdateRequest(_driveService, new Revision()
            {
                Id = revisionId,
                Published = true,
                PublishAuto = true
            }, googleDocId, revisionId)
            {
                Fields = "*"
            }.Execute();
            updatedRevision = request.Execute();
            _cache.Set(zenyaDocId, (googleDocId, revisionId));

            return Ok(updatedRevision);
        }
        /// <summary>
        /// Get connected google doc. Enter either zenya doc id or quick code
        /// </summary>
        /// <param name="zenyaDocId">Ex: d30e6730-1bb2-4a66-b0f5-22d5668b5d59</param>
        /// <param name="quickCode">Ex: 25-HN-RY</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> GetDocumentConnection(string? zenyaDocId, string? quickCode)
        {
            if (!string.IsNullOrWhiteSpace(quickCode))
            {
                var quickCodeInfo =await _documentQuickCodeClient.GetQuickCodeAsync(quickCode);
                zenyaDocId = quickCodeInfo.Document_id.ToString();
            }
            var zenyadoc = await _documentsClient.GetDocumentDataAsync(new Guid(zenyaDocId));
            if(zenyadoc is null) return NotFound();

            if (_cache.TryGetValue(zenyaDocId,out (string,string) googledocTuple))
            {
               var googledoc= await new RevisionsResource.GetRequest(_driveService,googledocTuple.Item1, googledocTuple.Item2)
               {
                   Fields = "*"
               }.ExecuteAsync();
               return Ok(googledoc);
            }

            return NotFound();
        }

    }
}
