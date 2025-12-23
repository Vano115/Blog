using Blog.Data.Entityes;
using Blog.Data.Repository;
using Blog.Data.UoW;
using Blog.Service.Tasks.Blog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Service.Checkers
{
    public class ArticleChecker
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ArticleChecker> _logger;

        public ArticleChecker (IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ArticleChecker>();
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Tag>> CheckAndCreateTags(List<string> tags)
        {
            List<Tag> result = new List<Tag>();

            var tagRepository = _unitOfWork.GetRepository<Tag>() as TagRepository ??
                throw new InvalidOperationException("Программа не получила нужный репозиторий тэгов");

            foreach (var tag in tags)
            {
                bool check = await tagRepository.CheckByNameAsync(tag);

                _logger.LogInformation(check + "NameTag");

                if (check)
                {
                    result.Add(await tagRepository.FindByNameAsync(tag));
                }
                else
                {
                    var newTag = new Tag() { Name = tag };
                    await tagRepository.CreateAsync(newTag);
                    result.Add(await tagRepository.FindByNameAsync(newTag.Name));
                }
            }

            return result;
        }
    }
}
