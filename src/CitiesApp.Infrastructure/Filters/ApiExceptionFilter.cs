using CitiesApp.Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CitiesApp.Infrastructure.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is EntityNotFoundException)
            {
                context.Result = new NotFoundResult();
            }
        }
    }
}
