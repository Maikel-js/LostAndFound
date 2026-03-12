using MediatR;
using LostAndFound.Application.Interfaces;
using LostAndFound.Application.Interfaces.Auth;
using LostAndFound.Application.DTOs.User;
using System.Security.Cryptography;
using System.Text;

namespace LostAndFound.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscar usuario por email
        var user = await _userRepository.GetByEmailAsync(request.Dto.Email);
        
        if (user == null) 
            return null; // Devolver nulo significa login fallido / No Autorizado
            
        // 2. Verificar Hash (Simple SHA256 para propósitos educativos)
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Dto.Password));
        var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        
        if (user.PasswordHash != hash)
            return null; // Contraseña incorrecta

        // 3. Generar JWT Token
        var token = _jwtProvider.GenerateToken(user);
        
        // 4. Retornar
        return new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EnrollmentNumber = user.EnrollmentNumber,
                Role = user.Role
            }
        };
    }
}
