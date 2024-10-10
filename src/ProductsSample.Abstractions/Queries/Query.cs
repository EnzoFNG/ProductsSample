using Flunt.Notifications;
using MediatR;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Abstractions.Queries;

public abstract class Query : NotifiableQuery<Notification>,
    IQuery,
    IBaseRequest;

public abstract class Query<TResult> : NotifiableQuery<Notification>,
    IQuery<TResult>,
    IBaseRequest
    where TResult : Result;