using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatingController : ControllerBase
    {
        private Models.context context;
        private readonly UserManager<Models.Child> _userManager;
        private readonly SignInManager<Models.Child> _signInManager;
        public CreatingController(Models.context _context, UserManager<Models.Child> userManager, SignInManager<Models.Child> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            context = _context;
        }

        [HttpPost]
        [Route("levelStuding")]
        public async Task<ActionResult> CreateLevelStuding(PostModels.CreatingLevelStudingModel model)
        {
            if (model.name < 1 || model.name > 11)
                return BadRequest("Введите корректное значение.");
            if (context.LevelStudings.Where(p => p.nameLevel == model.name.ToString()).FirstOrDefault() != null)
            {
                return BadRequest("Данный класс уже создан.");
            }
            context.LevelStudings.Add(new Models.LevelStuding { nameLevel = model.name.ToString() });
            context.SaveChanges();
            return Ok("Объект успешно создан.");
        }

        [HttpPost]
        [Route("subject")]
        public async Task<ActionResult> CreateSubject(PostModels.CreatingSubjectModel model)
        {
            try
            {
                if (context.LevelStudings.Where(p => p.nameLevel == model.levelStuding.ToString()).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса не существует.");
                }
                if (context.Subjects.Where(p => p.LevelStuding.nameLevel == model.levelStuding.ToString() && p.nameSubject == model.name).FirstOrDefault() != null)
                {
                    return BadRequest("Данная десциплина уже добавлена.");
                }
                context.Subjects.Add(new Models.Subject { LevelStuding = context.LevelStudings.Where(p => p.nameLevel == model.levelStuding.ToString()).FirstOrDefault(), nameSubject = model.name });
                context.SaveChanges();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangeAccessChapter")]
        public async Task<ActionResult> ChangeAccessChapter(PostModels.ChangeAccessChapterModel model)
        {
            try
            {
                if (context.Chapters.Where(p => p.name == model.nameChapter && p.Subject.LevelStuding.nameLevel == model.levelStuding).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса или раздела не существует.");
                }
                var changeableChapter = context.Chapters.Where(p => p.name == model.nameChapter && p.Subject.LevelStuding.nameLevel == model.levelStuding).FirstOrDefault();
                changeableChapter.access = model.access;
                await context.SaveChangesAsync();
                return Ok("Изменения успешно зафиксированы.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("chapter")]
        public async Task<ActionResult> CreateChapter(PostModels.CreatingChapterModel model)
        {
            try
            {
                if (context.Subjects.Where(p => p.LevelStuding.nameLevel == model.levelStuding && p.nameSubject == model.nameSubject).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса или предмета не существует.");
                }
                var selected = context.Subjects.Where(p => p.LevelStuding.nameLevel == model.levelStuding && p.nameSubject == model.nameSubject).FirstOrDefault();
                if (selected.Chapters.Where(p => p.name == model.name).FirstOrDefault() != null)
                {
                    return BadRequest("Данный раздел уже добавлен.");
                }
                selected.Chapters.Add(new Models.Chapter { Description = model.Description, name = model.name, access = model.access });
                await context.SaveChangesAsync();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChapterTheory")]
        public async Task<ActionResult> AddTheoryForChapter(PostModels.CreatingTheoreticalMaterialModel teoM)
        {
            try
            {
                if (context.Subjects.Where(p => p.LevelStuding.nameLevel == teoM.levelStuding
                && p.nameSubject == teoM.nameSubject).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса, предмета не существует.");
                }
                var selected = context.Subjects.Where(p => p.LevelStuding.nameLevel == teoM.levelStuding && p.nameSubject == teoM.nameSubject).FirstOrDefault();
                if (selected.Chapters.Where(p => p.name == teoM.chapterName).FirstOrDefault() == null)
                {
                    return BadRequest("Данного раздела не существует.");
                }
                var selectedChapter = selected.Chapters.Where(p => p.name == teoM.chapterName).FirstOrDefault();
                if (selectedChapter.TheoreticalMaterials.Where(p => p.header == teoM.header).FirstOrDefault() != null)
                {
                    return BadRequest("Теоретический материал с данным заголовком уже существует.");
                }
                selectedChapter.TheoreticalMaterials.Add(new Models.TheoreticalMaterial { content = teoM.content, header = teoM.header });
                await context.SaveChangesAsync();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("teacherReg")]
        //public async Task<ActionResult> RegistrationTeacher()
        //{

        //}

        [HttpPost]
        [Route("DeleteTheory")]
        public async Task<ActionResult> DeleteTheoryForChapter(PostModels.DeleteTheoreticalMaterialModel teoM)
        {
            try
            {
                if (context.Subjects.Where(p => p.LevelStuding.nameLevel == teoM.levelStuding
                && p.nameSubject == teoM.nameSubject).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса, предмета не существует.");
                }
                var selected = context.Subjects.Where(p => p.LevelStuding.nameLevel == teoM.levelStuding && p.nameSubject == teoM.nameSubject).FirstOrDefault();
                if (selected.Chapters.Where(p => p.name == teoM.chapterName).FirstOrDefault() == null)
                {
                    return BadRequest("Данного раздела не существует.");
                }
                var selectedChapter = selected.Chapters.Where(p => p.name == teoM.chapterName).FirstOrDefault();
                if (selectedChapter.TheoreticalMaterials.Where(p => p.header == teoM.header).FirstOrDefault() != null)
                {
                    return BadRequest("Теоретический материал с данным заголовком не существует.");
                }
                selectedChapter.TheoreticalMaterials.Remove(selectedChapter.TheoreticalMaterials.Where(p => p.header == teoM.header).FirstOrDefault());
                await context.SaveChangesAsync();
                return Ok("Объект успешно удален.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("TestPack")]
        public async Task<ActionResult> CreateTestPack(PostModels.CreatingTestPackModel model)
        {
            try
            {
                if (context.Chapters.Where(p => p.Subject.LevelStuding.nameLevel == model.levelStuding && p.Subject.nameSubject == model.subjectName).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса, предмета или раздела не существует.");
                }
                var selected = context.Chapters.Where(p => p.Subject.LevelStuding.nameLevel == model.levelStuding && p.Subject.nameSubject == model.subjectName && p.name == model.chapterName).FirstOrDefault();
                if (selected.TestPacks.Where(p => p.header == model.header).FirstOrDefault() != null)
                {
                    return BadRequest("Данная тестовая коллекция уже существует.");
                }
                selected.TestPacks.Add(new Models.TestPack { header = model.header, Type = model.Type });
                context.SaveChanges();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("TestTask")]
        public async Task<ActionResult> CreateTestTask(PostModels.CreatingTestTaskModel model)
        {
            try
            {
                if (context.Chapters.Where(p => p.Subject.LevelStuding.nameLevel == model.levelStuding && p.Subject.nameSubject == model.subjectName).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса, предмета или раздела не существует.");
                }
                var selected = context.Chapters.Where(p => p.Subject.LevelStuding.nameLevel == model.levelStuding && p.Subject.nameSubject == model.subjectName).FirstOrDefault();
                var testTask = new Models.TestTask { textQuestion = model.textQuestion };
                foreach (var mod in model.CreatingAnswearOnTaskModels)
                {
                    testTask.AnswearOnTasks.Add(new Models.AnswearOnTask { accuracy = mod.accuracy, textAnswear = mod.textAnswear });
                }
                if (selected.TestPacks.Where(p => p.header == model.testPackHeader).FirstOrDefault() == null)
                {
                    return BadRequest("Данной тестовой коллекции не существует.");
                }
                selected.TestPacks.Where(p => p.header == model.testPackHeader).FirstOrDefault().TestTasks.Add(testTask);
                context.SaveChanges();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("TaskWithOpenAnswear")]
        public async Task<ActionResult> CreateTaskWithOpenAnswear(PostModels.CreatingTaskWithOpenAnswModel model)
        {
            try
            {
                var testPack = context.TestPacks.FirstOrDefault(p => p.idTestPack == model.idTestPack);
                if (testPack == null)
                {
                    return BadRequest("Данного кейса не существует");
                }

                var openTask = new Models.TaskWithOpenAnsw { 
                    textQuestion = model.textQuestion, 
                    theme = model.theme, 
                    isIncreasedComplexity = model.isIncreasedComplexity,
                    fine = model.fine,
                    orderImportant = model.orderImportant,
                    ResultType = model.resultType,
                };

                openTask.htmlModel = (model.resultType) switch //здесь 3 вариант не работает
                {
                    Models.ResultTypes.Label => "<input type=\"text\" class=\"task__input\">",
                    Models.ResultTypes.Squares => "<div class=\"task__multiple-input\"> \r\n<input type=\"text\" class=\"task__input\"> \r\n<input type=\"text\" class=\"task__input\"> \r\n</div>\r\n",
                    Models.ResultTypes.Table => "<div data-columns=\"5\" class=\"task__input-table\">\r\n              <div class=\"task__table-cell\">А</div>\r\n              <div class=\"task__table-cell\">Б</div>\r\n              <div class=\"task__table-cell\">В</div>\r\n              <div class=\"task__table-cell\">Г</div>\r\n              <div class=\"task__table-cell\">Д</div>\r\n              <input type=\"text\" class=\"task__input task__table-cell\">\r\n              <input type=\"text\" class=\"task__input task__table-cell\">\r\n              <input type=\"text\" class=\"task__input task__table-cell\">\r\n              <input type=\"text\" class=\"task__input task__table-cell\">\r\n              <input type=\"text\" class=\"task__input task__table-cell\">\r\n            </div>\r\n"
                };

                openTask.AnswearOnTaskOpens.Add(new Models.AnswearOnTaskOpen
                {
                    mark = model.answear.mark,
                    answear = model.answear.answear
                });

                openTask.Solutions.Add(new Models.Solution { url = model.solution });

                var listTasks = testPack.GetNumbers();

                switch (model.mode)
                {
                    case PostModels.ModeCreating.Start:
                        for (int i = 0; i < listTasks.Count; i++)
                        {
                            listTasks[i].numericInPack = i + 2;
                        }
                        openTask.numericInPack = 1;
                        break;
                    case PostModels.ModeCreating.End:

                        openTask.numericInPack = listTasks.Count + 1;
                        break;
                    case PostModels.ModeCreating.Insert:
                        if (model.numberInList == null)
                            return BadRequest("Не указан номер вставки.");
                        for (int i = 0; i < listTasks.Count; i++)
                        {
                            if (model.numberInList - 1 > i)
                                listTasks[i].numericInPack = i + 1;
                            else
                                listTasks[i].numericInPack = i + 2;
                        }
                        openTask.numericInPack = model.numberInList.Value;
                        break;
                    default:
                        break;
                }

                testPack.TaskWithOpenAnsws.Add(openTask);
                await context.SaveChangesAsync();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("TaskWithClosedAnswear")]
        public async Task<ActionResult> CreateTaskWithClosedAnswear(PostModels.CreatingTaskWithClosedAnswModel model)
        {
            try
            {
                if (context.Chapters.Where(p => p.Subject.LevelStuding.nameLevel == model.levelStuding && p.Subject.nameSubject == model.subjectName && p.name == model.chapterName).FirstOrDefault() == null)
                {
                    return BadRequest("Данного класса, предмета или раздела не существует.");
                }
                var selected = context.Chapters.Where(p => p.Subject.LevelStuding.nameLevel == model.levelStuding && p.Subject.nameSubject == model.subjectName).FirstOrDefault();
                var testTask = new Models.TaskWithClosedAnsw { textQuestion = model.textQuestion, theme = model.theme, isIncreasedComplexity = model.isIncreasedComplexity };
                foreach (var mod in model.CreatingSolutionModels)
                {
                    using (var hhtp = new HttpClient())
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StringContent(mod.base64image), "source");
                        var request = await hhtp.PostAsync("https://freeimage.host/api/1/upload?key=6d207e02198a847aa98d0a2a901485a5&format=json", content);
                        request.EnsureSuccessStatusCode();
                        var res = request.Content.ReadAsAsync<PostModels.Rootobject>().Result;
                        testTask.Solutions.Add(new Models.Solution { url = res.image.url });
                    }
                }
                foreach (var mod in model.CreatingAnswearOnTaskModels)
                {
                    testTask.AnswearOnTask.Add(new Models.AnswearOnTask { accuracy = mod.accuracy, textAnswear = mod.textAnswear });
                }
                if (selected.TestPacks.Where(p => p.header == model.testPackHeader).FirstOrDefault() == null)
                {
                    return BadRequest("Данной тестовой коллекции не существует.");
                }

                var listTasks = selected.TestPacks.Where(p => p.header == model.testPackHeader).FirstOrDefault().GetNumbers();


                switch (model.mode)
                {
                    case PostModels.ModeCreating.Start:
                        for (int i = 0; i < listTasks.Count; i++)
                        {
                            listTasks[i].numericInPack = i + 2;
                        }
                        testTask.numericInPack = 1;
                        break;
                    case PostModels.ModeCreating.End:

                        testTask.numericInPack = listTasks.Count + 1;
                        break;
                    case PostModels.ModeCreating.Insert:
                        if (model.numberInList == null)
                            return BadRequest("Не указан номер вставки.");
                        for (int i = 0; i < listTasks.Count; i++)
                        {
                            if (model.numberInList - 1 > i)
                                listTasks[i].numericInPack = i + 1;
                            else
                                listTasks[i].numericInPack = i + 2;
                        }
                        testTask.numericInPack = model.numberInList.Value;
                        break;
                    default:
                        break;
                }

                selected.TestPacks.Where(p => p.header == model.testPackHeader).FirstOrDefault().TaskWithClosedAnsws.Add(testTask);
                context.SaveChanges();
                return Ok("Объект успешно создан.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
