using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                        var packexec = chapexec.TestPackExecutions.Where(p => p.idTestPack == mod4.idTestPack).FirstOrDefault();
                        foreach (var mod3 in mod4.TaskWithOpenAnsws)
                        {
                            if (packexec.TaskWithOpenAnswsExecutions.Where(p => p.idTaskWithOpenAnsws == mod3.idTaskWithOpenAnsw).FirstOrDefault() == null)
                            {
                                packexec.TaskWithOpenAnswsExecutions.Add(new Models.TaskWithOpenAnswsExecution { TaskWithOpenAnsws = mod3, status = "Ожидает решения" });
                            }
                            var taskexec = packexec.TaskWithOpenAnswsExecutions.Where(p => p.TaskWithOpenAnsws == mod3).FirstOrDefault();
                        }
                    }
                }
            }
            context.SaveChanges();
            return Ok(exec.SubjectExecutions.Select(p => new { idExec = p.idSubjectExecution, NameSub = p.Subject.nameSubject}));
        }

        [HttpPost]
        [Route("getChapters")]
        public async Task<ActionResult> GetChapters(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = context.ChapterExecutions.Where(p => p.idSubjectExecution == chaptersPost.idSubjectExecution).ToList();
            return Ok(s.Select(p => new { NameChapter = p.Chapter.name, Description = p.Chapter.Description, idExec = p.idChapterExecution, getProcentChapterDecide = p.getProcentChapter, access = p.Chapter.access }));
        }

        [HttpPost]
        [Route("getChapter")]
        public async Task<ActionResult> GetChapter(PostTestModels.GetChaptersPost chaptersPost)
        {
            var s = context.ChapterExecutions.Where(p => p.idChapterExecution == chaptersPost.idSubjectExecution).ToList();
            return Ok(s.Select(p => new { NameChapter = p.Chapter.name, Description = p.Chapter.Description, idExec = p.idChapterExecution, getProcentChapterDecide = p.getProcentChapter, getMaxProcentTest = p.getProcentChapterTest, getProcentDecideTaskWithOpen = p.getProcentChapterTask, TestPack= p.TestPackExecutions.Select(p=> new { TaskWithOpenAnsws = p.TaskWithOpenAnswsExecutions.Select(s => new { idExecTaskOpen = s.idTaskWithOpenAnswsExecution, status = s.status, serialNumber = p.TaskWithOpenAnswsExecutions.IndexOf(s) + 1, theme = s.TaskWithOpenAnsws.theme }) }) , theory = p.Chapter.TheoreticalMaterials, access = p.Chapter.access }));
        }

        [HttpPost]
        [Route("GetTaskOpenOnNumber")]
        public async Task<ActionResult> GetTaskOpenOnNumber(PostTestModels.GetTaskOpenOnNumberPost model)
        {
            try
            {
                var s = context.TaskWithOpenAnswsExecutions.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.headerTestPack).ToList();
                var mod = s[model.serialNumber - 1];
                return Ok(new { serialNumber = model.serialNumber, answear = mod.TaskWithOpenAnsws.answear, question = mod.TaskWithOpenAnsws.textQuestion, solution = mod.TaskWithOpenAnsws.Solutions.Select(s => new { url = s.url }) });
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
            var s = context.TaskWithOpenAnswsExecutions.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).ToList();
            var mod = s[model.serialNumber - 1];
            if (mod.TaskWithOpenAnsws.answear.ToLower().Trim() == model.answear.ToLower().Trim())
            {
                mod.status = "Решено верно.";
                context.SaveChanges();
                return Ok("Ответ верный.");
            }
            mod.status = "Решено неверно.";
            context.SaveChanges();
            return BadRequest("Ответ неверный");
        }

        [HttpPost]
        [Route("GetNextTaskOpenOnNumber")]
        public async Task<ActionResult> GetNextTaskOpenOnNumber(PostTestModels.GetNextTaskOpenOnNumberPost model)
        {
            try
            {
                var s = context.TaskWithOpenAnswsExecutions.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).ToList();
                if(s.Where(p=>p.TaskWithOpenAnsws.theme == model.theme).FirstOrDefault()==null)
                {
                    return BadRequest("Неверная тематика.");
                }
                Models.TaskWithOpenAnswsExecution mod = new Models.TaskWithOpenAnswsExecution();
                if (model.theme != null)
                {
                    for (int i = model.currentSerialNumber; i < s.Count; i++)
                    {
                        if (s[i].TaskWithOpenAnsws.theme == model.theme)
                            mod = s[i];
                    }
                    if (mod.idTaskWithOpenAnswsExecution == 0)
                    { return BadRequest("Задания по данной тематике закончились."); }
                }
                else
                    mod = s[model.currentSerialNumber];
                return Ok(new { serialNumber = model.currentSerialNumber + 1, answear = mod.TaskWithOpenAnsws.answear, question = mod.TaskWithOpenAnsws.textQuestion, solution = mod.TaskWithOpenAnsws.Solutions.Select(s => new { url = s.url }) });
            }
            catch
            {
                return BadRequest("Неверный серийный номер или тематика.");
            }
        }

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
                return Ok(context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault());
            }
            else if (model.mode == "new")
            {
                var m = context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault();
                m.status = "Завершен";
                m.result = m.TestTaskExecutions.Count(p => p.AnswearOnTask.accuracy);
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
            var m = context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter && p.TestPackExecution.TestPack.header == model.testPackHeader).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault();
            m.status = "Завершен";
            m.result = m.TestTaskExecutions.Count(p => p.AnswearOnTask.accuracy);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("replytesttask")]
        public async Task<ActionResult> ReplyTestTask(PostTestModels.BeginTestModel model)
        {
            return Ok();
        }

        private async Task<ActionResult> BeginTestSecond(PostTestModels.BeginTestModel model)
        {
            var s = new Models.TryingTestTask { };
            s.TestPackExecution.idChapterExecution = model.idExecChapter;
            foreach (var vvv in context.ChapterExecutions.Where(p => p.idChapter == model.idExecChapter).FirstOrDefault().Chapter.TestPacks.Where(p=>p.header == model.testPackHeader).FirstOrDefault().TestTasks)
            {
                s.TestTaskExecutions.Add(new Models.TestTaskExecution { TestTask = vvv });
            }
            context.TryingTestTasks.Add(s);
            context.SaveChanges();
            return Ok(context.TryingTestTasks.Where(p => p.TestPackExecution.idChapterExecution == model.idExecChapter).OrderByDescending(p => p.idTryingTestTask).FirstOrDefault());
        }
    }
}
