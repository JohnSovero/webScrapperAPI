using AspNetCoreRateLimit;
using backend.Config;
using backend.Infraestructure;
using backend.Middleware;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers and JSON serialization
builder.Services.AddControllers(options => options.Filters.Add<RestExceptionHandler>())
            .AddJsonOptions(jsonOptions =>
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Cache and Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Configuration of Database
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services of the application
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

// Configuration of swagger
SwaggerConfig.AddSwaggerDocumentation(builder.Services);

// Configuration of authentication
AuthenticationConfig.AddCustomAuthentication(builder.Services);

builder.Services.AddTransient<ScrapperService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

// Configuration of CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuration of Rate Limiting
RateLimitConfig.AddRateLimiting(builder.Services);

var app = builder.Build();

// CORS
app.UseCors("AllowAll");
// Swagger
app.UseSwagger();
app.UseSwaggerUI();



// Configuration of authentication and authorization
app.UseAuthentication();
app.UseIpRateLimiting();
app.UseAuthorization();


// Map controllers
app.MapControllers();
app.Run();