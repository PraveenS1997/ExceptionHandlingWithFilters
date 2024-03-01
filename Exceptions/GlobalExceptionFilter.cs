#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExceptionHandlerDemo.Exceptions;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<GlobalExceptionFilter> _logger;

    private static readonly IEnumerable<IExceptionHandler> DefaultHandlers = new List<IExceptionHandler>()
    {
        new ValidationExceptionHandler(),
        new UnauthorizedExceptionsHandler()
    };
    
    public GlobalExceptionFilter(IHostEnvironment hostEnvironment, ILogger<GlobalExceptionFilter> logger)
    {
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        foreach (var handler in GetExceptionHandlers())
        {
            if (!handler.Matches(context)) continue;
            
            handler.Handle(context, _logger);
            
            if (context.ExceptionHandled)
            {
                _logger.LogTrace("Exception handled by core exception filter ({filter})", handler.GetType().FullName);
                break;
            }
        }

        if (!context.ExceptionHandled && _hostEnvironment.IsDevelopment())
        {
            var result = new
            {
                ServerError = new
                {
                    Type = context.Exception.GetType().Name,
                    context.Exception.Message,
                    context.Exception.StackTrace,
                },
            };

            context.Result = new ObjectResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    protected virtual IEnumerable<IExceptionHandler> GetCustomExceptionHandlers()
    {
        return Enumerable.Empty<IExceptionHandler>();
    }
    
    protected virtual IEnumerable<IExceptionHandler> GetExceptionHandlers()
    {
        return DefaultHandlers.Concat(GetCustomExceptionHandlers());
    }
}