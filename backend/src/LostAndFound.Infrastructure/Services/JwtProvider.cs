using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtClaim = System.Security.Claims.Claim;
using System.Text;
using LostAndFound.Application.Interfaces.Auth;
using LostAndFound.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LostAndFound.Infrastructure.Services;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "superSecretKeyQueDeberiasCambiar1234!!"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new JwtClaim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new JwtClaim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new JwtClaim(JwtRegisteredClaimNames.Email, user.Email),
            new JwtClaim("FirstName", user.FirstName),
            new JwtClaim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "LostAndFoundApi",
            audience: _configuration["Jwt:Audience"] ?? "LostAndFoundClients",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
