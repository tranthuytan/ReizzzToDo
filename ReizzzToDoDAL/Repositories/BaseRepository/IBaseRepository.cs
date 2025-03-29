using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ReizzzToDo.DAL.Filter;

namespace ReizzzToDo.DAL.Repositories.BaseRepository
{
    public interface IBaseRepository<T> where T : class
    {
        public Task AddAsync(T entity);
        public Task<T?> GetById(long id);
        public Task<IEnumerable<T>> GetAll(Filter<T>? filterList = null,
                                        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
                                        string[]? orderByProperty = null,
                                        bool[]? isDescending = null);
        public Task<IEnumerable<T>> Pagination(Filter<T>? filterList = null,
                                        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
                                        int page = 1,
                                        int pageSize = 10,
                                        string[]? orderByProperty = null,
                                        bool[]? isDescending = null);
        public T Update(T entity);
        public void Delete(T entity);
        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? expression = null,
                                        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);
        public Task SaveChangesAsync();
    }
}
