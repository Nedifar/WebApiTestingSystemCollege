using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return Ok(exec.SubjectExecutions.Select(p => new { idExec = p.idSubjectExecution, NameSub = p.Subject.nameSubject }));
        }

        [HttpPost]
        [Route("getChapters")]
        public async Task<ActionResult> GetChapters(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = context.ChapterExecutions.Where(p => p.idSubjectExecution == chaptersPost.idSubjectExecution).ToList();
            return Ok(s.Select(p => new { NameChapter = p.Chapter.name, Description = p.Chapter.Description, getProcentChapterDecide = p.getProcentChapter, getMaxProcentTest = p.getProcentChapterTest, getProcentDecideTaskWithOpen = p.getProcentChapterTask, idExec = p.idChapterExecution, access = p.Chapter.access }));
        }

        [HttpPost]
        [Route("getChapter")]
        public async Task<ActionResult> GetChapter(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = context.ChapterExecutions.Where(p => p.idChapterExecution == chaptersPost.idSubjectExecution).ToList();
            return Ok(s.Select(p => new
            { 
                NameChapter = p.Chapter.name, 
                Description = p.Chapter.Description, 
                idExec = p.idChapterExecution, 
                getProcentChapterDecide = p.getProcentChapter,
                getMaxProcentTest = p.getProcentChapterTest, 
                getProcentDecideTaskWithOpen = p.getProcentChapterTask, 
                TestPack = p.TestPackExecutions.Select(p => new
                {
                    Header = p.TestPack.header,
                    haveFinalTest = p.haveFinalTest,
                    accessFinalTest = p.accessProcentFinalTest,
                    Tasks = p.GetTasksExecution().Select(l => new
                    {
                        idTaskExecution = l.idTaskExecution,
                        idTestPackExucution = l.idTestPackExecution,
                        status = l.GetStatus(),
                        serialNumber = p.GetTasksExecution().IndexOf(l) + 1
                    })
                }), 
                theory = p.Chapter.TheoreticalMaterials, 
                access = p.Chapter.access }));
        }

        [HttpPost]
        [Route("GetTaskOpenOnNumber")]
        public async Task<ActionResult> GetTaskOpenOnNumber(PostTestModels.GetTaskOpenOnNumberPost model)
        {
            try
            {
                var s = context.TestPackExecutions.Where(p => p.idChapterExecution == model.idExecChapter && p.TestPack.header == model.headerTestPack).FirstOrDefault();
                var closed = s.TaskWithClosedAnswsExecutions.Where(p => p.TaskWithClosedAnsw.numericInPack == model.serialNumber).FirstOrDefault();
                var opened = s.TaskWithOpenAnswsExecutions.Where(p => p.TaskWithOpenAnsw.numericInPack == model.serialNumber).FirstOrDefault();
                if (closed is not null)
                {
                    return Ok(new
                    {
                        serialNumber = model.serialNumber,
                        selectedAnswear = closed.AnswearOnTask?.idAnswearOnTask,
                        Answears = closed.TaskWithClosedAnsw.AnswearOnTask.Select(p => new
                        {
                            text = p.textAnswear,
                            idAnswear = p.idAnswearOnTask
                        }),
                        status = closed.GetStatus()
                    });
                }
                
                else if(opened is not null)
                {
                    return Ok(new
                    {
                        serialNumber = model.serialNumber,
                        selectedAnswear = opened.AnswearResult,
                        status = closed.GetStatus()
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
            if (s.TaskWithOpenAnsw.answear.ToLower().Trim() == model.answear.ToLower().Trim())
            {
                s.Status = StatusTaskExecution.Correct;
                await context.SaveChangesAsync();
                return Ok("Ответ верный.");
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
            if (s.TaskWithClosedAnsw.AnswearOnTask.Where(p=>p.accuracy).FirstOrDefault().idAnswearOnTask == model.idAnswear)
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
