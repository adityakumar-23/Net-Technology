using Microsoft.OpenApi.Models;
using StudentManagementAPI.Data;
using StudentManagementAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register ADO.NET Repository
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Configure Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Student Management REST API",
        Version = "v1",
        Description = "A complete and professional Student Management REST API developed with ASP.NET Core (.NET 8 Web API), ADO.NET and SQL Server (Experiment 10).",
        Contact = new OpenApiContact
        {
            Name = "Aditya Kumar",
            Url = new Uri("https://github.com/adityakumar-23/Net-Technology")
        }
    });
});

var app = builder.Build();

// Auto-initialize SQL Server Database & Seed Data
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    DatabaseInitializer.Initialize(config, logger);
}

// Custom Exception Handling Middleware (must be first in pipeline)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Custom Request Logging Middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API v1");
    c.RoutePrefix = "swagger";
});

// Redirect root to Swagger UI for easy accessibility
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
