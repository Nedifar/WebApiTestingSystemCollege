using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using webApiipAweb.Models;
using static System.Net.WebRequestMethods;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly context _context;

        public TestController(context context) => _context = context;

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> UploadImage(IFormFile image)
        {
            if (image != null)
            {
                string path = AppContext.BaseDirectory + "files\\";

                using (var stream = new FileStream(path + image.FileName, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
            }
            return Ok(new
            {
                success = 1,
                file = new
                {
                    url = "http://localhost:5000/" + image.FileName
                }
            });
        }

        [HttpPost]
        [Route("CreateTask")]
        public async Task<ActionResult> CreateTask(string content)
        {
            return Ok();
        }

        [HttpPost]
        [Route("fetchUrl")]
        public async Task<ActionResult> FetchUrl(model model)
        {
            return Ok(new
            {
                success = 1,
                file = new
                {
                    url = model.url
                }
            });
        }

        public record model (string url);
    }
}
