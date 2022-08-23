using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MultimediaStream.Interface;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Data.SqlClient;

namespace MultimediaStream.Services
{
    public class StreamFileUploadLocalService : IStreamFileUploadService
    {
        private IWebHostEnvironment Environment;
        public StreamFileUploadLocalService(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public async Task<bool> UploadFile(MultipartReader reader, MultipartSection? section, string name)
        {
            var a = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var b
                );

            string path = Path.Combine(this.Environment.WebRootPath, name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition
                );

                if (hasContentDispositionHeader)
                {
                    
                    if (contentDisposition.DispositionType.Equals("form-data") &&
                    (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
                    !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
                    {
                        string fileName = contentDisposition.FileName.Value;
                        try
                        {
                            using (SqlConnection connection = new SqlConnection("Data Source=PETSERVER;Initial Catalog=harishTraining;User Id=training;Password=*;"))
                            {
                                
                                //var insertRes;
                                string query = $"insert into [dbo].[videoview] (urlName, viewCount) values ('{fileName}', 0)";
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    connection.Open();
                                    var i = cmd.ExecuteNonQuery();

                                }
                               
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed");
                        }
                        string filePath = Path.GetFullPath(Path.Combine(Path.GetFullPath("wwwroot"),name));
                        byte[] fileArray;
                        using (var memoryStream = new MemoryStream())
                        {
                            await section.Body.CopyToAsync(memoryStream);
                            fileArray = memoryStream.ToArray();
                        }
                        using (var fileStream = System.IO.File.Create(Path.Combine(filePath, contentDisposition.FileName.Value)))
                        {
                            await fileStream.WriteAsync(fileArray);
                        }
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }
            return true;
        }
    }
}
