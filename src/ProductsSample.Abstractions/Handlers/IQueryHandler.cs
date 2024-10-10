using MediatR;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Abstractions.Queries;

namespace ProductsSample.Abstractions.Handlers;

public interface IQueryHandler<in TQuery> :
    IRequestHandler<TQuery>
    where TQuery : IQuery;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : Query<TResult>
    where TResult : Result;