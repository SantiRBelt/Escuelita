
using System.Net;
using Entregable_Universities;
using Entregable_Universities.Services;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

const string connectionName = "u963527317_student";
var connectionString = builder.Configuration.GetConnectionString(connectionName);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddJwtTokenServices(builder.Configuration);


//Add custom Services
builder.Services.AddScoped<IstudentsService, StudentsService>();

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(IPAddress.Any, 60);  // Escuchar en el puerto 60
//});

var app = builder.Build();
app.UseMiddleware<ApiResponseMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.Run();
