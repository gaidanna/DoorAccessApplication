using DoorAccessApplication.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DoorAccessApplication.Api.Filters
{
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<UnhandledExceptionFilterAttribute> _logger;

        public UnhandledExceptionFilterAttribute(ILogger<UnhandledExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var error = context.Exception.Message;
            var statusCode = HttpStatusCode.InternalServerError;

            _logger.LogError("Unhandled exception occurred while executing request: {ex}", context.Exception);

            if (context.Exception is InvalidOperationException)
            {
                statusCode = HttpStatusCode.NotFound;
                error = "The specified resource was not found.";
            }
            if (context.Exception is DbUpdateException)
            {
                statusCode = HttpStatusCode.BadRequest;
                error = "The specified resource could not be saved in database.";
            }
            if (context.Exception is EntityUpdateForbiddenException
                || context.Exception is EntityAddForbiddenException
                || context.Exception is EntityDeleteForbiddenException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new JsonResult(new
            {
                error = new[] { error }
            });
        }
    }
}
