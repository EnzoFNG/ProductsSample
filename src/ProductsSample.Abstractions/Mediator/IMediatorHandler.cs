using ProductsSample.Abstractions.Commands;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Abstractions.Queries;

namespace ProductsSample.Abstractions.Mediator;

public interface IMediatorHandler
{
    ValueTask<Result> SendCommandAsync<TCommand>(TCommand command) where TCommand : Command<Result>;

    ValueTask<Result> SendQueryAsync<TQuery>(TQuery query) where TQuery : Query<Result>;
}