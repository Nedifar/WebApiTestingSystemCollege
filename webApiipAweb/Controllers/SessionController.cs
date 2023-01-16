using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private Models.context context;
        private readonly UserManager<Models.Child> _userManager;
        private readonly SignInManager<Models.Child> _signInManager;
        public SessionController(Models.context _context, UserManager<Models.Child> userManager, SignInManager<Models.Child> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            context = _context;
        }

        [HttpPost]
        [Route("EndSession")]
        public async Task<ActionResult> EndSession(PostTestModels.EndSessionViewModel endSession)
        {
            var s = await context.ChapterExecutions.Where(p => p.idChapterExecution == endSession.idChapterExecution).ToListAsync();
            var currentSession = s?.FirstOrDefault()
                ?.SubjectExecution.LevelStudingExecution.Child.SessionChapterExecutions
                .Where(p => p.idChapter == s?.FirstOrDefault().idChapter)
                .OrderByDescending(p => p.idSessionChapterExecution).FirstOrDefault();
            if (currentSession == null || !currentSession.activeSession)
            {
                return BadRequest("Сессии не существует.");
            }
            currentSession.endDateTime = DateTime.UtcNow.AddHours(5);
            currentSession.activeSession = false;
            var taskExecution = s.FirstOrDefault().TestPackExecutions.FirstOrDefault().GetTasksExecution();
            int counter = 1;
            foreach (var task in taskExecution)
            {
                currentSession.SessionProgresses.Add(new()
                {
                    taskNumber = counter,
                    StatusTaskExecution = task.Status
                });
                counter++;
            }
            context.SaveChanges();
            return Ok("Сессия завершена");
        }

        [HttpGet]
        [Route("getSessions")]
        public async Task<ActionResult> GetAllSessions()
        {
            var sessions = await context.SessionChapterExecutions.ToListAsync();
            return Ok(sessions.Select(session => new
            {
                beginDateTime = session.beginDateTime,
                endDateTime = session.endDateTime,
                timeExecution = (session.endDateTime-session.endDateTime).Value,
                status = session.activeSession ? "Активна" : "Завершена",
                nameChapter = session.Chapter.name,
                SessionProgresses = session.SessionProgresses.OrderBy(p => p.taskNumber).Select(sessionProgress => new
                {
                    taskNumber = sessionProgress.taskNumber,
                    status = sessionProgress.GetStatus()
                }),
                FinishResults = session.SessionProgresses.OrderBy(p=>p.taskNumber).Select(sessionProgress => new
                {
                    taskNumber = sessionProgress.taskNumber,
                    status = sessionProgress.GetStatus()== "Ожидает выполнения"?"Решено неверно": sessionProgress.GetStatus()
                })
            }));
        }
    }
}
