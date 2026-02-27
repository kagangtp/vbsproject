
using System.Diagnostics;
using System.Security.Claims;
using IlkProjem.Core.Exceptions;
using IlkProjem.Core.Models;
using IlkProjem.DAL.Data;

namespace IlkProjem.API.Middlewares;
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        var watch = Stopwatch.StartNew();

        // 1. Request'i Yakala
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        // 2. Response'u Yakalamak İçin Stream Hazırla
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        string? innerErrorCode = null;
        string? exceptionMessage = null;

        try
        {
            await _next(context); // Business katmanına gönder
        }
        catch (Exception ex)
        {
            exceptionMessage = ex.Message;

            if (ex is BusinessException bEx)
            {
                innerErrorCode = bEx.ErrorCode.ToString();
                context.Response.StatusCode = 400; // Business hataları → 400
            }
            else
            {
                context.Response.StatusCode = 500; // Beklenmeyen hatalar → 500
            }

            // Hata response'unu yaz (GlobalExceptionHandler'a gerek kalmadan)
            context.Response.ContentType = "application/json";
            var errorResponse = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = ex is BusinessException ? exceptionMessage : "Sunucu hatası oluştu.",
                errorCode = innerErrorCode
            });
            await context.Response.WriteAsync(errorResponse);
        }
        finally
        {
            watch.Stop();

            // 3. Kullanıcı bilgisini al (Authentication çalıştıktan sonra)
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 4. Response'u Oku
            responseBody.Position = 0;
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Position = 0;

            // 4. Log kaydını oluştur ve veritabanına yaz
            var log = new ServiceLog
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                RequestBody = requestBody,
                ResponseBody = responseText,
                StatusCode = context.Response.StatusCode,
                InternalErrorCode = innerErrorCode,
                Exception = exceptionMessage,
                DurationMs = watch.ElapsedMilliseconds,
                UserId = userId
            };

            dbContext.ServiceLog.Add(log);
            await dbContext.SaveChangesAsync();

            // 5. Orijinal stream'e kopyala (response client'a gitsin)
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}