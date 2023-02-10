using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using webApiipAweb.Models;
using static System.Collections.Specialized.BitVector32;

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
        [Route("EndTheorySession")]
        public async Task<ActionResult> EndTheorySession(PostTestModels.EndSessionViewModel endSession)
        {
            var currentChapterExecution = context.ChapterExecutions.FirstOrDefault(p => p.idChapterExecution == endSession.idChapterExecution);
            if (currentChapterExecution == null)
                return NotFound("Такого раздела не существует.");
            var currentSession = currentChapterExecution.TheorySessions.OrderByDescending(p => p.idTheorySession)
                .FirstOrDefault(p=>p.active);
            if (currentSession == null)
                return NotFound("Активной сессии не существует.");
            currentSession.active = false;
            currentSession.endDate = DateTime.UtcNow.AddHours(5);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("EndSession")]
        public async Task<ActionResult> EndSession(PostTestModels.EndSessionViewModel endSession)
        {
            var s = await context.ChapterExecutions.Where(p => p.idChapterExecution == endSession.idChapterExecution).ToListAsync();
            var currentSession = s?.FirstOrDefault()
                ?.SubjectExecution.LevelStudingExecution.Child.SessionChapterExecutions
                .Where(p => p.idChapterExecution == s?.FirstOrDefault().idChapterExecution)
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
            var childs = context.Children.ToList();
            var sessions = await context.SessionChapterExecutions.ToListAsync();
            foreach (var session in sessions.Where(p => p.activeSession))
            {
                if (session.beginDateTime.Value.AddHours(1) < DateTime.UtcNow.AddHours(5) && session.activeSession)
                {
                    session.activeSession = false;
                    session.endDateTime = session.beginDateTime.Value.AddHours(1);
                    var selectedChapterExecution = session.ChapterExecution;
                    var taskExecution = selectedChapterExecution.TestPackExecutions.FirstOrDefault().GetTasksExecution();
                    int counter = 1;
                    foreach (var task in taskExecution)
                    {
                        session.SessionProgresses.Add(new()
                        {
                            taskNumber = counter,
                            StatusTaskExecution = task.Status
                        });
                        counter++;
                    }
                }
            }
            context.SaveChanges();
            return Ok(childs.Select(child => new
            {
                child = child.lastName + " " + child.firstName,
                level = child.LevelStudingExecutions.Select(level => new
                {
                    level = level.LevelStuding.nameLevel,
                    subject = level.SubjectExecutions.Select(subject => new
                    {
                        name = subject.Subject.nameSubject,
                        chapter = subject.ChapterExecutions.Select(chapter => new
                        {
                            name = chapter.Chapter.name,
                            sessions = chapter.SessionChapterExecutions.Select(session => new
                            {
                                idSes = session.idSessionChapterExecution,
                                beginDateTime = session.beginDateTime,
                                endDateTime = session.endDateTime,
                                timeExecution = ((session.endDateTime??DateTime.UtcNow.AddHours(5)) - session.beginDateTime).Value,
                                status = session.activeSession ? "Активна" : "Завершена",
                                nameChapter = session.ChapterExecution.Chapter.name,

                                SessionProgresses = session.SessionProgresses.OrderBy(p => p.taskNumber).Select(sessionProgress => new
                                {
                                    taskNumber = sessionProgress.taskNumber,
                                    status = sessionProgress.GetStatus()
                                }),
                                
                            }),
                            FinishResults = chapter.SessionChapterExecutions.OrderByDescending(p=>p.idSessionChapterExecution).FirstOrDefault()
                            ?.SessionProgresses.OrderBy(p => p.taskNumber).Select(sessionProgress => new
                            {
                                taskNumber = sessionProgress.taskNumber,
                                status = sessionProgress.GetStatus() == "Ожидает выполнения" ? "Решено неверно" : sessionProgress.GetStatus()
                            })
                        })
                    })
                })
            }));
        }

        [HttpGet]
        [Route("getsessionstheory")]
        public async Task<ActionResult> GetAllSessionsToTheory()
        {
            var childs = context.Children.ToList();
            var sessions = await context.TheorySessions.ToListAsync();
            foreach (var session in sessions.Where(p => p.active))
            {
                if (session.beginDate.Value.AddHours(1) < DateTime.UtcNow.AddHours(5) && session.active)
                {
                    session.active = false;
                    session.endDate = session.beginDate.Value.AddHours(1);
                }
            }
            context.SaveChanges();
            return Ok(childs.Select(child => new
            {
                child = child.lastName + " " + child.firstName,
                level = child.LevelStudingExecutions.Select(level => new
                {
                    level = level.LevelStuding.nameLevel,
                    subject = level.SubjectExecutions.Select(subject => new
                    {
                        name = subject.Subject.nameSubject,
                        chapter = subject.ChapterExecutions.Select(chapter => new
                        {
                            name = chapter.Chapter.name,
                            sessions = chapter.TheorySessions.Select(session => new
                            {
                                beginDateTime = session.beginDate,
                                endDateTime = session.endDate,
                                timeExecution = ((session.endDate ?? DateTime.UtcNow.AddHours(5)) - session.beginDate).Value,
                                status = session.active ? "Активна" : "Завершена",
                                nameChapter = session.ChapterExecution.Chapter.name,
                            }),
                        })
                    })
                })
            }));
        }

        [HttpPost]
        [Route("StartTheorySession")]
        public async Task<ActionResult> StartTheorySession(PostTestModels.EndSessionViewModel startTheorySession)
        {
            var s = await context.ChapterExecutions.FirstOrDefaultAsync(p => p.idChapterExecution == startTheorySession.idChapterExecution);
            var currentTheorySession = context.ChapterExecutions.FirstOrDefault(p => p.idChapterExecution == s.idChapterExecution)
                .TheorySessions.OrderByDescending(p => p.idTheorySession).FirstOrDefault();
            if (currentTheorySession != null && currentTheorySession.beginDate.Value.AddHours(1) < DateTime.UtcNow.AddHours(5) && currentTheorySession.active)
            {
                currentTheorySession.active = false;
                currentTheorySession.endDate = currentTheorySession.beginDate.Value.AddHours(1);
            }
            if (currentTheorySession == null || currentTheorySession.active == false)
            {
                currentTheorySession = new TheorySession
                {
                    active = true,
                    beginDate = DateTime.UtcNow.AddHours(5),
                    endDate = null,
                    ChapterExecution = s,
                    Child = s?.SubjectExecution.LevelStudingExecution.Child
                };
                context.TheorySessions.Add(currentTheorySession);
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Route("StartTaskSession")]
        public async Task<ActionResult> StartTaskSession(PostTestModels.EndSessionViewModel startSession)
        {
            var s = await context.ChapterExecutions.FirstOrDefaultAsync(p => p.idChapterExecution == startSession.idChapterExecution);
            var currentSession = s.SessionChapterExecutions
                .OrderByDescending(p => p.idSessionChapterExecution).FirstOrDefault();
            if (currentSession != null && currentSession.beginDateTime.Value.AddHours(1) < DateTime.UtcNow.AddHours(5) && currentSession.activeSession)
            {
                currentSession.activeSession = false;
                currentSession.endDateTime = currentSession.beginDateTime.Value.AddHours(1);
                var selectedChapterExecution = currentSession.ChapterExecution;
                var taskExecution = selectedChapterExecution.TestPackExecutions.FirstOrDefault().GetTasksExecution();
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
            }
            if (currentSession == null || currentSession.activeSession == false)
            {
                currentSession = new SessionChapterExecution
                {
                    activeSession = true,
                    beginDateTime = DateTime.UtcNow.AddHours(5),
                    endDateTime = null,
                    ChapterExecution = s,
                    Child = s?.SubjectExecution.LevelStudingExecution.Child
                };
                context.SessionChapterExecutions.Add(currentSession);
                context.SaveChanges();
            }
            return Ok();
        }
        [Route("getSessionsOnSchool")]
        [Authorize(Roles = "AdminSchool")]
        public async Task<ActionResult> GetSessionsOnSchool()
        {
            var childs = context.Children.ToList();
            var sessions = await context.SessionChapterExecutions.ToListAsync();
            foreach (var session in sessions.Where(p => p.activeSession))
            {
                if (session.beginDateTime.Value.AddHours(1) < DateTime.UtcNow.AddHours(5) && session.activeSession)
                {
                    session.activeSession = false;
                    session.endDateTime = session.beginDateTime.Value.AddHours(1);
                    var selectedChapterExecution = session.ChapterExecution;
                    var taskExecution = selectedChapterExecution.TestPackExecutions.FirstOrDefault().GetTasksExecution();
                    int counter = 1;
                    foreach (var task in taskExecution)
                    {
                        session.SessionProgresses.Add(new()
                        {
                            taskNumber = counter,
                            StatusTaskExecution = task.Status
                        });
                        counter++;
                    }
                }
            }
            context.SaveChanges();
            return Ok(childs.Select(child => new
            {
                child = child.lastName + " " + child.firstName,
                level = child.LevelStudingExecutions.Select(level => new
                {
                    level = level.LevelStuding.nameLevel,
                    subject = level.SubjectExecutions.Select(subject => new
                    {
                        name = subject.Subject.nameSubject,
                        chapter = subject.ChapterExecutions.Select(chapter => new
                        {
                            name = chapter.Chapter.name,
                            sessions = chapter.SessionChapterExecutions.Select(session => new
                            {
                                beginDateTime = session.beginDateTime,
                                endDateTime = session.endDateTime,
                                timeExecution = ((session.endDateTime ?? DateTime.UtcNow.AddHours(5)) - session.beginDateTime).Value,
                                status = session.activeSession ? "Активна" : "Завершена",
                                nameChapter = session.ChapterExecution.Chapter.name,

                                SessionProgresses = session.SessionProgresses.OrderBy(p => p.taskNumber).Select(sessionProgress => new
                                {
                                    taskNumber = sessionProgress.taskNumber,
                                    status = sessionProgress.GetStatus()
                                }),

                            }),
                            FinishResults = chapter.SessionChapterExecutions.OrderByDescending(p => p.idSessionChapterExecution).FirstOrDefault()
                            ?.SessionProgresses.OrderBy(p => p.taskNumber).Select(sessionProgress => new
                            {
                                taskNumber = sessionProgress.taskNumber,
                                status = sessionProgress.GetStatus() == "Ожидает выполнения" ? "Решено неверно" : sessionProgress.GetStatus()
                            })
                        })
                    })
                })
            }));
        }
    }
}
