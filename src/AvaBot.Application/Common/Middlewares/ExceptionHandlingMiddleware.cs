using System.Net;
using System.Text.Json;
using AvaBot.Application.Common.Models.Wrappers;
using AvaBot.Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace AvaBot.Application.Common.Middlewares
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {

            var statusCode = GetStatusCode(exception);
            var response = new APIError((HttpStatusCode)statusCode)
            {
                Message = exception.GetType().Name,
                TraceId = httpContext?.TraceIdentifier,
                ValidationErrors = GetErrors(exception),
                Code = (exception is ValidationException) ? ErrorCodes.Common.ValidationError.Code : null
            };
            if (exception is BadHttpRequestException { StatusCode: StatusCodes.Status413PayloadTooLarge })
            {

                response = new APIError(ErrorCodes.Common.PayloadTooLarge)
                {
                    TraceId = httpContext?.TraceIdentifier,
                    ValidationErrors = GetErrors(exception),
                    Message = exception.Message
                };
                statusCode = StatusCodes.Status413PayloadTooLarge;
            }
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            //ToDo Add logging

            var options = new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                DirectoryNotFoundException => StatusCodes.Status404NotFound,
                DllNotFoundException => StatusCodes.Status404NotFound,
                DriveNotFoundException => StatusCodes.Status404NotFound,
                EntryPointNotFoundException => StatusCodes.Status404NotFound,
                FileNotFoundException => StatusCodes.Status404NotFound,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                TimeZoneNotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
        private static List<ValidationError> GetErrors(Exception exception)
        {
            var errorList = new List<ValidationError>();
            if (exception is ValidationException validationException)
            {
                foreach (var error in validationException.Errors)
                {
                    errorList.Add(new ValidationError(error.PropertyName,
                        error.ErrorMessage,
                        error.ErrorCode ?? ErrorCodes.Common.ValidationError.Code));
                }
            }
            return errorList;
        }
    }
}
