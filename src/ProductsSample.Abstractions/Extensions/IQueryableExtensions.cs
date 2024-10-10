using System.Linq.Expressions;

namespace ProductsSample.Abstractions.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> When<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> where)
        where T : class
    {
        if (!condition)
            return query;

        return query.Where(where);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int take, int skip)
    {
        return query.Skip(skip).Take(take);
    }
}