using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace ELearning.Infrastructure.Base;
public interface IGenericRepository<T> where T : class
{
    // CRUD Operations
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddMultipleAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoveMultipleAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);


    IQueryable<T> Where(Expression<Func<T, bool>> predicate);

    // Query Operations
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        CancellationToken cancellationToken = default);
    public IQueryable<T> Find(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

    Task<T> FirstOrDefaultAsync(
      Expression<Func<T, bool>> predicate,
      Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
      CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    // Pagination and Sorting
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        Expression<Func<T, bool>> predicate,
        int pageNumber,
        int pageSize,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        CancellationToken cancellationToken = default);

    // Specification Pattern
    Task<IEnumerable<T>> FindBySpecificationAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);


    // Attach Operation
    void Attach(T entity);
}
