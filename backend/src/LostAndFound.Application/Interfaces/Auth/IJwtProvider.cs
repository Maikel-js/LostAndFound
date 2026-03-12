using LostAndFound.Domain.Entities;

namespace LostAndFound.Application.Interfaces.Auth;

// Esta interfaz vive en Application, pero su implementación vivirá en Infrastructure
public interface IJwtProvider
{
    string GenerateToken(User user);
}
