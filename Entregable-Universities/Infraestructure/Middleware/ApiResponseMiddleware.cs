﻿using System.Text.Json;
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
        // Captura el flujo de respuesta original
        var originalBodyStream = context.Response.Body;

        // Crea un nuevo flujo de memoria para manipular la respuesta
        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            // Invoca el siguiente middleware en la cadena
            await _next(context);

            // Verifica si la respuesta ya está envuelta (para evitar envolverla dos veces)
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Solo envolver respuestas JSON
            if (context.Response.ContentType?.Contains("application/json") == true)
            {
                // Deserializa la respuesta original
                var originalResponse = JsonSerializer.Deserialize<object>(responseBody);

                // Crea la respuesta estandarizada
                var apiResponse = new ApiResponse<object>
                {
                    message = context.Response.StatusCode is >= 200 and < 300 ? "Success" : "Error",
                    success = context.Response.StatusCode is >= 200 and < 300,
                    data = originalResponse,
                    timeStamp = DateTime.UtcNow
                };

                // Serializa la respuesta estandarizada
                var jsonResponse = JsonSerializer.Serialize(apiResponse);

                // Escribe la respuesta estandarizada en el cuerpo de la respuesta
                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
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

            var errorResponse = new ApiResponse<object>
            {
                message = "An unexpected error occurred.",
                success = false,
                data = new { error = ex.Message },
                timeStamp = DateTime.UtcNow
            };

            var jsonErrorResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonErrorResponse);
        }
    }
}

public class ApiResponse<T>
{
    public string message { get; set; }
    public DateTime timeStamp { get; set; }
    public bool success { get; set; }
    public T data { get; set; }
}