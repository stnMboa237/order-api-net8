using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderSystem.Api.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; 

            if (exception is ValidationException)
                statusCode = HttpStatusCode.InternalServerError;

            if (exception is ArgumentException)
                statusCode = HttpStatusCode.BadRequest;

            if (exception is UnauthorizedAccessException)
                statusCode = HttpStatusCode.Unauthorized;

            var response = new ErrorResponse(
                message: "An error occurred while processing your request.",
                statusCode: (int)statusCode,
                details: exception.Message
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }
}
