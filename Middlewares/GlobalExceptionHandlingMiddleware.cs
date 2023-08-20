using ClassroomPlus.Exceptions;
using ClassroomPlus.Middlewares;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

using KeyNotFoundException = ClassroomPlus.Exceptions.KeyNotFoundException;
using UnauthorizedContentException = ClassroomPlus.Exceptions.UnauthorizedContentException;


namespace ClassroomPlus.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
             await _next(context);
        }
        catch(Exception ex) 
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    public static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode status;
        string message = "";

        var exceptionType = ex.GetType();

        if (exceptionType == typeof(BadRequestException))
        {
            message = ex.Message;
            status = HttpStatusCode.BadRequest;
        }
        else if (exceptionType == typeof(NotFoundException))
        {
            message = ex.Message;
            status = HttpStatusCode.NotFound;
        }
        else if (exceptionType == typeof(NotImplementedException))
        {
            message = ex.Message;
            status = HttpStatusCode.NotImplemented;
        }

        else if (exceptionType == typeof(KeyNotFoundException))
        {
            message = ex.Message;
            status = HttpStatusCode.NotFound;
        }

        else if (exceptionType == typeof(UnauthorizedContentException))
        {
            message = ex.Message;
            status = HttpStatusCode.Unauthorized;
        }
        else if (exceptionType == typeof(LimitedCountException))
        {
            message = ex.Message;
            status = HttpStatusCode.Forbidden;
        }
        else if (exceptionType == typeof(EmailAlreadyExistException))
        {
            message = ex.Message;
            status = HttpStatusCode.ServiceUnavailable;
        }
        else if (exceptionType == typeof(UsernameAlreadyExistException))
        {
            message = ex.Message;
            status = HttpStatusCode.ServiceUnavailable;
        }
        else if (exceptionType == typeof(ShortPasswordException))
        {
            message = ex.Message;
            status = HttpStatusCode.NotAcceptable;
        }
        else if (exceptionType == typeof(WrongPasswordException))
        {
            message = ex.Message;
            status = HttpStatusCode.Forbidden;
        }
        else if(exceptionType == typeof(EmailNotValidException))
        {
            message = ex.Message;
            status = HttpStatusCode.ServiceUnavailable;
        }
        else
        {
            message = ex.Message;
            status = HttpStatusCode.InternalServerError;
        }

        var exceptionResult = JsonSerializer.Serialize(new { error = message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(exceptionResult);
    }
}

public static class GlobalErrorHandlerExtensions 
{
    public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
