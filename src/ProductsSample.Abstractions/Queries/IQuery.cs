using MediatR;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Abstractions.Queries;

public interface IQuery : IRequest
{ }

public interface IQuery<out TResult> : IRequest<TResult>
    where TResult : Result
{ }