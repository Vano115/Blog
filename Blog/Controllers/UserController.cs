using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.UserModels;
using Blog.Data.UoW;
using Blog.Models.UserModels;
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

            var result = await _userManager.GetUserAsync(User) ??
                throw new InvalidOperationException("Пользователь не найден или не авторизован");

            var model = new UserProfileViewModel()
            {
                UserName = result.UserName,
                ProfileImage = result.ProfileImage,
            };

            // Передача в модель списка статей
            model.Articles = await handler.GetUserArticles(result);
            _logger.LogInformation($"Вызов страницы пользователя {result.UserName}");

            // Заполненая страница пользователя
            return View("MyProfile", model);
        }


        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersBlog(string userNmae)
        {
            var user = await _userManager.FindByNameAsync(userNmae)
                    ?? throw new UserNotFoundException("Не найден пользователь");

            ViewUserModel model = new ViewUserModel();

            model = UserConverter.ConvertToModel(model, user);

            return View("UserProfile", model);
        }

        /// <summary>
        /// Редактирование профиля пользователя
        /// </summary>
        /// <param name="model">Модель заполняемая пользователем, 
        /// по умолчанию пользователю будут отображаться данные с его страницы</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [Route("EditPage")]
        [Authorize]
        public async Task<IActionResult> EditPage(UserEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User) ??
                throw new InvalidOperationException("Пользователь не найден или не авторизован");

            model = UserConverter.ConvertToModel(model, currentUser);

            return View("UserEditForm", model);
        }

        /// <summary>
        /// Редактирование данных пользователя
        /// </summary>
        /// <param name="model">Заполненая модель с новыми данными пользователя</param>
        /// <returns></returns>
        [Authorize]
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            //!!! Доработать try-catch

            // Проверка корректности заполнения свойств модели
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User) ??
                throw new InvalidOperationException("Пользователь не найден или не авторизован");

                // Проверка найден ли пользователь
                if (user != null)
                {
                    // Вносим пользователю отредактирование данные
                    user.UpdateFromModel(model);
                }
                else
                {
                    return RedirectToAction("Edit", "User", model);
                }

                // Обновляем информацию в БД
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // При успешном обновлении данных переходим в профиль пользователя
                    return RedirectToAction("MyProfile", "User");
                }
                else
                {
                    // При неудачном обновлении данных в БД возвращаемся обратно на страницу редактирования
                    return RedirectToAction("Edit", "User");
                }
            }

            // Если данные модели некорректны возвращаемся обратно на страницу редактирования
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("User/UserEditForm", model);
            }
        }
    }   
}
