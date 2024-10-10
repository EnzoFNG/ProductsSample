using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Abstractions.Queries;

public abstract class PaginatedQuery<TResult> : Query<TResult>
    where TResult : Result
{
    private const int _maxSize = 100;
    private int _size = 50;

    public int Page { get; set; } = 1;

    public int Size
    {
        get
        {
            return _size;
        }
        set
        {
            _size = Math.Min(_maxSize, value);
        }
    }

    [BindNever]
    public int Skip => Size * (Page - 1);

    public void ValidatePaginate()
    {
        AddNotifications(
            ContractRequires
            .IsFalse(_size is > 50 or < 1, "PaginatedQuery", "The page size must be between 1 and 50"));
    }
}