
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Entregable_Universities.Models;
using Microsoft.AspNetCore.Http;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ApiResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            // Invoca el siguiente middleware en la cadena
            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Verifica si la respuesta es JSON o si es un error 400/404
            if (context.Response.ContentType?.Contains("application/json") == true ||
                context.Response.StatusCode == StatusCodes.Status400BadRequest ||
                context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                // Determina el mensaje basado en el código de estado
                string message = context.Response.StatusCode switch
                {
                    StatusCodes.Status400BadRequest => "Bad Request",
                    StatusCodes.Status404NotFound => "Resource Not Found",
                    _ => context.Response.StatusCode is >= 200 and < 300 ? "Success" : "Error"
                };

                // Asegurar que el contenido de data siempre sea un objeto o una estructura esperada
                object? data;
                try
                {
                    data = string.IsNullOrWhiteSpace(responseBody) ? null : JsonSerializer.Deserialize<object>(responseBody);
                }
                catch
                {
                    data = responseBody; // Si no se puede deserializar, devuelve el string original
                }

                var apiResponse = new ApiResponseModel<object>
                {
                    message = message,
                    success = context.Response.StatusCode is >= 200 and < 300,
                    data = data,
                    timeStamp = DateTime.UtcNow
                };

                var jsonResponse = JsonSerializer.Serialize(apiResponse);

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = context.Response.StatusCode; // Mantener el código de estado original
                await context.Response.WriteAsync(jsonResponse);
            }
            else
            {
                // Si no es JSON, devuelve la respuesta original
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
            context.Response.Body = originalBodyStream;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var errorResponse = new ApiResponseModel<object>
            {
                message = "An unexpected error occurred.",
                success = false,
                data = ex.Message, // Error directamente dentro de data
                timeStamp = DateTime.UtcNow
            };

            var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonErrorResponse);
        }
    }
}