using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NoteNinja.Repositories
{
    public interface IRepository<T>
    {

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
