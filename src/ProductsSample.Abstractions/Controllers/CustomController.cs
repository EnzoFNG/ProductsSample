using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using ProductsSample.Abstractions.Primitives;

namespace ProductsSample.Abstractions.Controllers;

[ApiController]
public class CustomController : ControllerBase
{
    protected sealed record ErrorResponse(string Key, string Message);

    private readonly List<ErrorResponse> _errors = [];

    protected ActionResult CustomResponse(Result result)
    {
        if (result is { IsSuccess: false, Response: null})
            return NotFound();

        if (result!.Response!.GetType() == typeof(List<Notification>))
        {
            var notifications = result.Response! as List<Notification>;
            foreach (var error in notifications!)
            {
                var errorResponse = new ErrorResponse(error!.Key, error.Message);
                AddError(errorResponse);
            }
        }
        else if (typeof(Notification).IsAssignableFrom(result!.Response.GetType()))
        {
            var notification = result.Response! as Notification;
            var errorResponse = new ErrorResponse(notification!.Key, notification.Message);
            AddError(errorResponse);
        }
        else if (result.IsSuccess is false)
        {
            var error = result.Response! as string;
            var errorResponse = new ErrorResponse(GetType().Name, error!);
            AddError(errorResponse);
        }

        if (!IsOperationValid())
            return BadRequest(new Dictionary<string, ErrorResponse[]>
            {
                { $"errors", _errors.ToArray() }
            });

        if (result.Response is string response)
        {
            var successResponse = new SuccessResponse(response);
            return Ok(successResponse);
        }

        return Ok(result.Response!);
    }

    protected bool IsOperationValid() => _errors.Count == 0;
    protected void AddError(ErrorResponse error) => _errors.Add(error);
    public sealed record SuccessResponse(string Message);
}