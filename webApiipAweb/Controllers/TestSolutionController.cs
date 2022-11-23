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
        private Models.context context;
        private readonly UserManager<Models.Child> _userManager;
        private readonly SignInManager<Models.Child> _signInManager;
        public TestSolutionController(Models.context _context, UserManager<Models.Child> userManager, SignInManager<Models.Child> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            context = _context;
        }

        [HttpPost]
        [Route("getSubjects")]
        public async Task<ActionResult> GetSubjects(PostTestModels.GetSubjectPost subjectPost)
        {
            var child = context.Children.Where(p => p.Id == subjectPost.ChildId).FirstOrDefault();
            if (child == null)
                return BadRequest("Не найдено.");
            var exec = child.LevelStudingExecutions.Where(p => p.LevelStuding.nameLevel == child.levelStuding.ToString()).FirstOrDefault();
            foreach (var mod1 in context.LevelStudings.Where(p => p.nameLevel == child.levelStuding.ToString()).FirstOrDefault().Subjects)
            {

                if (exec.SubjectExecutions.Where(p => p.idSubject == mod1.idSubject).FirstOrDefault() == null)
                {
                    exec.SubjectExecutions.Add(new Models.SubjectExecution { Subject = mod1 });
                }

                var subexec = exec.SubjectExecutions.Where(p => p.Subject == mod1).FirstOrDefault();
                foreach (var mod2 in mod1.Chapters)
                {
                    if (subexec.ChapterExecutions.Where(p => p.idChapter == mod2.idChapter).FirstOrDefault() == null)
                    {
                        subexec.ChapterExecutions.Add(new Models.ChapterExecution { Chapter = mod2 });
                    }
                    var chapexec = subexec.ChapterExecutions.Where(p => p.Chapter == mod2).FirstOrDefault();
                    foreach (var mod4 in mod2.TestPacks)
                    {
                        if (chapexec.TestPackExecutions.Where(p => p.idTestPack == mod4.idTestPack).FirstOrDefault() == null)
                        {
                            chapexec.TestPackExecutions.Add(new Models.TestPackExecution { TestPack = mod4 });
                        }
                        var packexec = chapexec.TestPackExecutions.Where(p => p.TestPack == mod4).FirstOrDefault();
                        foreach (var mod3 in mod4.TaskWithOpenAnsws)
                        {
                            if (packexec.TaskWithOpenAnswsExecutions.Where(p => p.idTask == mod3.idTask).FirstOrDefault() == null) //???
                            {
                                packexec.TaskWithOpenAnswsExecutions.Add(new Models.TaskWithOpenAnswsExecution { TaskWithOpenAnsw = mod3, Status = StatusTaskExecution.AwaitingExecution });
                            }
                            var taskexec = packexec.TaskWithOpenAnswsExecutions.Where(p => p.TaskWithOpenAnsw == mod3).FirstOrDefault();
                        }
                        foreach (var mod3 in mod4.TaskWithClosedAnsws)
                        {
                            if (packexec.TaskWithClosedAnswsExecutions.Where(p => p.idTask == mod3.idTask).FirstOrDefault() == null) //???
                            {
                                packexec.TaskWithClosedAnswsExecutions.Add(new Models.TaskWithClosedAnswsExecution { TaskWithClosedAnsw = mod3, Status = StatusTaskExecution.AwaitingExecution });
                            }
                            var taskexec = packexec.TaskWithClosedAnswsExecutions.Where(p => p.TaskWithClosedAnsw == mod3).FirstOrDefault();
                        }
                    }
                }
            }
            await context.SaveChangesAsync();
            return Ok(exec.SubjectExecutions.Select(p => new { idExec = p.idSubjectExecution, NameSub = p.Subject.nameSubject, ProgressInProcent = p.getProgressInProcent }));
        }

        [HttpPost]
        [Route("getChapters")]
        public async Task<ActionResult> GetChapters(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = await context.ChapterExecutions.Where(p => p.idSubjectExecution == chaptersPost.idSubjectExecution).ToListAsync();
            return Ok(s.Select(p => new { NameChapter = p.Chapter.name, Description = p.Chapter.Description, getProcentChapterDecide = p.getProcentMainTasks, MainPackProcent = p.getProcentMainTasks, OtherPackProcent = p.getProcentOtherTasks, idExec = p.idChapterExecution, access = p.Chapter.access }));
        }

        [HttpPost]
        [Route("getChapter")]
        public async Task<ActionResult> GetChapter(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = await context.ChapterExecutions.Where(p => p.idChapterExecution == chaptersPost.idSubjectExecution).ToListAsync();
            return Ok(s.Select(p => new
            {
                NameChapter = p.Chapter.name,
                Description = p.Chapter.Description,
                idExec = p.idChapterExecution,
                getProcentChapterDecide = p.getProcentChapter,
                MainPackProcent = p.getProcentMainTasks,
                OtherPackProcent = p.getProcentOtherTasks,

                TestPack = p.TestPackExecutions.Select(p => new
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
                theory = p.Chapter.TheoreticalMaterials.Select(l => new
                {
                    header = l.header,
                    content = l.content
                }),
                access = p.Chapter.access,
            }));
        }

        [HttpPost]
        [Route("GetTaskOpenOnNumber")]
        public async Task<ActionResult> GetTaskOpenOnNumber(PostTestModels.GetTaskOpenOnNumberPost model)
        {
            try
            {
                var s = await context.TestPackExecutions.Where(p => p.idChapterExecution == model.idExecChapter && p.TestPack.header == model.headerTestPack).FirstOrDefaultAsync();
                var closed = s.TaskWithClosedAnswsExecutions.Where(p => p.TaskWithClosedAnsw.numericInPack == model.serialNumber).FirstOrDefault();
                var opened = s.TaskWithOpenAnswsExecutions.Where(p => p.TaskWithOpenAnsw.numericInPack == model.serialNumber).FirstOrDefault();
                int? id = opened?.idTask ?? closed?.idTask;
                string imageUrl = String.Empty;
                //var request = await http.GetAsync($"http://192.168.147.72:83/api/userprofileimage/task?name=task{id}.jpeg");
                //if (request.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                imageUrl = "http://192.168.147.72:83/" + $"task{id}.jpeg";
                //}

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
                        status = closed.GetStatus(),
                        type = closed.TaskWithClosedAnsw.TypesTask.ToString(),
                        theme = closed.TaskWithClosedAnsw.theme,
                        solutions = closed.TaskWithClosedAnsw.Solutions.Select(p => new
                        {
                            url = "http://192.168.147.72:83/" + $"sol{p.idSolution}.jpeg"
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
                            url = "http://192.168.147.72:83/" + $"sol{p.idSolution}.jpeg"
                        }),
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
            var s = context.TaskWithOpenAnswsExecutions
                .Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter
                && p.TestPackExecution.TestPack.header == model.testPackHeader
                && p.TaskWithOpenAnsw.numericInPack == model.serialNumber)
                .FirstOrDefault();
            s.AnswearResult = model.answear;

            double mark = s.TaskWithOpenAnsw.AnswearOnTaskOpens.Sum(p => p.mark);
            int count = s.TaskWithOpenAnsw.AnswearOnTaskOpens.Count();

            string ans = model.answear;

            switch (s.TaskWithOpenAnsw.orderImportant)
            {
                case true:
                    {
                        if (count == 1)
                        {
                            if (s.TaskWithOpenAnsw.AnswearOnTaskOpens.FirstOrDefault().answear != model.answear)
                            {
                                mark = 0;
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

            if (s.mark == s.TaskWithOpenAnsw.AnswearOnTaskOpens.Sum(p => p.mark))
            {
                s.Status = StatusTaskExecution.Correct;
                await context.SaveChangesAsync();
                return Ok("Ответ верный.");
            }
            else if (s.mark > 0)
            {
                s.Status = StatusTaskExecution.PartCorrect;
                await context.SaveChangesAsync();
                return Ok("Ответ частично верный.");
            }
            s.Status = StatusTaskExecution.InCorrect;
            await context.SaveChangesAsync();
            return BadRequest("Ответ неверный");
        }

        [HttpPost]
        [Route("ReplyTaskClosedOnNumber")]
        public async Task<ActionResult> ReplyTaskClosedOnNumber(PostModels.ReplyTaskClosedOnNumberPost model)
        {
            ///Добавь сюда сохарнеие результата без зависимости от правильности
            var s = context.TaskWithClosedAnswsExecutions
                .Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter
                && p.TestPackExecution.TestPack.header == model.testPackHeader
                && p.TaskWithClosedAnsw.numericInPack == model.serialNumber)
                .FirstOrDefault();
            s.AnswearOnTask = context.AnswearOnTasks.Where(p => p.idAnswearOnTask == model.idAnswear).FirstOrDefault();
            if (s.TaskWithClosedAnsw.AnswearOnTask.Where(p => p.accuracy).FirstOrDefault().idAnswearOnTask == model.idAnswear)
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
                var mod = context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault();
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
                return Ok(context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter).OrderByDescending(p => p.idTryingTestTask).Select(p => new
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
                }).FirstOrDefault());
            }
            else if (model.mode == "new")
            {
                var m = context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault();
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
                var m = context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault();
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
            var testtask = context.TestTaskExecutions.Where(p => p.idTestTaskExecution == model.idTestTaskExecution).FirstOrDefault();
            testtask.AnswearOnTask = context.AnswearOnTasks.Where(p => p.idAnswearOnTask == model.idAnswearOnTask).FirstOrDefault();
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
            var s = new Models.TryingTestTask { };
            var ms = context.ChapterExecutions.Where(p => p.idChapterExecution == model.idExecChapter).FirstOrDefault();
            s.status = "Начат";
            s.TestPackExecution = context.ChapterExecutions.Where(p => p.idChapterExecution == model.idExecChapter).FirstOrDefault().TestPackExecutions.FirstOrDefault();
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
