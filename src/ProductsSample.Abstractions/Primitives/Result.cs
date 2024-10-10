using Flunt.Notifications;

namespace ProductsSample.Abstractions.Primitives;

public class Result
{
    protected Result(bool isSuccess, object? response)
    {
        IsSuccess = isSuccess;
        Response ??= response;
    }

    public bool IsSuccess { get; private set; }
    public object? Response { get; private set; } = null;

    public static Result Success(object response) => new(true, response);
    public static Result Fail(object? response = null) => new(false, response);
}