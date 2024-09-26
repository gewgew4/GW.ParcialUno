using Domain;
using System.Linq.Expressions;

namespace Infrastructure.Repo.Interfaces;

public interface IGenericRepo<T> where T : BaseEntity<T>
{
    Task<T> GetById(int id, bool tracking = true, params string[] includeProperties);
    Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, bool tracking = true, params string[] includeProperties);

    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task Remove(int id);
    Task Remove(T entity);
    Task<List<T>> GetAll(bool tracking = true, params string[] includeProperties);
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate,
                                  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                  int? top = null,
                                  int? skip = null,
                                  params string[] includeProperties);
}
