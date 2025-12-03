using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    /// <summary>
    /// Интерфейс для работы репозиториев
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        // Разбор: IRepository<T> T - обозначает тип данных который будет наследоваться от
        // этого репозитория и при применении T Get(int id); - мы получим обьект этого класса
        IEnumerable<T> GetAll();
        Task<T?> GetAsync(int id);
        Task<int> CreateAsync(T item);
        Task<int> UpdateAsync(T item);
        Task<int> DeleteAsync(T item);
    }
}
