using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExceptionHandlerDemo.Exceptions;

public class ValidationExceptionHandler : ExceptionHandler<ValidationException>
{
    protected override IActionResult HandleException(ValidationException exception, ModelStateDictionary modelState, ILogger logger)
    {
        logger.LogInformation(exception, "Validation Exception: {message}", exception.Message);
        modelState.AddModelError("ValidationExceptionMessage", exception.Message);
        
        foreach (var error in exception.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return new BadRequestObjectResult(modelState);
    }
}