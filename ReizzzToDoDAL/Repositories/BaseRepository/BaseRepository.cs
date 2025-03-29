using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Filter;

namespace ReizzzToDo.DAL.Repositories.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly ReizzzToDoContext _context;

        public BaseRepository(ReizzzToDoContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
        {
            IQueryable<T> query = _dbSet;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includeFunc != null)
            {
                query = includeFunc(query);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Filter<T>? filterList = null,
                                        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
                                        string[]? orderByProperty = null,
                                        bool[]? isDescending = null)
        {
            if (orderByProperty != null && isDescending != null && orderByProperty.Length != isDescending.Length)
            {
                throw new ArgumentException($"\"{nameof(orderByProperty)}\" and \"{nameof(isDescending)}\" must have the same length");
            }
            IQueryable<T> query = _dbSet;
            if (filterList != null && filterList.Conditions.Count > 0)
            {
                foreach (var condition in filterList.Conditions)
                {
                    query = query.Where(condition);
                }
            }
            if (includeFunc is not null)
            {
                query = includeFunc(query);
            }
            if ((orderByProperty != null && isDescending != null) && ((orderByProperty!.Length > 0) && (isDescending!.Length > 0)))
            {
                var orderedQuery = ApplyOrderBy(query, orderByProperty![0], isDescending![0]);
                if (orderByProperty!.Length > 1)
                {
                    for (int i = 1; i < orderByProperty.Length; i++)
                    {
                        orderedQuery = ThenApplyOrderBy(orderedQuery, orderByProperty[i], isDescending[i]);
                    }
                }
                query = orderedQuery;
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetById(long id)
        {
            T? result = await _dbSet.FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<T>> Pagination(Filter<T>? filterList,
                                                        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
                                                        int page = 1,
                                                        int pageSize = 10,
                                                        string[]? orderByProperty = null,
                                                        bool[]? isDescending = null)
        {
            if (orderByProperty != null && isDescending != null && orderByProperty.Length != isDescending.Length)
            {
                throw new ArgumentException($"\"{nameof(orderByProperty)}\" and \"{nameof(isDescending)}\" must have the same length");
            }

            IQueryable<T> query = _dbSet;
            if (filterList!.Conditions.Count > 0)
            {
                foreach (var condition in filterList.Conditions)
                {
                    query = query.Where(condition);
                }
            }
            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            var orderedQuery = ApplyOrderBy(query, orderByProperty![0], isDescending![0]);
            if (orderByProperty!.Length > 1)
            {
                for (int i = 1; i < orderByProperty.Length; i++)
                {
                    orderedQuery = ThenApplyOrderBy(orderedQuery, orderByProperty[i], isDescending[i]);
                }
            }
            query = orderedQuery;
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public T Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        private IOrderedQueryable<T> ApplyOrderBy(IQueryable<T> query, string orderByProperty, bool descending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(orderByProperty);
            if (property == null)
            {
                throw new ArgumentException($"{entityType} doesn't have property {orderByProperty}");
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            if (descending)
            {
                return Queryable.OrderByDescending(query, (dynamic)orderByExpression);
            }
            return Queryable.OrderBy(query, (dynamic)orderByExpression);
        }
        private IOrderedQueryable<T> ThenApplyOrderBy(IOrderedQueryable<T> query, string orderByProperty, bool descending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(orderByProperty);
            if (property == null)
            {
                throw new ArgumentException($"{entityType} doesn't have property {orderByProperty}");
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            if (descending)
            {
                return query = Queryable.ThenByDescending(query, (dynamic)orderByExpression);
            }
            return query = Queryable.ThenBy(query, (dynamic)orderByExpression);
        }
    }
}
