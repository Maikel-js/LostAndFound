using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LostAndFound.Application;
using LostAndFound.Application.Interfaces.Auth;
using LostAndFound.Infrastructure.Services;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories;
using LostAndFound.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var originsConfig = builder.Configuration["AllowedOrigins"];
        if (string.IsNullOrWhiteSpace(originsConfig))
        {
            policy.AllowAnyOrigin();
        }
        else
        {
            var origins = originsConfig.Split(',')
                .Select(origin => origin.Trim())
                .Where(origin => !string.IsNullOrWhiteSpace(origin))
                .ToArray();

            policy.WithOrigins(origins);
        }

        policy.AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Capa de Aplicación
builder.Services.AddApplication();

// Capa de Infraestructura
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

// Configure Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "LostAndFoundApi",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "LostAndFoundClients",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "superSecretKeyQueDeberiasCambiar1234!!"))
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.SeedAsync(db);
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is ArgumentException or InvalidOperationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { message = exception?.Message ?? "Unexpected error" });
    });
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowFrontend");

// El orden aquí es vital para la seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
