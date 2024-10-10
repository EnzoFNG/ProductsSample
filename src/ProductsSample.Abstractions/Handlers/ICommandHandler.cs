using MediatR;
using ProductsSample.Abstractions.Commands;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Abstractions.Handlers;

public interface ICommandHandler<in TCommand> :
    IRequestHandler<TCommand>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResult> :
    IRequestHandler<TCommand, TResult>
    where TCommand : Command<TResult>
    where TResult : Result;