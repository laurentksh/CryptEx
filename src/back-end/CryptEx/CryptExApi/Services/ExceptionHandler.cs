using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptExApi.Services
{
    public interface IExceptionHandlerService
    {
        public IActionResult Handle(Exception exception, HttpRequest request);
    }

    public class DefaultExceptionHandlerService : IExceptionHandlerService
    {
        public IActionResult Handle(Exception exception, HttpRequest request)
        {
            var problem = GetProblem(exception);
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = $"/Problems/{problem.problemType}",
                Title = problem.httpStatus.ToString(),
                Status = (int)problem.httpStatus,
                Detail = problem.problemMsg,
                Instance = request?.Path
            };

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        private static (HttpStatusCode httpStatus, string problemType, string problemMsg) GetProblem(Exception exception)
        {
            var exMsg = exception.Message ?? "Unspecified";

            return exception switch
            {
                FormatException or ArgumentException => (HttpStatusCode.BadRequest, "BadRequest", exMsg),
                NotImplementedException => (HttpStatusCode.NotImplemented, "NotImplemented", exMsg),
                NullReferenceException => (HttpStatusCode.InternalServerError, "InternalServerError", "Unspecified server error"),
                _ => (HttpStatusCode.InternalServerError, "InternalServerError", "Unspecified server error")
            };
        }
    }
}
