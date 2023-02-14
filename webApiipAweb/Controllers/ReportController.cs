using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public async Task<ActionResult> FullReportGenerate([FromQuery] string areaName, [FromQuery] string[] classes, string subject)
        {
            if (subject == null)
            {
                return BadRequest("Укажите предмет");
            }

            XLWorkbook workbook = new();
            workbook.Worksheets.Add("Основной");
            var worksheet = workbook.Worksheets.Worksheet("Основной");
            var writer = new WorksheetWriter(worksheet);
            var municipalities = await context.Municipalities.Where(p => p.Area.areaName == areaName)
                .ToListAsync();

            int municipalityCount = 1;

            foreach (var municipality in municipalities)
            {
                if (municipality.Schools.FirstOrDefault(p => p.Users.Any()) is School)
                {
                    continue;
                }//???
                writer.Write(1, municipalityCount.ToString(), municipality.name); //???

                int schoolCount = 1;
                foreach (var school in municipality.Schools)
                {

                    var filtredUsers = classes == null
                        ? school.Users
                        : school.Users.Where(p =>
                        {
                            return classes.Contains(p.levelStuding.ToString());
                        });

                    filtredUsers = filtredUsers.Where(p => p.LevelStudingExecutions
                        .FirstOrDefault(p => p.SubjectExecutions
                        .FirstOrDefault(p => p.Subject.nameSubject == subject)
                        is SubjectExecution)
                        is LevelStudingExecution);

                    var sortedUsers = filtredUsers.OrderBy(p => p.levelStuding)
                        .ThenBy(p => p.UserName)
                        .ToList();

                    if (!sortedUsers.Any())
                    {
                        continue;
                    }
                    writer.Write(1, schoolCount.ToString(), school.nameSchool); //???

                    var usersGroupLevel = sortedUsers.GroupBy(p => p.levelStuding);

                    foreach (var level in usersGroupLevel)
                    {
                        writer.Write(2, level.Key.ToString());

                        foreach (var user in level)
                        {
                            var userFiltredSubject = user.SessionChapterExecutions.Where(p => p.ChapterExecution.SubjectExecution.Subject.nameSubject == subject);
                            string[] arraySessions = new string[userFiltredSubject.Count()];

                            writer.Write(3, user.UserName)
                        }
                    }
                }
            }
        }

        private string[] ListSessionToArray<T>(List<T> list)
        {

        }
    }

    public class WorksheetWriter
    {
        private int selectedRow { get; set; }

        private readonly IXLWorksheet xL;

        public WorksheetWriter(IXLWorksheet worksheet)
        {
            xL = worksheet;
        }

        public void Write(int column, params string[] content)
        {
            foreach (var cellValue in content)
            {
                xL.Cell(selectedRow, column).Value = cellValue;
                column++;
            }
            selectedRow++;
        }

        public void DeleteRow(int lenght)
        {
            for (int i = 1; i <= lenght; i++)
            {
                xL.Cell(selectedRow, i).Value = "";
            }
        }

        public void RowUp()
        {
            selectedRow--;
        }
    }
}
