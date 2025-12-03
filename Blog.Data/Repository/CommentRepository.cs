using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.DbSettings;
using Blog.Data.Entityes;

namespace Blog.Data.Repository
{
    public class CommentRepository : Repository<Comment>
    {
        private readonly ApplicationContext _context;

        public CommentRepository(ApplicationContext db) : base(db)
        {
            _context = db;
        }
    }
}
