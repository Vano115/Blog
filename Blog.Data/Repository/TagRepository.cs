using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.DbSettings;
using Blog.Data.Entityes;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Repository
{
    public class TagRepository : Repository<Tag>
    {
        private readonly ApplicationContext _context;

        public TagRepository(ApplicationContext db) : base(db)
        {
            _context = db;
        }

        public async Task<Tag> FindByNameAsync (string name)
        {
            var result = await _context.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            return result;
        }

        public async Task<bool> CheckByNameAsync(string name)
        {
            var result = await _context.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
