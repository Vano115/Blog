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
using Blog.Data.Models.UserModels;
using Blog.Models.UserModels;

namespace Blog.Controllers
{
    [Route("Blog")]
    //[Authorize(Roles = "Guest, User")] !!!
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(ILoggerFactory loggerFactory, UserManager<User> userManager,
            SignInManager<User> signInManager, IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<BlogController>();
            _loggerFactory = loggerFactory;
            _userManager = userManager;
            _unitOfWork = unitOfWork;

        }

        [Route("WriteArticleView")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> WriteArticleView()
        {
            return View("WriteArticleView");
        }

        [Route("CreateArticle")]
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

                    var handler = new BlogHandler(_unitOfWork, _loggerFactory, _userManager) { };

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


        [Route("Finder")]
        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken] с Get запросом просто так не работает, надо настраивать
        public async Task<IActionResult> Finder(string searchString)
        {
            FindViewModel result = new FindViewModel();
            var handler = new BlogHandler(_unitOfWork, _loggerFactory, _userManager) { };

            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {
                    result.Articles = await handler.GetArticle(searchString);

                    result.Users = await handler.GetUsers(searchString);

                }
                catch
                {
                    RedirectToAction("Error", "Home");
                }

                return View("FindView", result);
            }

            return View("Blog/FindView", result);
        }

        [Route("ReadArticle/{id}")]
        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadArticle(int id)
        {
            try
            {
                var handler = new BlogHandler(_unitOfWork, _loggerFactory, _userManager) { };

                var article = await handler.GetArticleById(id);

                ReadArticleViewModel result = new ReadArticleViewModel()
                {
                    Id = id,
                    Title = article.Title,
                    Text = article.Text,
                    Tags = article.Tags,
                    Comments = article.Comments,
                    Owner = article.Owner,
                };

                return View("ReadArticle", result);
            }
            catch
            {
                return RedirectToAction("Home", "Error");
            }
        }

        [Route("AddComment")]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(string text, int articleId)
        {
            try
            {
                var handler = new BlogHandler(_unitOfWork, _loggerFactory, _userManager) { };

                User commentOwner = await _userManager.GetUserAsync(User) 
                    ?? throw new UserNotFoundException("Не найден пользователь");

                await handler.CreateComment(articleId, text, commentOwner);

                return StatusCode(200);
            }
            catch
            {
                return RedirectToAction("Home", "Error");
            }
        }
    }
}
