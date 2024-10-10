using MediatR;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Abstractions.Commands;

public interface ICommand : IRequest
{ }

public interface ICommand<out TResult> : IRequest<TResult>
    where TResult : Result
{ }