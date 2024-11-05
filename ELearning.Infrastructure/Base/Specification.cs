using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infrastructure.Base;
public class Specification<T> : ISpecification<T>
{
    public Specification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
        Includes = new List<Func<IQueryable<T>, IQueryable<T>>>();
    }

    public Expression<Func<T, bool>> Criteria { get; }

    public List<Func<IQueryable<T>, IQueryable<T>>> Includes { get; }

    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set; }

    public int? Skip { get; private set; }
    public int? Take { get; private set; }

    public Specification<T> AddInclude(Func<IQueryable<T>, IQueryable<T>> includeExpression)
    {
        Includes.Add(includeExpression);
        return this;
    }

    public Specification<T> ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
    {
        OrderBy = orderByExpression;
        return this;
    }

    public Specification<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        return this;
    }
}

