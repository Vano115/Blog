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
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountManagerController(ILoggerFactory loggerFactory, UserManager<User> userManager,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<AccountManagerController>();
            _loggerFactory = loggerFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;

        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model"> Модель заполненная пользователем на сайте</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var registrator = new Register(_userManager, _loggerFactory);

                    var result = await registrator.RegisterTask(model);

                    if (result != null)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(result);

                        var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "AccountManager",
                            new { userId = result.Id, code = code },
                            protocol: HttpContext.Request.Scheme) ??
                            throw new Exception("Ошибка генерации URL для подтверждения почты");

                        var confirmEmailSendResult = await registrator.ConfirmEmailTask(model.Email, callbackUrl);

                        if (confirmEmailSendResult)
                        {
                            return Content("Для завершения регистрации проверьте электронную почту и " +
                                "перейдите по ссылке, указанной в письме");
                        }
                    }
                }
                catch (WrongValueException ex)
                {
                    // Доработать! Уточнить код ошибки
                    ModelState.AddModelError("401", ex.Message);
                    _logger.LogInformation($"Пользователь {model.UserName} незарегистрирован: " + ex.Message);
                    return View(model);
                }
                catch (UserNotCreatedException ex)
                {
                    _logger.LogError(ex.Errors.First().Description);
                    foreach (var error in ex.Errors)
                    {
                        // Доработать! Уточнить код ошибки
                        ModelState.AddModelError("401", error.Description);
                    }
                    return View(model);
                }

                return RedirectToAction("Index","Home");
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

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AutorizeViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync
                    (model.UserName,
                    model.Password,
                    model.RememberMe,
                    false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns> Возвращает пользователя на главную страницу</returns>
        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Выход пользователя");
            return RedirectToAction("Index", "Home");
        }

    }
}
