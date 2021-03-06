// Copyright 2018 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START docs_quickstart]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DocsQuickstart
{
    class Programs
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/docs.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { DocsService.Scope.Documents, DocsService.Scope.Drive, DriveService.Scope.DriveAppdata, DriveService.Scope.DriveMetadata };
        static string ApplicationName = "Google Docs API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("./client_secret_408797503550-eeprtabf7n4od01k1lbai04o7ct7av64.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Docs API service.
            /*var service = new DocsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                ApiKey = "AIzaSyA9jxBxcEfEdEUSjZYVutUgOvVBoQFfm8M"
            }) ;*/

            // GET Request.
            // String documentId = "";
            // DocumentsResource.GetRequest request = service.Documents.Get(documentId);

            // CREATE Request.
            // Document doc = new Document();
            // doc.Title = "My Document";
            // DocumentsResource.CreateRequest request = service.Documents.Create(doc);

            // Execute request.
            // Prints the title of the requested doc:
            // https://docs.google.com/document/d/195j9eDD3ccgjQRttHhJPymLJUCOUjs-jmwTrekvdjFE/edit
            //Document doc1 = request.Execute();
            //Console.WriteLine($"Document {doc1.Title} successfully found");


            // CREATE File AppData
            Google.Apis.Drive.v3.Data.File fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = "newdoc.docx";
            fileMetadata.Parents = new string[] { "appDataFolder" };

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                ApiKey = "AIzaSyA9jxBxcEfEdEUSjZYVutUgOvVBoQFfm8M"
            }) ;

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            request.Execute();
            Console.WriteLine("File ID: " + fileMetadata.Id);


            // SEARCH Appdata
            var requestSearch = service.Files.List();
            requestSearch.Spaces = "appDataFolder";
            requestSearch.Fields = "nextPageToken, files(id, name)";
            requestSearch.PageSize = 10;
            var responseSearch = requestSearch.Execute();

            foreach(Google.Apis.Drive.v3.Data.File file in responseSearch.Files)
            {
                Console.WriteLine($"Found file {file.Name}");
            }

        }
    }
}
// [START docs_quickstart]
