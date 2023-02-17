using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using webApiipAweb.Auth;
using webApiipAweb.Email;
using webApiipAweb.Models;

namespace webApiipAweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private Models.context context;
        private readonly UserManager<Models.Child> _userManager;
        private readonly SignInManager<Models.Child> _signInManager;
        public UserController(Models.context _context, UserManager<Models.Child> userManager, SignInManager<Models.Child> signInManager, IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            context = _context;
        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult> GetToken(SignPost signPost)
        {
            try
            {
                var identity = await GetIdentity(signPost.email, signPost.pas);
                var child = await _userManager.FindByEmailAsync(signPost.email);
                if (identity == null)
                    return BadRequest(new { errorText = "Invalid username or password." });
                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    id = identity.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value,
                };
                return new JsonResult(response);
            }
            catch
            {
                return BadRequest(@"-\_/-");
            }
        }

        [HttpPost]
        [Route("sign")]
        public async Task<ActionResult<Child>> Sign(SignPost signPost)
        {
            try
            {
                var child = await _userManager.FindByEmailAsync(signPost.email);
                var res = await _signInManager.CheckPasswordSignInAsync(child, signPost.pas, false);
                var roles = await _userManager.GetRolesAsync(child);
                if (res.Succeeded)
                {
                    if (await _userManager.IsEmailConfirmedAsync(child))
                    {
                        //var userId = User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                        //var child = context.Children.FirstOrDefault(p => p.Id == userId);
                        string imageUrl = String.Empty;
                        using (var http = new HttpClient())
                        {
                            var request = await http.GetAsync($"http://192.168.147.72:83/api/userprofileimage?name={child.imagePath}");
                            if (request.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                imageUrl = "http://192.168.147.72:83/" + $"img{child.Id}.jpeg";
                            }
                        }
                        return Ok(new
                        {
                            ChildId = child.Id,
                            firstName = child.firstName,
                            lastName = child.lastName,
                            email = child.Email,
                            levelStuding = child.levelStuding,
                            point = child.point,
                            spendPoint = child.spendPoint,

                            Appeals = child.Appeals.Select(s => new
                            {
                                date = s.dateAppeal,
                                idAppeal = s.idAppeal,
                                inArchive = s.inArchive,
                                status = s.status.ToString(),
                                textAppeal = s.textAppeal,
                                type = s.TypeAppeal.typeName
                            }),
                            image = imageUrl,
                            roles = child.UserRoles.Select(p => new
                            {
                                p.Role.Name
                            })
                        });
                    }
                }
                return BadRequest("Неправильный логин email или пароль");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("reg")]
        public async Task<ActionResult> Registration(RegPost model)
        {
            var ch = new Models.Child();
            try
            {
                if (context.Users.Where(p => p.Email == model.email).Count() != 0)
                {
                    return BadRequest("Данный email уже используется.");
                }
                ch = new Models.Child
                {
                    Email = model.email,
                    levelStuding = model.levelStuding,
                    lastName = model.lastName,
                    firstName = model.firstName,
                };
                ch.UserName = ch.Id;
                foreach (Models.ThingPack item in context.ThingPacks.ToList())
                {
                    var thingExe = new Models.ThingPackExecution { ThingPack = item };
                    foreach (var iteem in item.Things)
                        thingExe.ThingExecutions.Add(new Models.ThingExecution { Thing = iteem });
                    ch.ThingPackExecutions.Add(thingExe);
                }

                ch.LevelStudingExecutions.Add(new Models.LevelStudingExecution { LevelStuding = context.LevelStudings.Where(p => p.nameLevel == ch.levelStuding.ToString()).FirstOrDefault() });
                //context.Children.Add(child);
                var result = await _userManager.CreateAsync(ch, model.pas);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(ch, new string[] { "child" });
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(ch);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "User",
                        new { userId = ch.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    callbackUrl = callbackUrl.Replace("http://localhost:5000", "https://gamification.oksei.ru/gameserv");
                    callbackUrl = callbackUrl.Replace("192.168.147.72:81", "gamification.oksei.ru");
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(ch.Email, "Confirm your account",
                        $"Пожалуйста, подтвердите почту по ссылке: <a href='{callbackUrl}'>link</a>");

                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
                else
                {
                    string str = String.Empty;
                    foreach (var ss in result.Errors)
                        str += ss.Description + "\n";
                    return BadRequest(str);
                }
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(ch);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("editProfile")]
        public async Task<ActionResult> editProfile(EditProfileModel profileModel)
        {
            try
            {
                var result = await _userManager.FindByIdAsync(profileModel.idUser);
                if (result == null)
                    return BadRequest("Неверный Id");
                using (var hhtp = new HttpClient())
                {
                    if (profileModel.base64image != null || !String.IsNullOrWhiteSpace(profileModel.base64image))
                    {
                        try
                        {
                            var modelImageSet = new { name = profileModel.idUser, base64 = profileModel.base64image };
                            using (var http = new HttpClient())
                            {
                                var request = await http.PostAsync("http://192.168.147.72:83/api/userprofileimage", modelImageSet, new JsonMediaTypeFormatter());
                                result.imagePath = $"images/img{profileModel.idUser}.jpeg";
                            }
                        }
                        catch { return BadRequest("Проблемы с добавлением изображения."); }
                    }
                    result.firstName = profileModel.firstName;
                    result.lastName = profileModel.lastName;
                    result.levelStuding = profileModel.levelStuding;
                    if (result.LevelStudingExecutions.Where(p => p.LevelStuding.nameLevel == result.levelStuding.ToString()).FirstOrDefault() == null)
                    {
                        result.LevelStudingExecutions.Add(new Models.LevelStudingExecution { LevelStuding = context.LevelStudings.Where(p => p.nameLevel == result.levelStuding.ToString()).FirstOrDefault() });
                    }
                    context.SaveChanges();
                    return Ok("Успешно");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PasswordChange(ChangePasswordModel passwordModel)
        {
            try
            {
                var result = await _userManager.FindByNameAsync(passwordModel.email);
                if (result != null)
                {
                    var cc = await _userManager.GeneratePasswordResetTokenAsync(result);
                    result.passRecoveryCode = new Random().Next(111111, 999999).ToString();
                    context.SaveChanges();
                    //var callbackUrl = Url.Action(
                    //    "ConfirmEmail",
                    //    "User",
                    //    new { userId = result.Id, code = cc },
                    //    protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(result.Email, "Password recovery",
                        $"Код подтверждения: <a> {result.passRecoveryCode}</a>");

                    return Content("Для завершения сброса пароля проверьте электронную почту и введите код, указанный в письме");
                }
                else
                {
                    return BadRequest("Данный email не найден");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Ok("Успешно подтверждено.");
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("passworConfirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmCodeReset(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest();
            }
            if (user.passRecoveryCode == code)
                return Ok("Успешно подтверждено");
            else
                return BadRequest("Неверный код");
        }

        [HttpPost]
        [Route("pasChang")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordFinishModel model)
        {
            if (model.userName == null || model.newPassword == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByNameAsync(model.userName);
            if (user == null)
            {
                return BadRequest();
            }
            var _passwordValidator =
                HttpContext.RequestServices.GetService(typeof(IPasswordValidator<Models.Child>)) as IPasswordValidator<Models.Child>;
            var _passwordHasher =
                HttpContext.RequestServices.GetService(typeof(IPasswordHasher<Models.Child>)) as IPasswordHasher<Models.Child>;

            IdentityResult result =
                await _passwordValidator.ValidateAsync(_userManager, user, model.newPassword);
            if (result.Succeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, model.newPassword);
                await _userManager.UpdateAsync(user);
                return Ok("Пароль успешно сменен.");
            }
            else
            {
                string errors = String.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                return BadRequest(errors);
            }
        }
        [HttpPost]
        [Route("buyThing")]
        public async Task<ActionResult> BuyThingOfChild(BuyThingModel buyThing)
        {
            var result = await _userManager.FindByIdAsync(buyThing.idChild);
            if (result == null)
                return BadRequest("Неверный Id");
            var res = context.Things.Where(p => p.idThing == buyThing.idThing).FirstOrDefault();
            if (res == null)
                return BadRequest("Такого предмета не существует");
            var rr = result.ThingPackExecutions.Where(p => p.ThingPack.idThingPack == res.ThingPack.idThingPack).FirstOrDefault().ThingExecutions.Where(p => p.Thing.idThing == buyThing.idThing).FirstOrDefault();
            if (rr.isFinished)
                return BadRequest("Данная позиция уже куплена");
            if (rr.Thing.price <= result.point)
            {
                result.point -= rr.Thing.price;
                rr.isFinished = true;
                if (result.ThingPackExecutions.Where(p => p.ThingPack.idThingPack == res.ThingPack.idThingPack).FirstOrDefault().ThingExecutions.Where(p => p.isFinished == false).Count() == 0)
                {
                    result.ThingPackExecutions.Where(p => p.ThingPack.idThingPack == res.ThingPack.idThingPack).FirstOrDefault().isCompleted = true;
                }
                context.SaveChanges();
                return Ok("Покупка завершена");
            }
            else
            {
                return BadRequest("Недостаточно поинтов для покупки данного предмета.");
            }

        }

        [HttpPost]
        [Route("sendAppeal")]
        public async Task<ActionResult> sendAppeal(PostModels.SendAppealModel model)
        {
            var child = context.Children.Where(p => p.Id == model.ChildId).FirstOrDefault();
            if (child == null)
            {
                return BadRequest("Данного пользователя не существует");
            }
            var type = context.TypeAppeals.Where(p => p.typeName == model.type).FirstOrDefault();
            if (type == null)
            {
                return BadRequest("Данного типа обращения не существует");
            }
            child.Appeals.Add(new Models.Appeal { dateAppeal = DateTime.Now, status = Models.Status.InProcessing, TypeAppeal = type, textAppeal = model.textAppeal });
            context.SaveChanges();
            return Ok("Успешно");
        }
        [HttpPost]
        [Route("deleteAppeal")]
        public async Task<ActionResult> deleteAppeal(List<PostModels.DeleteAppealModel> model)
        {
            var child = context.Children.Where(p => p.Id == model.FirstOrDefault().ChildId).FirstOrDefault();
            if (child == null)
            {
                return BadRequest("Данного пользователя не существует");
            }
            foreach (var mod in model)
            {
                try
                {
                    child.Appeals.Remove(context.Appeals.Where(p => p.idAppeal == mod.idAppeal).FirstOrDefault());
                }
                catch
                {
                    return BadRequest($"Не существует обращения с таким Id: {mod.idAppeal}");
                }
            }
            context.SaveChanges();
            return Ok("Успешно");
        }
        [HttpPost]
        [Route("archiveAppeal")]
        public async Task<ActionResult> archiveAppeal(List<PostModels.DeleteAppealModel> model)
        {
            var child = context.Children.Where(p => p.Id == model.FirstOrDefault().ChildId).FirstOrDefault();
            if (child == null)
            {
                return BadRequest("Данного пользователя не существует");
            }
            foreach (var mod in model)
            {
                try
                {
                    child.Appeals.Where(p => p.idAppeal == mod.idAppeal).FirstOrDefault().inArchive = true;
                }
                catch
                {
                    return BadRequest($"Не существует обращения с таким Id: {mod.idAppeal}");
                }
            }
            context.SaveChanges();
            return Ok("Успешно");
        }

        [HttpPost]
        [Route("GetAppeals")]
        public async Task<ActionResult> GetAppeals(GetAppealModel model)
        {
            var appealsSelectedChild = context.Appeals.Where(p => p.ChildId == model.idChild).Select(p => new
            {
                idAppeal = p.idAppeal,
                textAppeal = p.textAppeal,
                dateAppeal = p.dateAppeal.ToString("HH.mm dd.MM.yyyy"),
                type = p.TypeAppeal.typeName,
                status = p.GetStatus(),
                inArchive = p.inArchive
            }).ToList();
            return Ok(appealsSelectedChild == null ? "У вас нет обращений." : appealsSelectedChild);
        }

        [HttpPost]
        [Route("GetChapterResult")]
        public async Task<ActionResult> GetChapterResult(PostResultChapetModel model)
        {
            var chapter = context.ChapterExecutions.Where(p => p.idChapterExecution == model.idChapterExecution).FirstOrDefault();
            if (chapter == null)
            {
                return BadRequest("Такого выполнения раздела не существует.");
            }
            else
            {
                return Ok(chapter.TestPackExecutions.Select(p => new
                {
                    Header = p.TestPack.header,
                    Tasks = p.GetTasksExecution().Select(l => new
                    {
                        status = l.GetStatus(),
                        serialNumber = p.GetTasksExecution().IndexOf(l) + 1
                    }),
                    Test = p.TryingTestTasks.OrderByDescending(p => p.result).FirstOrDefault()?.TestTaskExecutions.Select(s => new
                    {
                        number = p.TryingTestTasks.OrderByDescending(p => p.result).FirstOrDefault().TestTaskExecutions.IndexOf(s) + 1,
                        status = s.GetStatus()
                    })
                }));
            }
        }

        private async Task<List<Claim>> GetIdentity(string email, string password)
        {
            var child = context.Children.FirstOrDefault(c => c.Email == email);
            var res = await _signInManager.CheckPasswordSignInAsync(child, password, false);
            if (res.Succeeded)
            {
                if (await _userManager.IsEmailConfirmedAsync(child))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, child.Id),

                };
                    foreach (var role in await _userManager.GetRolesAsync(child))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    return claims;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }


    public class SignPost
    {
        public string email { get; set; }
        public string pas { get; set; }
    }

    public class RegPost
    {
        public string email { get; set; }
        public string pas { get; set; }
        public int levelStuding { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
    }
    public class ChangePasswordModel
    {
        public string email { get; set; }
    }
    public class ChangePasswordFinishModel
    {
        public string userName { get; set; }
        public string newPassword { get; set; }
    }
    public class BuyThingModel
    {
        public string idChild { get; set; }
        public int idThing { get; set; }
    }

    public class EditProfileModel
    {
        public string idUser { get; set; }
        public int levelStuding { get; set; }
        public string base64image { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
    }

    public class GetAppealModel
    {
        public string idChild { get; set; }
    }

    public class PostResultChapetModel
    {
        public string idChapterExecution { get; set; }
    }
}
