using Blog.Data.Entityes;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Service.Tasks.Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Tasks.UserTask
{
    public class UserHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserHandler> _logger;
        private readonly UserManager<User> _userManager;

        public UserHandler(IUnitOfWork unitOfWirk, ILoggerFactory loggerFactory, UserManager<User> userManager)
        {
            _logger = loggerFactory.CreateLogger<UserHandler>();
            _unitOfWork = unitOfWirk;
            _userManager = userManager;
        }

        public async Task<List<Article>> GetUserArticles(User user)
        {
            // С помощью паттерна UnitOfWork получаем репозиторий для работы со статьёй в БД
            var articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий статей");

            List<Article> result = await articleRepository.GetUserArticles(user);

            return result;
        }

        public async Task<bool> DeleteUser(User user)
        {
            // С помощью паттерна UnitOfWork получаем репозитории
            // для статей
            var articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository ??
                throw new InvalidOperationException("Программа не получила репозиторий статей");

            await articleRepository.DeleteUserArticles(user);

            await _userManager.DeleteAsync(user);

            return true;
        }
    }
}
