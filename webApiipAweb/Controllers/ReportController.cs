using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using webApiipAweb.Models;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly context context;

        public ReportController(context context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> FullReportGenerate([FromQuery] string areaName, [FromQuery] string[] classes)
        {
            XLWorkbook workbook = new();
            workbook.Worksheets.Add("Основной");
            var worksheet = workbook.Worksheets.Worksheet("Основной");
            var municipalities = await context.Municipalities.Where(p => p.Area.areaName == areaName)
                .ToListAsync();

            foreach (var municipality in municipalities)
            {
                foreach (var school in municipality.Schools)
                {
                    var filtredUsers = 
                    school.Users.OrderBy(p => p.levelStuding).ToList()
                    foreach (var child in )
                    {
                        child.
                    }
                }
            }
        }
    }
}
}
