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

        public async Task<User> RegisterTask(RegisterViewModel model)
        {
            AccountManagerChecker checker = new AccountManagerChecker(_userManager);

            await checker.CheckModel(model);

            User user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return await _userManager.FindByIdAsync(user.Id)
                    ?? throw new UserNotFoundException($"Ошибка поиска только что созданного пользователя {user.UserName}");

            }

            throw new UserNotCreatedException(result.Errors);
        }
    }
}
