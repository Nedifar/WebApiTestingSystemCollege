using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webApiipAweb.Models;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<Child> _userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<Child> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return Ok(name);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return Ok(name);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> AddUserToRole(AddUserToRoleModelView model)
        {
            var user = await _userManager.FindByIdAsync(model.idChild);
            var result = await _userManager.AddToRoleAsync(user, model.role);
            if (result.Succeeded)
            {
                return Ok(_userManager.GetRolesAsync(user));
            }
            return BadRequest();
        }
    }

    public class AddUserToRoleModelView
    {
        public string role { get; set; }
        public string idChild { get; set; }
    }
}
