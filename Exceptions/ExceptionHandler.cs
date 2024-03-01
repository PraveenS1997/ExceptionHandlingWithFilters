using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExceptionHandlerDemo.Exceptions;

public interface IExceptionHandler
{
    public bool Matches(ExceptionContext context);

    public void Handle(ExceptionContext context, ILogger logger);
}

public abstract class ExceptionHandler<T> : IExceptionHandler
    where T : Exception
{
    public bool Matches(ExceptionContext context)
    {
        return context.Exception is T;
    }

    public void Handle(ExceptionContext context, ILogger logger)
    {
        var result = HandleException((T)context.Exception, context.ModelState, logger);

        context.Result = result;
        context.ExceptionHandled = true;
    }

    protected abstract IActionResult HandleException(T exception, ModelStateDictionary modelState,
        ILogger logger);
}