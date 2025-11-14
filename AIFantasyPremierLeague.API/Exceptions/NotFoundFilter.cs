using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AIFantasyPremierLeague.API.Exceptions;
public class NotFoundFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TeamNotFoundException or PlayerNotFoundException or PlayerPerformanceNotFoundException)
        {
            var response = new NotFoundObjectResult(new
            {
                Status = 404,
                Error = "Not Found",
                context.Exception.Message
            });

            context.Result = response;
            context.ExceptionHandled = true;
        }
    }
}