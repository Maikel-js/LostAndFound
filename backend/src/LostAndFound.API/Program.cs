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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["AllowedOrigins"]?.Split(',') ?? ["http://localhost:5173", "http://localhost:3000"])
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Capa de Aplicación
builder.Services.AddApplication();

// Capa de Infraestructura
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// El orden aquí es vital para la seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
