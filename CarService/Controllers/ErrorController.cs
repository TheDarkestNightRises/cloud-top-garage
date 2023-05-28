using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/Error")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;

        // Handle specific exception types
        if (exception is ArgumentException)
        {
            return BadRequest(exception.Message);
        }
        if (exception is ValidationException)
        {
            return BadRequest(exception.Message);
        }

        // Handle other exceptions
        return StatusCode(500, exception?.Message ?? "An error occurred.");
    }
}