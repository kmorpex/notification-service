using AutoWrapper.Wrappers;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NotificationService.WebAPI.Controllers;

[ApiController]
[Route("/api/v{version:apiVersion}/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public abstract class ApiControllerAbstract : ControllerBase
{
    protected new static ApiResponse Response(object? result = null, int status = 200)
    {
        return new ApiResponse(result, status);
    }

    protected static ApiResponse NotFoundResponse(object? result = null)
    {
        return new ApiResponse(result, 404);
    }

    protected static ApiResponse ErrorResponse(string title = "Server error", int statusCode = 500)
    {
        throw new ApiProblemDetailsException(title, statusCode);
    }

    protected static ApiResponse ErrorResponse(ModelStateDictionary modelState)
    {
        throw new ApiProblemDetailsException(modelState);
    }

    protected new ApiResponse Response<T>(ErrorOr<T> result)
    {
        if (result.Value is not null && !result.IsError)
        {
            return new ApiResponse(result.Value);
        }

        return new ApiResponse(Problem(result.Errors));
    }


    private ApiResponse Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return ErrorResponse();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblems(errors);
        }

        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        return Problems(errors[0]);
    }

    private ApiResponse Problems(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        return ErrorResponse(statusCode: statusCode, title: error.Description);
    }

    private ApiResponse ValidationProblems(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ErrorResponse(modelStateDictionary);
    }
}

public static class HttpContextItemKeys
{
    public const string Errors = "errors";
}