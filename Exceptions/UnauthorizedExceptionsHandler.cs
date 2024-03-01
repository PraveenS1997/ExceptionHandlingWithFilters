using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExceptionHandlerDemo.Exceptions;

public class UnauthorizedExceptionsHandler : ExceptionHandler<UnauthorizedAccessException>
{
    protected override IActionResult HandleException(UnauthorizedAccessException exception, ModelStateDictionary modelState, ILogger logger)
    {
        logger.LogWarning(exception, "Unauthorized: {message}", exception.Message);
        
        return new UnauthorizedResult();
    }
}