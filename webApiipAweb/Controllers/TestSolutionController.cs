using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using webApiipAweb.Models;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestSolutionController : ControllerBase
    {
        private readonly context context;
        public TestSolutionController(Models.context _context)
        {
            context = _context;
        }

        [HttpPost]
        [Route("getSubjects")]
        public async Task<ActionResult> GetSubjects(PostTestModels.GetSubjectPost subjectPost)
        {
            var child = await context.Children.FirstOrDefaultAsync(p => p.Id == subjectPost.ChildId);
            if (child == null)
                return BadRequest("Не найдено.");
            var exec = child.LevelStudingExecutions.FirstOrDefault(p => p.LevelStuding.nameLevel == child.levelStuding.ToString());
            foreach (var mod1 in context.LevelStudings.FirstOrDefault(p => p.nameLevel == child.levelStuding.ToString())?.Subjects)
            {

                if (exec.SubjectExecutions.FirstOrDefault(p => p.idSubject == mod1.idSubject) == null)
                {
                    exec.SubjectExecutions.Add(new SubjectExecution { Subject = mod1 });
                }

                var subexec = exec.SubjectExecutions.FirstOrDefault(p => p.Subject == mod1);
                foreach (var mod2 in mod1.Chapters)
                {
                    if (subexec.ChapterExecutions.FirstOrDefault(p => p.idChapter == mod2.idChapter) == null)
                    {
                        subexec.ChapterExecutions.Add(new ChapterExecution { Chapter = mod2 });
                    }
                    var chapexec = subexec.ChapterExecutions.FirstOrDefault(p => p.Chapter == mod2);
                    foreach (var mod4 in mod2.TestPacks)
                    {
                        if (chapexec.TestPackExecutions.FirstOrDefault(p => p.idTestPack == mod4.idTestPack) == null)
                        {
                            chapexec.TestPackExecutions.Add(new TestPackExecution { TestPack = mod4 });
                        }
                        var packexec = chapexec.TestPackExecutions.FirstOrDefault(p => p.TestPack == mod4);
                        foreach (var mod3 in mod4.TaskWithOpenAnsws)
                        {
                            if (packexec.TaskWithOpenAnswsExecutions.FirstOrDefault(p => p.idTask == mod3.idTask) == null) //???
                            {
                                packexec.TaskWithOpenAnswsExecutions.Add(new TaskWithOpenAnswsExecution { TaskWithOpenAnsw = mod3, Status = StatusTaskExecution.AwaitingExecution });
                            }
                            var taskexec = packexec.TaskWithOpenAnswsExecutions.FirstOrDefault(p => p.TaskWithOpenAnsw == mod3);
                        }
                        foreach (var mod3 in mod4.TaskWithClosedAnsws)
                        {
                            if (packexec.TaskWithClosedAnswsExecutions.FirstOrDefault(p => p.idTask == mod3.idTask) == null) //???
                            {
                                packexec.TaskWithClosedAnswsExecutions.Add(new Models.TaskWithClosedAnswsExecution { TaskWithClosedAnsw = mod3, Status = StatusTaskExecution.AwaitingExecution });
                            }
                            var taskexec = packexec.TaskWithClosedAnswsExecutions.FirstOrDefault(p => p.TaskWithClosedAnsw == mod3);
                        }
                    }
                }
            }
            await context.SaveChangesAsync();
            return Ok(exec.SubjectExecutions.Select(p => new
            {
                idExec = p.idSubjectExecution,
                NameSub = p.Subject.nameSubject,
                ProgressInProcent = p.getProgressInProcent
            }
            ));
        }

        [HttpPost]
        [Route("getChapters")]
        public async Task<ActionResult> GetChapters(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = await context.ChapterExecutions.Where(p => p.idSubjectExecution == chaptersPost.idSubjectExecution).ToListAsync();
            return Ok(s.OrderBy(p => p.Chapter.numeric)
                .ThenBy(p => p.idChapter)
                .Select(p => new
                {
                    NameChapter = p.Chapter.name,
                    p.Chapter.Description,
                    getProcentChapterDecide = p.getProcentMainTasks,
                    MainPackProcent = p.getProcentMainTasks,
                    OtherPackProcent = p.getProcentOtherTasks,
                    idExec = p.idChapterExecution,
                    p.Chapter.access
                }
                ));
        }

        [HttpPost]
        [Route("getChapter")]
        public async Task<ActionResult> GetChapter(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = await context.ChapterExecutions.FirstOrDefaultAsync(p => p.idChapterExecution == chaptersPost.idSubjectExecution);

            return Ok(new
            {
                NameChapter = s.Chapter.name,
                Description = s.Chapter.Description,
                idExec = s.idChapterExecution,
                getProcentChapterDecide = s.getProcentChapter,
                MainPackProcent = s.getProcentMainTasks,
                OtherPackProcent = s.getProcentOtherTasks,

                TestPack = s.TestPackExecutions.Select(p => new
                {
                    Type = p.TestPack.GetPackType(),
                    Header = p.TestPack.header,
                    haveFinalTest = p.haveFinalTest,
                    accessFinalTest = p.accessProcentFinalTest,
                    Tasks = p.GetTasksExecution().Select(l => new
                    {
                        idTaskExecution = l.idTaskExecution,
                        idTestPackExucution = l.idTestPackExecution,
                        status = l.GetStatus(),
                        serialNumber = p.GetTasksExecution().IndexOf(l) + 1,
                        isIncreasedComplexity = l.isHard,
                        theme = l.themes
                    })
                }),
                theory = s.Chapter.TheoreticalMaterials.Select(l => new
                {
                    l.header,
                    l.content,
                    additionalMaterial = l.additionalMaterial,
                    resources = l.TheoreticalMaterialResources.Select(s => new
                    {
                        header = s.header,
                        url = "https://gamification.oksei.ru/imagecontainer/" + s.url
                    })
                }),
                s.Chapter.access,
            });
        }

        [HttpPost]
        [Route("GetTaskOpenOnNumber")]
        public async Task<ActionResult> GetTaskOpenOnNumber(PostTestModels.GetTaskOpenOnNumberPost model)
        {
            try
            {
                var s = await context.TestPackExecutions.FirstOrDefaultAsync(p => p.idChapterExecution == model.idExecChapter && p.TestPack.header == model.headerTestPack);
                var closed = s.TaskWithClosedAnswsExecutions.FirstOrDefault(p => p.TaskWithClosedAnsw.numericInPack == model.serialNumber);
                var opened = s.TaskWithOpenAnswsExecutions.FirstOrDefault(p => p.TaskWithOpenAnsw.numericInPack == model.serialNumber);
                int? id = opened?.idTask ?? closed?.idTask;
                string imageUrl = "http://192.168.147.72:83/" + $"task{id}.jpeg";
                var timeNow = DateTime.UtcNow.AddHours(5);
                var currentTime = (new DateTime(timeNow.Year, timeNow.Month, timeNow.Day + 1, 0, 0, 0) - timeNow).Hours
                    + "ч "
                    + (new DateTime(timeNow.Year, timeNow.Month, timeNow.Day + 1, 0, 0, 0) - timeNow).Minutes + "мин";
                if ((new DateTime(timeNow.Year, timeNow.Month, timeNow.Day + 1, 0, 0, 0) - timeNow).Hours == 0)
                    currentTime = currentTime.Replace("0ч ", "");
                if (closed is not null)
                {
                    return Ok(new
                    {
                        textQuestion = closed.TaskWithClosedAnsw.textQuestion,
                        serialNumber = model.serialNumber,
                        selectedAnswear = closed.AnswearOnTask?.idAnswearOnTask,
                        Answears = closed.TaskWithClosedAnsw.AnswearOnTask.Select(p => new
                        {
                            text = p.textAnswear,
                            idAnswear = p.idAnswearOnTask
                        }),
                        locked = closed.lockedTime > DateTime.UtcNow.AddHours(5),
                        lockedTime = currentTime,
                        status = closed.GetStatus(),
                        type = closed.TaskWithClosedAnsw.TypesTask.ToString(),
                        theme = closed.TaskWithClosedAnsw.theme,
                        solutions = closed.TaskWithClosedAnsw.Solutions.Select(p => new
                        {
                            url = "http://192.168.147.72:83/" + $"{p.url.Replace("images/", "")}"
                        })
                    });
                }
                else if (opened is not null)
                {
                    return Ok(new
                    {
                        textQuestion = imageUrl,
                        serialNumber = model.serialNumber,
                        selectedAnswear = opened.AnswearResult,
                        status = opened.GetStatus(),
                        type = opened.TaskWithOpenAnsw.TypesTask.ToString(),
                        theme = opened.TaskWithOpenAnsw.theme,
                        solutions = opened.TaskWithOpenAnsw.Solutions.Select(p => new
                        {
                            url = "http://192.168.147.72:83/" + $"{p.url.Replace("images/", "")}"
                        }),
                        locked = opened.lockedTime > DateTime.UtcNow.AddHours(5),
                        lockedTime = currentTime,
                        typeResult = opened.TaskWithOpenAnsw.GetResultType(),
                        modelResult = opened.TaskWithOpenAnsw.htmlModel
                    });
                }
                else
                {
                    return BadRequest("Что-то не так.");
                }
            }
            catch
            {
                return BadRequest("Неверный серийный номер.");
            }
        }

        [HttpPost]
        [Route("ReplyTaskOpenOnNumber")]
        public async Task<ActionResult> ReplyTaskOpenOnNumber(PostModels.ReplyTaskOpenOnNumberPost model)
        {
            var s = await context.TaskWithOpenAnswsExecutions
                .FirstOrDefaultAsync(p => p.TestPackExecution.idChapterExecution == model.idExecChapter
                && p.TestPackExecution.TestPack.header == model.testPackHeader
                && p.TaskWithOpenAnsw.numericInPack == model.serialNumber);
            s.AnswearResult = model.answear;

            double mark = s.TaskWithOpenAnsw.AnswearOnTaskOpens.Sum(p => p.mark);
            int count = s.TaskWithOpenAnsw.AnswearOnTaskOpens.Count;

            string ans = model.answear;

            switch (s.TaskWithOpenAnsw.orderImportant)
            {
                case true:
                    {
                        if (count == 1)
                        {
                            if (s.TaskWithOpenAnsw.ResultType == ResultTypes.Label)
                            {
                                if (s.TaskWithOpenAnsw.AnswearOnTaskOpens.FirstOrDefault().answear != model.answear)
                                {
                                    mark = 0;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < s.TaskWithOpenAnsw.AnswearOnTaskOpens.FirstOrDefault().answear.Length; i++)
                                    if (ans[0] == s.TaskWithOpenAnsw.AnswearOnTaskOpens.FirstOrDefault().answear[i])
                                    {
                                        ans = ans.Remove(0, 1);
                                    }
                                    else
                                    {
                                        mark -= s.TaskWithOpenAnsw.fine;
                                    }
                            }
                        }
                        else if (count > 1)
                        {
                            foreach (var item in s.TaskWithOpenAnsw.AnswearOnTaskOpens)
                            {
                                if (ans.IndexOf(item.answear) == 0)
                                {
                                    ans = ans.Remove(0, item.answear.Length);
                                }
                                else
                                {
                                    ans = ans.Remove(0, item.answear.Length);
                                    mark -= s.TaskWithOpenAnsw.fine;
                                }
                            }
                        }
                        break;
                    }
                case false:
                    {
                        if (count == 1)
                        {
                            foreach (var item in s.TaskWithOpenAnsw.AnswearOnTaskOpens.FirstOrDefault().answear)
                                if (ans.Contains(item.ToString()))
                                {
                                    ans = ans.Remove(ans.IndexOf(item.ToString()), 1);
                                }
                                else
                                {
                                    mark -= s.TaskWithOpenAnsw.fine;
                                }

                            if (ans.Length != 0)
                            {
                                mark -= (s.TaskWithOpenAnsw.fine) * ans.Length;
                            }
                        }
                        else if (count > 1)
                        {
                            foreach (var item in s.TaskWithOpenAnsw.AnswearOnTaskOpens)
                            {
                                if (ans.Contains(item.answear))
                                {
                                    ans = ans.Remove(ans.IndexOf(item.ToString()), 1);
                                }
                                else
                                {
                                    mark -= s.TaskWithOpenAnsw.fine;
                                }
                            }
                            if (ans.Length != 0)
                            {
                                mark -= (s.TaskWithOpenAnsw.fine) * ans.Length;
                            }
                        }
                        break;
                    }
            }
            if (mark < 0)
                mark = 0;
            s.mark = mark;
            var date = DateTime.UtcNow.AddHours(5).AddDays(1).Date;
            if (s.mark == s.TaskWithOpenAnsw.AnswearOnTaskOpens.Sum(p => p.mark))
            {
                s.Status = StatusTaskExecution.Correct;
                await context.SaveChangesAsync();
                return Ok("Ответ верный.");
            }
            else if (s.mark > 0)
            {
                s.Status = StatusTaskExecution.PartCorrect;
                s.lockedTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                await context.SaveChangesAsync();
                return Ok("Ответ частично верный.");
            }
            s.Status = StatusTaskExecution.InCorrect;
            s.lockedTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            await context.SaveChangesAsync();
            return BadRequest("Ответ неверный");
        }

        [HttpPost]
        [Route("ReplyTaskClosedOnNumber")]
        public async Task<ActionResult> ReplyTaskClosedOnNumber(PostModels.ReplyTaskClosedOnNumberPost model)
        {
            var s = await context.TaskWithClosedAnswsExecutions
                .FirstOrDefaultAsync(p => p.TestPackExecution.idChapterExecution == model.idExecChapter
                && p.TestPackExecution.TestPack.header == model.testPackHeader
                && p.TaskWithClosedAnsw.numericInPack == model.serialNumber);
            s.AnswearOnTask = await context.AnswearOnTasks.FirstOrDefaultAsync(p => p.idAnswearOnTask == model.idAnswear);
            if (s.TaskWithClosedAnsw.AnswearOnTask.FirstOrDefault(p => p.accuracy)?.idAnswearOnTask == model.idAnswear)
            {
                s.Status = StatusTaskExecution.Correct;
                await context.SaveChangesAsync();
                return Ok("Ответ верный.");
            }
            s.Status = StatusTaskExecution.InCorrect;
            await context.SaveChangesAsync();
            return BadRequest("Ответ неверный");
        }

        //[HttpPost]
        //[Route("GetNextTaskOpenOnNumber")]
        //public async Task<ActionResult> GetNextTaskOpenOnNumber(PostTestModels.GetNextTaskOpenOnNumberPost model)
        //{
        //    try
        //    {
        //        var s = context.TaskWithOpenAnswsExecutions.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).ToList();
        //        if (s.Where(p => p.TaskWithOpenAnsws.theme == model.theme).FirstOrDefault() == null)
        //        {
        //            return BadRequest("Неверная тематика.");
        //        }
        //        Models.TaskWithOpenAnswsExecution mod = new Models.TaskWithOpenAnswsExecution();
        //        if (model.theme != null)
        //        {
        //            for (int i = model.currentSerialNumber; i < s.Count; i++)
        //            {
        //                if (s[i].TaskWithOpenAnsws.theme == model.theme)
        //                    mod = s[i];
        //            }
        //            if (mod.idTaskWithOpenAnswsExecution == 0)
        //            { return BadRequest("Задания по данной тематике закончились."); }
        //        }
        //        else
        //            mod = s[model.currentSerialNumber];
        //        return Ok(new { serialNumber = model.currentSerialNumber + 1, answear = mod.TaskWithOpenAnsws.answear, question = mod.TaskWithOpenAnsws.textQuestion, solution = mod.TaskWithOpenAnsws.Solutions.Select(s => new { url = s.url }) });
        //    }
        //    catch
        //    {
        //        return BadRequest("Неверный серийный номер или тематика.");
        //    }
        //}

        [HttpPost]
        [Route("begintest")]
        public async Task<ActionResult> BeginTest(PostTestModels.BeginTestModel model)
        {
            if (model.mode == null)
            {
                var mod = await context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter 
                && p.TestPackExecution.TestPack.header == model.testPackHeader)
                    .OrderByDescending(p => p.idTryingTestTask)
                    .FirstOrDefaultAsync();

                if (mod != null && mod.status == "Начат")
                {
                    return Ok("Есть незаконченный тест, хотите продолжить его? Или начнете заново?");
                }
                else
                {
                    return await BeginTestSecond(model);
                }
            }
            else if (model.mode == "continue")
            {
                return Ok(await context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter)
                    .OrderByDescending(p => p.idTryingTestTask)
                    .Select(p => new
                {
                    TestTaskExecutions = p.TestTaskExecutions.Select(s => new
                    {
                        idTestTaskExecution = s.idTestTaskExecution,
                        answearOnTask = s.AnswearOnTask == null ? null : new
                        {
                            idAnswearOnTask = s.AnswearOnTask.idAnswearOnTask,
                            text = s.AnswearOnTask.textAnswear,
                            status = s.GetStatus()
                        },
                        TestPack = new
                        {
                            s.TestTask.textQuestion,
                            AnswearOnTasks = s.TestTask.AnswearOnTasks.Select(m => new
                            {
                                m.idAnswearOnTask,
                                m.textAnswear
                            })
                        }
                    })
                }).FirstOrDefaultAsync());
            }
            else if (model.mode == "new")
            {
                var m = await context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter 
                && p.TestPackExecution.TestPack.header == model.testPackHeader)
                    .OrderByDescending(p => p.idTryingTestTask)
                    .FirstOrDefaultAsync();
                m.status = "Завершен";
                m.result = m.TestTaskExecutions.Count(p =>
                {
                    if (p.AnswearOnTask != null)
                        return p.AnswearOnTask.accuracy;
                    else
                        return false;
                });
                context.SaveChanges();
                return await BeginTestSecond(model);
            }
            else
            {
                return BadRequest("Неверный режим.");
            }
        }

        [HttpPost]
        [Route("endtest")]
        public async Task<ActionResult> EndTest(PostTestModels.BeginTestModel model)
        {
            try
            {
                var m = await context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter 
                && p.TestPackExecution.TestPack.header == model.testPackHeader)
                    .OrderByDescending(p => p.idTryingTestTask)
                    .FirstOrDefaultAsync();
                m.status = "Завершен";
                m.result = m.TestTaskExecutions.Count(p =>
                {
                    if (p.AnswearOnTask != null)
                        return p.AnswearOnTask.accuracy;
                    else
                        return false;
                });
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Тест успешно завершен.");
        }

        [HttpPost]
        [Route("replytesttask")]
        public async Task<ActionResult> ReplyTestTask(ReplyTestTask model)
        {
            var testtask = await context.TestTaskExecutions.FirstOrDefaultAsync(p => p.idTestTaskExecution == model.idTestTaskExecution);
            testtask.AnswearOnTask = context.AnswearOnTasks.FirstOrDefault(p => p.idAnswearOnTask == model.idAnswearOnTask);
            if (testtask.AnswearOnTask.accuracy)
            {
                testtask.StatusExecution = StatusExecution.Correct;
                await context.SaveChangesAsync();
                return Ok("Верно");
            }
            else
            {
                testtask.StatusExecution = StatusExecution.InCorrect;
                await context.SaveChangesAsync();
                return Ok("Не верно");

            }
        }

        private async Task<ActionResult> BeginTestSecond(PostTestModels.BeginTestModel model)
        {
            var s = new TryingTestTask { };
            var ms = context.ChapterExecutions.FirstOrDefault(p => p.idChapterExecution == model.idExecChapter);
            s.status = "Начат";
            s.TestPackExecution = context.ChapterExecutions.FirstOrDefault(p => p.idChapterExecution == model.idExecChapter)?.TestPackExecutions.FirstOrDefault();
            foreach (var vvv in s.TestPackExecution.TestPack.TestTasks)
            {
                s.TestTaskExecutions.Add(new Models.TestTaskExecution { TestTask = vvv });
            }
            context.TryingTestTasks.Add(s);
            await context.SaveChangesAsync();
            return Ok(context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter).OrderByDescending(p => p.idTryingTestTask).Select(p => new
            {
                TestTaskExecutions = p.TestTaskExecutions.Select(s => new
                {
                    idTestTaskExecution = s.idTestTaskExecution,
                    status = s.GetStatus(),
                    answearOnTask = s.AnswearOnTask == null ? null : new
                    {
                        idAnswearOnTask = s.AnswearOnTask.idAnswearOnTask,
                        text = s.AnswearOnTask.textAnswear
                    },
                    TestPack = new
                    {
                        s.TestTask.textQuestion,
                        AnswearOnTasks = s.TestTask.AnswearOnTasks.Select(m => new
                        {
                            m.idAnswearOnTask,
                            m.textAnswear
                        })
                    }
                })
            }).FirstOrDefault());
        }
    }

    public class ReplyTestTask
    {
        public int idTestTaskExecution { get; set; }
        public int idAnswearOnTask { get; set; }
    }
}
