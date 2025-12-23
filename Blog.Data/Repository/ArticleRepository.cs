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

        public async Task<List<Article>> GetUserArticles(User user)
        {
            return await _context.Articles.Where(a => a.Owner == user).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTitle(string title)
        {
            return await _context.Articles.Where(a => a.Title == title).ToListAsync();
        }

        public async Task<List<Article>> GetArticlesByTag(Tag tag)
        {
            return await _context.Articles.Where(t => t.Tags.Contains(tag)).ToListAsync();
        }
    }
}
