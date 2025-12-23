using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.BlogModels;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Service.Checkers;
using Blog.Service.Exceptions.AccountManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Tasks.Blog
{
    public class ArticleHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ArticleHandler> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public ArticleHandler(IUnitOfWork unitOfWirk, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ArticleHandler>();
            _loggerFactory = loggerFactory;
            _unitOfWork = unitOfWirk;
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

            if (tag.Name != "default")
            {
                result.AddRange(await articleRepository.GetArticlesByTag(tag));
            }

            _logger.LogInformation($"Результат создания статьи {result}");

            return result;
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
    }
}
