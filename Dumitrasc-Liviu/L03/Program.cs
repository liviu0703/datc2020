using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;

namespace L03
{
    class Program
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "DATC L03";
        static DriveService service;
        static UserCredential credential;

        static string fileName = "ceva.txt";
        static string contentType = "text/plain"; 

        static void Main(string[] args)
        {
            Initialize();
            GetAllFiles();
            UploadFile(service, fileName, contentType);
        }

        static void Initialize(){
            

            using (var stream =
                new FileStream("client-datc-l03.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "Liviu",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            
        }

        static void GetAllFiles() {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + credential.Token.AccessToken);

            using(var response = request.GetResponse())
            {
                using(Stream data = response.GetResponseStream())
                using(var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach (var file in myData["files"])
                    {
                        if(file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }

        static void UploadFile(DriveService service, string fileName, string contentType)
        {
            var fileToDrive = new Google.Apis.Drive.v3.Data.File();
            fileToDrive.Name = fileName;
            fileToDrive.Parents = new List<string> { "root" };
            FilesResource.CreateMediaUpload request;
            using(var stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                request = service.Files.Create(fileToDrive, stream, contentType);
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File upload: " + file.Id);
        }
    }
}
