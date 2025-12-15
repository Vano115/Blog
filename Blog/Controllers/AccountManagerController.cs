using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.UserModels;
using Blog.Data.UoW;
using Blog.Service.Exceptions.AccountManager;
using Blog.Service.Tasks.AccountManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Blog.Controllers
{
    [Route("[controller]")]
    public class AccountManagerController : Controller
    {
        private readonly ILogger<AccountManagerController> _logger;
        private readonly ILogger<Register> _registerLogger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;

        public AccountManagerController(ILogger<AccountManagerController> logger, UserManager<User> userManager,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork, IDbContextFactory<ApplicationContext> context,
            ILogger<Register> registerLogger)
        {
            _logger = logger;
            _registerLogger = registerLogger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _dbContextFactory = context;
        }

        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    var registrator = new Register(_userManager, _dbContextFactory, _registerLogger);

                    var result = await registrator.RegisterTask(model);

                    if (result != null)
                    {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(result);

                        var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "AccountManager",
                            new { userId = result.Id, code = code },
                            protocol: HttpContext.Request.Scheme);

                        EmailService emailService = new EmailService();

                        await emailService.SendEmailAsync(model.Email, "Confirm your account",
                            $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                        return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

                    }
                }
                catch (WrongValueException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    _logger.LogInformation($"Пользователь {model.UserName} незарегистрирован: " + ex.Message);
                    return View(model);
                }
                catch(UserNotCreatedException ex)
                {
                    _logger.LogError(ex.Errors.First().Description);
                    ModelState.AddModelError("401", ex.Errors.First().Description);
                    return View(model);
                }

                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [Route("Autorize")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Autorize(AutorizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registrator = new Register(_userManager, _dbContextFactory, _registerLogger);

                    var result = await registrator.RegisterTask(model);

                    if (result != null)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(result);

                        var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "AccountManager",
                            new { userId = result.Id, code = code },
                            protocol: HttpContext.Request.Scheme);

                        EmailService emailService = new EmailService();

                        await emailService.SendEmailAsync(model.Email, "Confirm your account",
                            $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                        return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

                    }
                }
                catch (WrongValueException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    _logger.LogInformation($"Пользователь {model.UserName} незарегистрирован: " + ex.Message);
                    return View(model);
                }
                catch (UserNotCreatedException ex)
                {
                    _logger.LogError(ex.Errors.First().Description);
                    ModelState.AddModelError("401", ex.Errors.First().Description);
                    return View(model);
                }

                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            var resultConfirm = await _userManager.ConfirmEmailAsync(user, code);
            var resultRole = await _userManager.AddToRoleAsync(user, "User");

            if (resultConfirm.Succeeded && resultRole.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }
    }
}
