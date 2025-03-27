using System.Net;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.Exceptions;

namespace MzadPalestine.API.Middleware;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ غير متوقع: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ProblemDetails();

        switch (exception)
        {
            case AppException e:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Title = "خطأ في الطلب";
                response.Detail = e.Message;
                _logger.LogWarning("خطأ في الطلب: {Message}", e.Message);
                break;

            case NotFoundException e:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Status = (int)HttpStatusCode.NotFound;
                response.Title = "لم يتم العثور على العنصر";
                response.Detail = e.Message;
                _logger.LogInformation("عنصر غير موجود: {Message}", e.Message);
                break;

            case UnauthorizedAccessException e:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Status = (int)HttpStatusCode.Unauthorized;
                response.Title = "غير مصرح";
                response.Detail = "غير مصرح لك بالوصول إلى هذا المورد";
                _logger.LogWarning("محاولة وصول غير مصرح: {Message}", e.Message);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Title = "خطأ في الخادم";
                response.Detail = _env.IsDevelopment() 
                    ? exception.Message 
                    : "حدث خطأ داخلي في الخادم";
                _logger.LogError(exception, "خطأ غير معالج: {Message}", exception.Message);
                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}
