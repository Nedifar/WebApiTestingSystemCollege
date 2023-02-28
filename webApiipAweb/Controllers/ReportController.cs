using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApiipAweb.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(AuthenticationSchemes = "Bearer", Roles ="su")]
        [Route("fullTaskReport")]
        public async Task<ActionResult> FullReportGenerate([FromQuery] string areaName, [FromQuery] string[] classes, string subject)
        {
            var request = Request;
            var response = Response;
            await SessionController.BeforeSessionResponse(context);

            if (subject == null)
            {
                return BadRequest("Укажите предмет");
            }

            await Task.Run(async () =>
            {
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
                        writer.ColumnWidthSettingsSet(1, 3, 21);
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

                            var usersGroupLevel = sortedUsers.GroupBy(p => p.levelStuding + p.levelWord);

                            foreach (var level in usersGroupLevel.OrderBy(p => p.Key))
                            {
                                writer.Write(2, level.Key.ToString());

                                var chaptersInSubject = await context.Chapters.Where(p => p.Subject.nameSubject == subject
                                    && p.Subject.LevelStuding.nameLevel == level.FirstOrDefault().levelStuding.ToString())
                                        .ToListAsync();

                                foreach (var chapter in chaptersInSubject)
                                {
                                    if (level.Any(p => p.SessionChapterExecutions
                                    .Any(s => s.ChapterExecution.Chapter.name == chapter.name)))
                                    {
                                        writer.Write(2, chapter.name);
                                        foreach (var user in level.OrderBy(p => p.lastName))
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

                                            var tasksCount = new string[arraySessions.Count()];
                                            for (int i = 1; i <= arraySessions.Count; i++)
                                            {
                                                tasksCount[i - 1] = i.ToString();
                                            }

                                            writer.Write(6, tasksCount);
                                            writer.Write(3, user.lastName, "ИТОГО:", WorksheetWriter.FormingRightNoRight(arraySessions));
                                            writer.RowUp();

                                            writer.Write(6, arraySessions.ToArray());
                                            writer.RowUp();

                                            writer.Write(6 + arraySessions.Count, "", "",
                                                new DateTime(sessionsFiltred.Sum(p =>
                                                {
                                                    return ((p.endDateTime ?? DateTime.UtcNow.AddHours(5)) - p.beginDateTime).Value.Ticks;
                                                })).ToString("HH:mm:ss"));

                                            arraySessions.Clear();

                                            int sessionsCount = sessionsFiltred.Count;
                                            foreach (var session in sessionsFiltred.OrderByDescending(p => p.beginDateTime))
                                            {
                                                session.SessionProgresses.OrderBy(p => p.taskNumber).ToList().ForEach(item =>
                                                {
                                                    arraySessions.Add(item.GetStatus());
                                                });

                                                var timeExecution = ((session.endDateTime ?? DateTime.UtcNow.AddHours(5)) - session.beginDateTime).Value.ToString().Split('.')[0];

                                                writer.Write(4, "Сессия " + sessionsCount--, WorksheetWriter.FormingRightNoRight(arraySessions));
                                                writer.RowUp();
                                                writer.Write(6, arraySessions.ToArray());
                                                writer.RowUp();
                                                writer.Write(arraySessions.Count + 6, session.beginDateTime,
                                                    session.endDateTime, timeExecution);
                                                arraySessions.Clear();
                                            }
                                            writer.RowDown();
                                        }
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
                workbook.SaveAs(AppContext.BaseDirectory + @"Reports\fullTaskReport.xlsx");
            }
            );
            return PhysicalFile(AppContext.BaseDirectory + @"Reports\fullTaskReport.xlsx", "application/x-msexcel", "report.xlsx");
        }

        [HttpGet]
        [Route("shortTaskReport")]
        public async Task<ActionResult> ShortReportGenerate([FromQuery] string areaName, [FromQuery] string[] classes, string subject)
        {
            await SessionController.BeforeSessionResponse(context);

            if (subject == null)
            {
                return BadRequest("Укажите предмет");
            }

            await Task.Run(async () =>
            {
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
                        writer.ColumnWidthSettingsSet(1, 3, 21);
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

                            var usersGroupLevel = sortedUsers.GroupBy(p => p.levelStuding + p.levelWord);

                            foreach (var level in usersGroupLevel.OrderBy(p => p.Key))
                            {
                                writer.Write(2, level.Key.ToString());

                                var chaptersInSubject = await context.Chapters.Where(p => p.Subject.nameSubject == subject
                                    && p.Subject.LevelStuding.nameLevel == level.FirstOrDefault().levelStuding.ToString())
                                        .ToListAsync();

                                foreach (var chapter in chaptersInSubject)
                                {
                                    if (level.Any(p => p.SessionChapterExecutions
                                    .Any(s => s.ChapterExecution.Chapter.name == chapter.name)))
                                    {
                                        writer.Write(2, chapter.name);
                                        foreach (var user in level.OrderBy(p => p.lastName))
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

                                            var tasksCount = new string[arraySessions.Count()];
                                            for (int i = 1; i <= arraySessions.Count; i++)
                                            {
                                                tasksCount[i - 1] = i.ToString();
                                            }

                                            writer.Write(6, tasksCount);
                                            writer.Write(3, user.lastName, "ИТОГО:", WorksheetWriter.FormingRightNoRight(arraySessions));
                                            writer.RowUp();

                                            writer.Write(6, arraySessions.ToArray());
                                            writer.RowUp();

                                            writer.Write(6 + arraySessions.Count,
                                                new DateTime(sessionsFiltred.Sum(p =>
                                                {
                                                    return ((p.endDateTime ?? DateTime.UtcNow.AddHours(5)) - p.beginDateTime).Value.Ticks;
                                                })).ToString("HH:mm:ss"));

                                            arraySessions.Clear();

                                            writer.RowDown();
                                        }
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
                workbook.SaveAs(AppContext.BaseDirectory + @"Reports\shortTaskReport.xlsx");
            }
            );
            return PhysicalFile(AppContext.BaseDirectory + @"Reports\shortTaskReport.xlsx", "application/x-msexcel", "shortReport.xlsx");
        }

        [HttpGet]
        [Route("fullTheoryReport")]
        public async Task<ActionResult> FullTheoryReportGenerate([FromQuery] string areaName, [FromQuery] string[] classes, string subject)
        {
            await SessionController.BeforeSessionResponse(context);

            if (subject == null)
            {
                return BadRequest("Укажите предмет");
            }

            await Task.Run(async () =>
            {
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
                        writer.ColumnWidthSettingsSet(1, 3, 21);
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

                            var usersGroupLevel = sortedUsers.GroupBy(p => p.levelStuding + p.levelWord);

                            foreach (var level in usersGroupLevel.OrderBy(p => p.Key))
                            {
                                writer.Write(2, level.Key.ToString());

                                var chaptersInSubject = await context.Chapters.Where(p => p.Subject.nameSubject == subject
                                    && p.Subject.LevelStuding.nameLevel == level.FirstOrDefault().levelStuding.ToString())
                                        .ToListAsync();

                                foreach (var chapter in chaptersInSubject)
                                {
                                    if (level.Any(p => p.TheorySessions
                                    .Any(s => s.ChapterExecution.Chapter.name == chapter.name)))
                                    {
                                        writer.Write(2, chapter.name);
                                        foreach (var user in level.OrderBy(p => p.lastName))
                                        {
                                            var sessionsFiltred = user.TheorySessions.Where(p => p.ChapterExecution.Chapter.name == chapter.name).ToList();
                                            var sessionSorted = sessionsFiltred.OrderByDescending(p => p.beginDate).ToList();
                                            if (sessionSorted == null || sessionSorted.Count == 0)
                                                continue;

                                            writer.Write(5, "Начало", "Конец", "Время");

                                            writer.Write(3, user.lastName);
                                            writer.RowUp();

                                            int sessionsCount = sessionsFiltred.Count;
                                            foreach (var session in sessionsFiltred.OrderByDescending(p => p.beginDate))
                                            {
                                                var timeExecution = ((session.endDate ?? DateTime.UtcNow.AddHours(5)) - session.beginDate).Value.ToString().Split('.')[0];

                                                writer.Write(4, "Сессия " + sessionsCount--);
                                                writer.RowUp();
                                                writer.Write(5, session.beginDate,
                                                    session.endDate, timeExecution);
                                            }
                                            writer.RowDown();
                                        }
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
                workbook.SaveAs(AppContext.BaseDirectory + @"Reports\fullTheoryReport.xlsx");
            }
            );
            return PhysicalFile(AppContext.BaseDirectory + @"Reports\fullTheoryReport.xlsx", "application/x-msexcel", "report.xlsx");
        }

        [HttpGet]
        [Route("shortTheoryReport")]
        public async Task<ActionResult> ShortTheoryReportGenerate([FromQuery] string areaName, [FromQuery] string[] classes, string subject)
        {
            await SessionController.BeforeSessionResponse(context);

            if (subject == null)
            {
                return BadRequest("Укажите предмет");
            }

            await Task.Run(async () =>
            {
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
                        writer.ColumnWidthSettingsSet(1, 3, 21);
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

                            var usersGroupLevel = sortedUsers.GroupBy(p => p.levelStuding + p.levelWord);

                            foreach (var level in usersGroupLevel.OrderBy(p => p.Key))
                            {
                                writer.Write(2, level.Key.ToString());

                                var chaptersInSubject = await context.Chapters.Where(p => p.Subject.nameSubject == subject
                                    && p.Subject.LevelStuding.nameLevel == level.FirstOrDefault().levelStuding.ToString())
                                        .ToListAsync();

                                foreach (var chapter in chaptersInSubject)
                                {
                                    if (level.Any(p => p.TheorySessions
                                    .Any(s => s.ChapterExecution.Chapter.name == chapter.name)))
                                    {
                                        writer.Write(2, chapter.name);
                                        foreach (var user in level.OrderBy(p => p.lastName))
                                        {
                                            var sessionsFiltred = user.TheorySessions.Where(p => p.ChapterExecution.Chapter.name == chapter.name).ToList();
                                            var sessionSorted = sessionsFiltred.OrderByDescending(p => p.beginDate).ToList();
                                            if (sessionSorted == null || sessionSorted.Count == 0)
                                                continue;

                                            writer.Write(5, "Мин", "Макс", "Сумм");

                                            writer.Write(3, user.lastName, "ИТОГО:",
                                                sessionsFiltred.Count != 0 ? sessionsFiltred.Min(p => new DateTime(((p.endDate ?? DateTime.UtcNow.AddHours(5)) - p.beginDate).Value.Ticks)).ToString("HH:mm:ss") : "",
                                                sessionsFiltred.Count != 0 ? sessionsFiltred.Max(p => new DateTime(((p.endDate ?? DateTime.UtcNow.AddHours(5)) - p.beginDate).Value.Ticks)).ToString("HH:mm:ss") : "");
                                            writer.RowUp();

                                            writer.Write(7,
                                                new DateTime(sessionsFiltred.Sum(p =>
                                                {
                                                    return ((p.endDate ?? DateTime.UtcNow.AddHours(5)) - p.beginDate).Value.Ticks;
                                                })).ToString("HH:mm:ss"));

                                            int sessionsCount = sessionsFiltred.Count;
                                            writer.RowDown();
                                        }
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
                workbook.SaveAs(AppContext.BaseDirectory + @"Reports\shortTheoryReport.xlsx");
            }
            );
            return PhysicalFile(AppContext.BaseDirectory + @"Reports\shortTheoryReport.xlsx", "application/x-msexcel", "report.xlsx");
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

        public static string FormingRightNoRight(List<string> sessionsArray)
        {
            int right = 0;
            int noRight = 0;
            int _default = 0;
            foreach (var session in sessionsArray)
            {
                switch (session)
                {
                    case "Ожидает выполнения":
                        _default++;
                        break;
                    case "Решено неверно":
                        noRight++;
                        break;
                    case "Решено верно":
                        right++;
                        break;
                }
            }
            return right + " / " + noRight + " / " + _default;
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
                            "Ожидает выполнения" => XLColor.Gray,
                            "Решено неверно" => XLColor.Red,
                            "Решено верно" => XLColor.Green,
                            _ => XLColor.Transparent
                        };
                        xL.Cell(selectedRow, column).Value = cellValue.ToString() switch
                        {
                            "Ожидает выполнения" => -1,
                            "Решено неверно" => 0,
                            "Решено верно" => 1,
                            _ => ""
                        };
                        xL.Cell(selectedRow, column).WorksheetColumn().Width = 3;

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

        public void ColumnWidthSettingsSet(int row, int column, int width)
        {
            xL.Cell(row, column).WorksheetColumn().Width = width;
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
