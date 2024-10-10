using MediatR;
using ProductsSample.Abstractions.Primitives;
using System.Text.Json.Serialization;

namespace ProductsSample.Abstractions.Commands;

public abstract class Command<TResult> : NotifiableObject<Command<TResult>>,
    ICommand<TResult>,
    IBaseRequest
    where TResult : Result;

public abstract class CommandWithId<TResult> : Command<TResult>
    where TResult : Result
{
    [JsonIgnore]
    public Ulid Id { get; set; }

    public Command<TResult> WithId(Ulid id)
    {
        Id = id;
        ValidateId();
        return this;
    }

    private void ValidateId()
    {
        AddNotifications(
            ContractRequires
            .IsFalse(Id == Ulid.Empty, Key, "Id is invalid."));
    }
}