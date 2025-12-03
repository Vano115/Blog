using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blog.Data.DbSettings;
using Blog.Data.Repository;

namespace Blog.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _appContext;

        private Dictionary<Type, object> _repositories = null!;

        public UnitOfWork(ApplicationContext context)
        {
            _appContext = context;
        }

        public void Dispose()
        {

        }

        public async Task<int> CompleteAsync()
        {
            return await _appContext.SaveChangesAsync();
        }

        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            if (hasCustomRepository)
            {
                var customRepo = _appContext.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_appContext);
            }

            return (IRepository<TEntity>)_repositories[type];

        }
    }
}
