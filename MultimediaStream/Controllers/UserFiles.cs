using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MultimediaStream.Models;

namespace MultimediaStream.Controllers
{
    public class UserFiles : BaseController
    {
     
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserFiles(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("UserFiles")]
        public ActionResult UserFiless(string name)
        {
            //var name = Request.Headers["username"];
            var provider = new PhysicalFileProvider(webHostEnvironment.WebRootPath).Root + name;
            
            string[] filePaths = Directory.GetFiles(provider);

            List<FileList> PicsList = new List<FileList>();

            int i = 1;
            string fileEx = null;

            foreach (string filePath in filePaths)

            {
                if (Path.HasExtension(filePath))
                {
                     fileEx = Path.GetExtension(filePath);
                }

                PicsList.Add(new FileList

                {

                    FileId = i++,

                    FileName = Path.GetFileName(filePath),

                   FilePath = "file:///"+(filePath).Replace("\\", "/"),

                   FileExten = fileEx

                });

                

            }
            return View(PicsList);

        }
    }
}
