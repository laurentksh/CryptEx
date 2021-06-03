using System;
using System.Net;
using CryptExApi.Exceptions;
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
            var problemDetails = new ProblemDetails
            {
                Type = $"/Problems/{problem.problemType}",
                Title = problem.httpStatus.ToString(),
                Status = (int)problem.httpStatus,
                Detail = problem.problemMsg,
                Instance = request?.Path,
            };

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        private static (HttpStatusCode httpStatus, string problemType, string problemMsg) GetProblem(Exception exception)
        {
            var exMsg = exception.Message ?? "Unspecified";

            return exception switch
            {
                UnauthorizedException => (HttpStatusCode.Unauthorized, "Unauthorized", exMsg),
                BadRequestException => (HttpStatusCode.BadRequest, "BadRequest", exMsg),
                FormatException or ArgumentException => (HttpStatusCode.BadRequest, "BadRequest", exMsg),
                NotImplementedException => (HttpStatusCode.NotImplemented, "NotImplemented", exMsg),
                NullReferenceException => (HttpStatusCode.InternalServerError, "InternalServerError", "Unspecified server error"),
                NotFoundException => (HttpStatusCode.NotFound, "NotFound", exMsg),
                CryptoApiException => (HttpStatusCode.ServiceUnavailable, "Unavailable", "The service is unavailable at the moment, please try again later"),
                _ => (HttpStatusCode.InternalServerError, "InternalServerError", "Unspecified server error")
            };
        }
    }
}
