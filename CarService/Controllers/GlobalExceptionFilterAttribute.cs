using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        // Handle specific exceptions
        if (exception is ArgumentException)
        {
            context.Result = new BadRequestObjectResult(exception.Message);
            return;
        }
        if (exception is ValidationException)
        {
            context.Result = new BadRequestObjectResult(exception.Message);
            return;
        }
        // Handle other exceptions
        context.Result = new ObjectResult(exception.Message)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }
}
