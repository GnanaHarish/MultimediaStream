using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using MultimediaStream.Models;
using MultimediaStream.ViewModels;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MultimediaStream.Controllers
{
    public class FullUserVideos : BaseController

    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FullUserVideos(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("updatevideo")]
        public IActionResult Index(FileViewModel model)
        {
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=PETSERVER;Initial Catalog=harishTraining;User Id=training;Password=*;"))
                {
    
                    string queryCount = $"SELECT COUNT(*) FROM [dbo].[videoview] WHERE urlName like '%{model.Url}%'";
                    using (SqlCommand cmd = new SqlCommand(queryCount, connection))
                    {
                        connection.Open();
                        count = (int)(cmd.ExecuteScalar());
                        connection.Close();
                    }
                }
                if (count > 0)
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=PETSERVER;Initial Catalog=harishTraining;User Id=training;Password=*;"))
                    {
                        int i;
                        //var insertRes;
                        string query = $"select (viewCount) from [dbo].[videoview] where urlName like '%{model.Url}%'";
          
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            i = (int)(cmd.ExecuteScalar());

                        }
                        i++;
                        string insertQuery = $"update  [dbo].[videoview] set viewCount = {i} where urlName like '%{model.Url}%'";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            //connection.Open();
                            var insertRes = cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
                else {
                    using (SqlConnection connection = new SqlConnection("Data Source=PETSERVER;Initial Catalog=harishTraining;User Id=training;Password=*;"))
                    {
                        string query = $"insert into [dbo].[videoview] (urlName, viewCount) values ('{model.Url}', 0)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            var i = cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok("Student added succesfull");
        }

        [HttpGet("trending")]
        public ActionResult FullUserVideoss()
        {
            //var name = Request.Headers["username"];
            string[] dirs = Directory.GetDirectories(webHostEnvironment.WebRootPath, "*", SearchOption.TopDirectoryOnly);
            List<AllFiles> PicsList = new List<AllFiles>();
            int i = 1;
            string fileEx = null;
            int viewCount = 0;
            int p = 0;
            foreach (string folderName in dirs) {
                
            var provider = folderName;
 
            string[] filePaths = Directory.GetFiles(provider);
            foreach (string filePath in filePaths)
            {
                    int idx = filePath.LastIndexOf("\\");
                    string fileName = filePath.Substring(idx + 1);
                    if (Path.HasExtension(filePath))
                {
                    fileEx = Path.GetExtension(filePath);
                }
                    try
                    {
                        using (SqlConnection connection = new SqlConnection("Data Source=PETSERVER;Initial Catalog=harishTraining;User Id=training;Password=*;"))
                        {
                            int count;
                            string queryCount = $"SELECT COUNT(*) FROM [dbo].[videoview] WHERE urlName like '%{fileName}%'";
                            using (SqlCommand cmd = new SqlCommand(queryCount, connection))
                            {
                                connection.Open();
                                count = (int)(cmd.ExecuteScalar());
                            }
                            //var insertRes;
                            if (count > 0)
                            {
                                string query = $"select (viewCount) from [dbo].[videoview] where urlName like '%{fileName}%'";
                                using (SqlCommand cmd = new SqlCommand(query, connection))
                                {
                                    p = (int)(cmd.ExecuteScalar());

                                }
                            }
                            else {
                                p = 0;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.ToString());
                    }

                    PicsList.Add(new AllFiles

                    {

                        FileId = i++,

                        FileName = Path.GetFileName(filePath),

                        FilePath = "file:///" + (filePath).Replace("\\", "/"),

                        FileExten = fileEx,

                        viewCount = p

                    }); ;



            }
            }
            //var fileList = PicsList.ToList();
            return View(PicsList);

        }
    }
}
