using MediatR;
using ProductsSample.Abstractions.Commands;
using ProductsSample.Abstractions.Primitives;
using ProductsSample.Abstractions.Queries;

namespace ProductsSample.Abstractions.Mediator;

public sealed class MediatorHandler(IMediator mediator) : IMediatorHandler
{
    public async ValueTask<Result> SendCommandAsync<T>(T command)
        where T : Command<Result>
    {
        command.Validate();

        if (!command.IsValid)
            return Result.Fail(command.Notifications);

        return await mediator.Send(command);
    }

    public async ValueTask<Result> SendQueryAsync<T>(T query)
        where T : Query<Result>
    {
        query.Validate();

        if (!query.IsValid)
            return Result.Fail(query.Notifications);

        return await mediator.Send(query);
    }
}