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
    public class CommentRepository : Repository<Comment>
    {
        private readonly ApplicationContext _context;

        public CommentRepository(ApplicationContext db) : base(db)
        {
            _context = db;
        }

        public async Task<int> DeleteArticlesComments(Article article)
        {
            var comments = await _context.Comments.Where(x => x.ArticleId == article.Id).ToListAsync();
            _context.Comments.RemoveRange(comments);
            return await _context.SaveChangesAsync();
        }
    }
}
