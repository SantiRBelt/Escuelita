using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ApiResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Excluir las rutas de Swagger
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/swagger/v1/swagger.json"))
        {
            await _next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Verificar si la respuesta ya está envuelta
            var isAlreadyWrapped = responseBody.Contains("\"success\":") && responseBody.Contains("\"message\":");

            if (!isAlreadyWrapped && context.Response.ContentType?.Contains("application/json") == true)
            {
                var response = new ApiResponse<object>
                {
                    Message = context.Response.StatusCode == 200 ? "Success" : "Error",
                    TimeStamp = DateTime.UtcNow,
                    Success = context.Response.StatusCode is >= 200 and < 300,
                    Data = string.IsNullOrWhiteSpace(responseBody) ? null : JsonSerializer.Deserialize<object>(responseBody)
                };

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
            else
            {
                // Si la respuesta ya está envuelta o no es JSON, devolver el cuerpo original
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errorResponse = new ApiResponse<object>
            {
                Message = "An unexpected error occurred.",
                TimeStamp = DateTime.UtcNow,
                Success = false,
                Data = new { error = ex.Message }
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}

public class ApiResponse<T>
{
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; }
    public bool Success { get; set; }
    public T Data { get; set; }
}