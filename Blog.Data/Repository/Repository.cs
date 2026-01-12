using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.DbSettings;

namespace Blog.Data.Repository
{
    /// <summary>
    /// Основной класс - репозиторий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _db;
        public DbSet<T> Set
        {
            get;
            private set;
        }

        public Repository(ApplicationContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        public async Task<int> CreateAsync(T item)
        {
            await Set.AddAsync(item);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T item)
        {
            Set.Remove(item);
            return await _db.SaveChangesAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await Set.FindAsync(id);
        }

        public async Task <IEnumerable<T>> GetAll()
        {
            return Set;
        }

        public async Task<int> UpdateAsync(T item)
        {
            Set.Update(item);
            return await _db.SaveChangesAsync();
        }
    }
}
