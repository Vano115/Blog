using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Repository;

namespace Blog.Data.UoW
{
    /// <summary>
    /// Паттерн UnitOfWork для доступа к репозиториям единым способом
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();

        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;

    }
}
