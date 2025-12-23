using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.UserModels;
using Blog.Data.UoW;
using Blog.Service.Exceptions.AccountManager;
using Blog.Service.Tasks.AccountManager;
using Blog.Service.Tasks.UserTask;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Blog.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ILoggerFactory loggerFactory, UserManager<User> userManager,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<UserController>();
            _loggerFactory = loggerFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;

        }

        /// <summary>
        /// Страница профиля пользователя
        /// </summary>
        /// <returns> Страница пользователя с фото и заполнеными данными из UserViewModel</returns>

        [HttpGet]
        [Route("MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            var handler = new UserHandler(_unitOfWork, _loggerFactory) { };

            // Свойство ControllerBase.User - выдаёт пользователя из текущей сессии
            var user = User;

            var result = await _userManager.GetUserAsync(user) ??
                throw new InvalidOperationException("Пользователь не найден или не авторизован");

            var model = new UserProfileViewModel()
            {
                UserName = result.UserName,
                ProfileImage = result.ProfileImage,
            };

            // Передача в модель списка пользователей
            model.Articles = await handler.GetUserArticles(result);
            _logger.LogInformation($"Вызов страницы пользователя {result.UserName}");

            // Заполненая страница пользователя
            return View("MyProfile", model);
        }
    }   
}
