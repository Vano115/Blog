using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.BlogModels;
using Blog.Data.Models.UserModels;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Models.UserModels;
using Blog.Service.Checkers;
using Blog.Service.Exceptions.AccountManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Tasks.Blog
{
    public class BlogHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlogHandler> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserManager<User> _userManager;

        public BlogHandler(IUnitOfWork unitOfWirk, ILoggerFactory loggerFactory, UserManager<User> userManager)
        {
            _logger = loggerFactory.CreateLogger<BlogHandler>();
            _loggerFactory = loggerFactory;
            _unitOfWork = unitOfWirk;
            _userManager = userManager;
        }

        public async Task<bool> CreateArticle (User owner, ArticleViewModel model)
        {
            List<string> crudTags = model.Tags.Split('#').ToList();
            List<Tag> tags = new List<Tag>();

            if (crudTags.Count > 0)
            {
                ArticleChecker checker = new ArticleChecker(_unitOfWork, _loggerFactory);
                tags = await checker.CheckAndCreateTags(crudTags);
            }

            var article = new Article() 
            { 
                Owner = owner,
                Title = model.Title,
                Text = model.Text,
                Tags = tags
            };

            // На этом этапе создания статьи так же можно добавить какие-либо микросервисы:
            // проверка текста статьи или ещё что-то подобное


            // С помощью паттерна UnitOfWork получаем репозиторий для работы со статьёй в БД
            var articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий статей");

            var result = await articleRepository.CreateAsync(article);

            _logger.LogInformation($"Результат создания статьи {result}");

            return true;
        }

        public async Task<List<Article>> GetArticle(string input)
        {
            List<Article> result = new List<Article>();

            // С помощью паттерна UnitOfWork получаем репозиторий для работы со статьёй в БД
            var articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий статей");

            var tagRepository = _unitOfWork.GetRepository<Tag>() as TagRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий статей");

            var tag = await tagRepository.FindByNameAsync(input);

            result = await articleRepository.GetArticlesByTitle(input);

            if (tag != null)
            {
                var articlesByTag = await articleRepository.GetArticlesByTag(tag);
                result = result.Union(articlesByTag).ToList();
            }

            _logger.LogInformation($"Результат поиска статьи {result}");

            return result;
        }

        public async Task<List<ViewUserModel>> GetUsers(string input)
        {

            List<ViewUserModel> users = new List<ViewUserModel>();

            var find = _userManager.Users.AsEnumerable().Where(x => x.UserName.ToLower().Contains(input.ToLower())).ToList();

            if (find.Count > 0)
            {

                foreach (var user in find)
                {
                    ViewUserModel model = new ViewUserModel();
                    users.Add(UserConverter.ConvertToModel(model, user));
                }
            }
            return users;
        }

        public async Task<Article> GetArticleById(int input)
        {
            // С помощью паттерна UnitOfWork получаем репозиторий для работы со статьёй в БД
            var articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий статей");

            Article result = await articleRepository.GetAsync(input) ??
                throw new ArticleNotFoundException("Программа не нашла статью по id");

            return result;
        }

        /// <summary>
        /// Метод создания комментария
        /// </summary>
        /// <param name="articleId">Id комментируемой статьи</param>
        /// <param name="text">Текст комментария</param>
        /// <param name="owner">Пользователь написавший комментарий</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArticleNotFoundException"></exception>
        public async Task<bool> CreateComment(int articleId,  string text, User owner)
        {
            var articleRepository = _unitOfWork.GetRepository<Article>() as ArticleRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий статей");

            var article = await articleRepository.GetAsync(articleId) ??
                throw new ArticleNotFoundException("Программа не нашла статью по id");

            Comment comment = new Comment()
            {
                Article = article,
                Text = text,
                User = owner,
                PublicDate = DateTime.UtcNow,
                Likes = 0
            };

            var commentRepository = _unitOfWork.GetRepository<Comment>() as CommentRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий комментариев");

            var result = await commentRepository.CreateAsync(comment);

            if (result != 0)
            {
                return true;
            }

            return false;
        }
    }
}
