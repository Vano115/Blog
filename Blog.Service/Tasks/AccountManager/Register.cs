using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.UserModels;
using Blog.Models.UserModels;
using Blog.Service.Checkers;
using Blog.Service.Exceptions.AccountManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Blog.Service.Tasks.AccountManager
{
    public class Register
    {
        private readonly UserManager<User> _userManager;
        private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;
        private readonly ILogger<Register> _logger;

        public Register(UserManager<User> userManager, 
            IDbContextFactory<ApplicationContext> context,
            ILogger<Register> logger) 
        {
            _userManager = userManager;
            _dbContextFactory = context;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model"> Модель заполненная пользователем</param>
        /// <returns> Зарегистрированый пользователь</returns>
        /// <exception cref="UserNotFoundException"> Ошибка проверки записи пользователя в БД</exception>
        /// <exception cref="UserNotCreatedException"> Ошибка записи пользователя в БД</exception>
        public async Task<User> RegisterTask(RegisterViewModel model)
        {
            AccountManagerChecker checker = new AccountManagerChecker(_userManager);

            // Валидация данных
            await checker.CheckModel(model);

            User user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            // Запись пользователя в БД
            var result = await _userManager.CreateAsync(user, model.Password);

            // Проверка
            if (result.Succeeded)
            {
                return await _userManager.FindByIdAsync(user.Id)
                    ?? throw new UserNotFoundException($"Ошибка поиска только что созданного пользователя {user.UserName}");

            }

            throw new UserNotCreatedException(result.Errors);
        }

        /// <summary>
        /// Задача для отправки письма для подтверждения эл.почты
        /// </summary>
        /// <param name="Email">Почта пользователя</param>
        /// <param name="callBackUrl">Адресная страница для подтверждения</param>
        /// <returns></returns>
        public async Task<bool> ConfirmEmailTask(string Email, string callBackUrl)
        {
            EmailService emailService = new EmailService();

            await emailService.SendConfirmEmailAsync(Email, "Confirm your account",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callBackUrl}'>link</a>");

            // Заглушка! Доработать!
            return true;
        }
    }
}
