using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var areas = await context.Areas.Where(p=>p.Region.regionName == region).OrderBy(p=>p.areaName).ToListAsync();
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
            var schools = await context.Schools.Where(p => p.Municipality.name== municipality).OrderBy(p => p.nameSchool).ToListAsync();
            return Ok(schools.Select(p => new
            {
                name = p.nameSchool
            }));
        }
    }
}
