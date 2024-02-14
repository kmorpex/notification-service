// using System.Net;
// using AutoWrapper.Wrappers;
// using FluentValidation;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.AspNetCore.Mvc.ModelBinding;
//
// namespace NotificationService.WebAPI.Middlewares;
//
// public class ExceptionFilter : IExceptionFilter
// {
//     private readonly IWebHostEnvironment _env;
//
//     public ExceptionFilter(IWebHostEnvironment env)
//     {
//         _env = env;
//     }
//
//     public void OnException(ExceptionContext context)
//     {
//         Console.WriteLine("ExceptionFilter.OnException");
//         var ex = context.Exception;
//
//         if (ex is ApiProblemDetailsException || ex is ApiException)
//         {
//             return;
//         }
//
//         var code = ex switch
//         {
//             // AccessDeniedException => HttpStatusCode.Forbidden,
//             // AuthorizationException => HttpStatusCode.Unauthorized,
//             // NotFoundException => HttpStatusCode.NotFound,
//             // ApplicationLayerException => HttpStatusCode.BadRequest,
//             // DomainException => HttpStatusCode.Conflict,
//             // SecurityTokenException => HttpStatusCode.Unauthorized,
//             // ValidationException => HttpStatusCode.BadRequest,
//             _ => HttpStatusCode.InternalServerError
//         };
//
//         if (ex is ValidationException exception)
//         {
//             var modelState = new ModelStateDictionary();
//             foreach (var err in exception.Errors)
//             {
//                 modelState.AddModelError(err.PropertyName, err.ErrorMessage);
//             }
//
//             throw new ApiProblemDetailsException(modelState);
//         }
//
//         if (_env.IsDevelopment())
//         {
//             Console.WriteLine(ex);
//         }
//
//         switch ((int)code)
//         {
//             case 401:
//                 throw new ApiProblemDetailsException("Unauthorized", (int)code);
//             case 500:
//                 {
//                     const string title = "Internal Server Error";
//
//                     if (!_env.IsDevelopment())
//                     {
//                         throw new ApiProblemDetailsException(title, (int)code);
//                     }
//
//                     var details = new ProblemDetails { Title = title, Detail = ex.ToString(), Status = (int)code };
//
//                     throw new ApiProblemDetailsException(details);
//                 }
//             default:
//                 throw new ApiProblemDetailsException(ex.Message, (int)code);
//         }
//     }
// }
