using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public class ArticleRepository : Repository<Article>
    {
        private readonly ApplicationContext _context;

        public ArticleRepository(ApplicationContext db) : base(db)
        {
            _context = db;
        }

        public async Task<Article> GetArticleById(int id)
        {
            return await _context.Articles.FirstOrDefaultAsync(t => t.Id == id) ??
                throw new Exception("Проблема при поиске статьи с данным идентификатором");
        }

        public async Task<List<Article>> GetUserArticles(User user)
        {
            return await _context.Articles.Where(a => a.Owner == user).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTitle(string title)
        {
            return await _context.Articles.Where(a => a.Title.ToUpper().Contains(title.ToUpper())).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTag(Tag tag)
        {
            return await _context.Articles.Where(t => t.Tags.Contains(tag)).ToListAsync();
        }

        public new async Task<List<Article>> GetAll()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<int> DeleteUserArticles(User user)
        {
            var articles = await _context.Articles.Where(a => a.Owner == user).ToListAsync();
            _context.Articles.RemoveRange(articles);
            _context.SaveChanges();
            return articles.Count;
        }
    }
}
