using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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



                            var chaptersInSubject = await context.Chapters.Where(p => p.Subject.nameSubject == subject
                                && p.Subject.LevelStuding.nameLevel == level.Key.ToString())
                                    .ToListAsync();

                            foreach (var chapter in chaptersInSubject)
                            {
                                writer.Write(2, chapter.name);
                                foreach (var user in level)
                                {
                                    var sessionsFiltred = user.SessionChapterExecutions.Where(p => p.ChapterExecution.Chapter.name == chapter.name).ToList();
                                    List<string> arraySessions = new();
                                    var sessionSorted = sessionsFiltred.OrderByDescending(p => p.beginDateTime)
                                        .FirstOrDefault()?.SessionProgresses
                                        .OrderBy(p => p.taskNumber)
                                        .ToList();
                                    if (sessionSorted == null)
                                        continue;
                                    sessionSorted.ForEach(sessionProgress =>
                                {
                                    arraySessions.Add(sessionProgress.GetStatus() == "Ожидает выполнения"
                                        ? "Решено неверно"
                                        : sessionProgress.GetStatus());
                                });
                                    writer.RowDown();
                                    writer.Write(3, user.lastName);
                                    writer.RowUp();
                                    writer.Write(4, arraySessions.ToArray());

                                    arraySessions.Clear();

                                    foreach (var session in sessionsFiltred.OrderByDescending(p => p.beginDateTime))
                                    {
                                        session.SessionProgresses.OrderBy(p => p.taskNumber).ToList().ForEach(item =>
                                        {
                                            arraySessions.Add(item.GetStatus());
                                        });

                                        var timeExecution = ((session.endDateTime ?? DateTime.UtcNow.AddHours(5)) - session.beginDateTime).Value.ToString().Split('.')[0];

                                        writer.Write(4, arraySessions.ToArray());
                                        writer.RowUp();
                                        writer.Write(arraySessions.Count + 4, session.beginDateTime,
                                            session.endDateTime, timeExecution);
                                        arraySessions.Clear();
                                    }
                                    writer.RowDown();
                                }
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }
                municipalityCount++;
            }
            workbook.SaveAs("main.xlsx");
            return Ok();
        }
    }

    public class WorksheetWriter
    {
        private int selectedRow { get; set; } = 1;

        private readonly IXLWorksheet xL;

        public WorksheetWriter(IXLWorksheet worksheet)
        {
            xL = worksheet;
        }

        public void Write(int column, params object[] content)
        {
            try
            {
                foreach (var cellValue in content)
                {
                    if (cellValue.ToString() != "Ожидает выполнения"
        && cellValue.ToString() != "Решено неверно"
        && cellValue.ToString() != "Решено верно")
                    {
                        xL.Cell(selectedRow, column).Value = cellValue.ToString();
                    }
                    else
                    {
                        xL.Cell(selectedRow, column).Style.Fill.BackgroundColor = cellValue.ToString() switch
                        {
                            "Ожидает выполнения" => XLColor.Orange,
                            "Решено неверно" => XLColor.Red,
                            "Решено верно" => XLColor.Green,
                            _ => XLColor.Transparent
                        };
                        xL.Cell(selectedRow, column).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        xL.Cell(selectedRow, column).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        xL.Cell(selectedRow, column).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        xL.Cell(selectedRow, column).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    }
                    column++;
                }
            }
            catch { }
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

        public void RowDown()
        {
            selectedRow++;
        }
    }
}
