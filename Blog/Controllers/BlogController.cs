using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Blog.Data.Models.BlogModels;
using Blog.Data.UoW;
using Blog.Service.Tasks.Blog;
using Blog.Service.Exceptions.AccountManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("[controller]")]
    //[Authorize(Roles = "Guest, User")] !!!
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(ILoggerFactory loggerFactory, UserManager<User> userManager,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<BlogController>();
            _loggerFactory = loggerFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;

        }

        [Route("WriteArticleView")]
        [HttpGet]
        public async Task<IActionResult> WriteArticleView()
        {
            return View("WriteArticleView");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArticle(ArticleViewModel model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    User owner = await _userManager.GetUserAsync(User)
                        ?? throw new UserNotFoundException("Внутренняя ошибка, не определён владелец статьи при создании");

                    var handler = new ArticleHandler(_unitOfWork, _loggerFactory) { };

                    await handler.CreateArticle(owner, model);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

                return RedirectToAction("MyProfile", "User");
            }

            return View("WriteArticleView", model);
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<List<Article>> FindArticle(string input)
        {
            List<Article> result = new List<Article>();
            try
            {
                var handler = new ArticleHandler(_unitOfWork, _loggerFactory) { };

                result = await handler.GetArticle(input);
            }
            catch
            {

            }

            return result;
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadArticle(int id)
        {
            try
            {
                var handler = new ArticleHandler(_unitOfWork, _loggerFactory) { };

                var article = await handler.GetArticleById(id);

                ReadArticleViewModel result = new ReadArticleViewModel()
                {
                    Title = article.Title,
                    Text = article.Text,
                    Tags = article.Tags,
                    Comments = article.Comments,
                    Owner = article.Owner,
                };

                return View(result);
            }
            catch
            {

            }

            return View();
        }
    }
}
