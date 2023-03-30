using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using webApiipAweb.Models;
namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListDataRegController : ControllerBase
    {
        private readonly context context;

        public ListDataRegController(context _context)
        {
            this.context = _context;
        }

        [HttpGet]
        [Route("getRegions")]
        public async Task<ActionResult> GetRegions()
        {
            var regions = await context.Regions.OrderBy(p => p.regionName).ToListAsync();
            return Ok(regions.Select(p => new
            {
                Name = p.regionName,
            }));
        }

        [HttpGet]
        [Route("getAreas")]
        public async Task<ActionResult> GetRegions(string region)
        {
            var areas = await context.Areas.Where(p => p.Region.regionName == region).OrderBy(p => p.areaName).ToListAsync();
            return Ok(areas.Select(p => new
            {
                Name = p.areaName
            }));
        }

        [HttpGet]
        [Route("getMunicipalities")]
        public async Task<ActionResult> GetMunicipalities(string area)
        {
            var municipalities = await context.Municipalities.Where(p => p.Area.areaName == area).OrderBy(p => p.name).ToListAsync();
            return Ok(municipalities.Select(p => new
            {
                name = p.name
            }));
        }

        [HttpGet]
        [Route("getSchools")]
        public async Task<ActionResult> GetSchool(string municipality)
        {
            var schools = await context.Schools.Where(p => p.Municipality.name == municipality).OrderBy(p => p.nameSchool).ToListAsync();
            return Ok(schools.Select(p => new
            {
                idSchool = p.idSchool,
                name = p.nameSchool
            }));

        }

        [HttpGet]
        [Route("getLevelsStuding")]
        public async Task<ActionResult> GetLevelsStuding()
        {
            var levels = await context.LevelStudings.OrderBy(p => Convert.ToInt32(p.nameLevel)).ToListAsync();
            return Ok(levels.Select(p => new
            {
                level = p.nameLevel,
                subjs = p.Subjects.Select(p => new
                {
                    idSubj = p.idSubject,
                    nameSubject = p.nameSubject,
                    chapters = p.Chapters.Select(p => new
                    {
                        idChapter = p.idChapter,
                        chapterName = p.name,
                        testPacks = p.TestPacks.Select(p => new
                        {
                            idTestPack = p.idTestPack,
                            name = p.header
                        })
                    })
                })
            }));
        }

        [HttpGet]
        [Route("getSubjectsFromLevel")]
        public async Task<ActionResult> GetSubjectsFromLevel(string level)
        {
            var currentLevel = await context.LevelStudings.FirstOrDefaultAsync(p => p.nameLevel == level);

            return Ok(currentLevel?.Subjects.Select(p => new
            {
                Name = p.nameSubject
            }));
        }

        [HttpGet]
        [Route("getChaptersFromSubject")]
        public async Task<ActionResult> GetChaptersFromSubject(string level, string subject)
        {
            var currentSubject = (await context.LevelStudings.FirstOrDefaultAsync(p => p.nameLevel == level))
                ?.Subjects.FirstOrDefault(p => p.nameSubject == subject);

            return Ok(currentSubject?.Chapters.Select(p => new
            {
                Name = p.name
            }));
        }

        [HttpGet]
        [Route("getTasksFromChapters")]
        public async Task<ActionResult> GetTasksFromChapters(string level, string subject, string chapter)
        {
            var currentChapter = (await context.LevelStudings.FirstOrDefaultAsync(p => p.nameLevel == level))
                ?.Subjects.FirstOrDefault(p => p.nameSubject == subject)
                ?.Chapters.FirstOrDefault(p => p.name == chapter);

            return Ok(currentChapter.TestPacks.Select(p => new
            {
                TestPackName = p.header,
                Tasks = p.GetNumbers().Select(p => new
                {
                    IdTask = p.idTask,
                    Question = p.textQuestion
                })
            }));
        }
    }
}
