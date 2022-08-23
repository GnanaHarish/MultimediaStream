using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MultimediaStream.Interface;
using MultimediaStream.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MultimediaStream.Controllers
{
    public class MediaController : BaseController
    {
        readonly IStreamFileUploadService _streamFileUploadService;
        public MediaController(IStreamFileUploadService streamFileUploadService)
        {
            _streamFileUploadService = streamFileUploadService;
        }
        [HttpGet("upload")]
        public IActionResult Upload([FromQuery] string name)
        {
            ViewData["name"] = name;
            return View();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Index()
        {
            var name = Request.Headers["username"];
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
            ).Value;
            var reader = new MultipartReader(boundary, Request.Body);
            var section = await reader.ReadNextSectionAsync();
            string response = string.Empty;
            try
            {
                if (await _streamFileUploadService.UploadFile(reader, section, name))
                {

                    ViewBag.Message = "File Upload Successful";
                }
                else
                {
                    ViewBag.Message = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                //Log ex
                ViewBag.Message = "File Upload Failed";
            }
            return Ok(new ResponseForRegister { Status = Constants.Constants.ConstSucess, Message = Constants.Constants.ConstRegistrationSucess });

        }
    }
}
