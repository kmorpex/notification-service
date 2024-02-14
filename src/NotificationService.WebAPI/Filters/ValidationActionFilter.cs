using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NotificationService.WebAPI.Filters;

public class ValidationActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            throw new ApiProblemDetailsException(context.ModelState);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
