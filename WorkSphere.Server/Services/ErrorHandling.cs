using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.Entities;
using TastyTreats.Types;

namespace WorkSphere.Server.Services
{
    public class ErrorHandling
    {
        public static ObjectResult HandleException(Exception ex, HttpContext httpContext)
        {
            var errors = new List<ValidationError>();

            if (ex is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    errors.Add(new ValidationError
                    {
                        Description = innerException.Message,
                        ErrorType = ErrorType.Model // or appropriate ErrorType
                    });
                }
            }
            else
            {
                errors.Add(new ValidationError
                {
                    Description = ex.Message,
                    ErrorType = ErrorType.Model // or appropriate ErrorType
                });
            }

            return new ObjectResult(new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title = "Internal Server Error",
                status = 500,
                errors,
                traceId = httpContext.TraceIdentifier
            })
            {
                StatusCode = 500
            };
        }
    }
}

