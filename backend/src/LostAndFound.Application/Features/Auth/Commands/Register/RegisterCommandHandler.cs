using MediatR;
using LostAndFound.Domain.Entities;
using LostAndFound.Domain.Enums;
using LostAndFound.Application.Interfaces;
using LostAndFound.Application.Interfaces.Auth;
using LostAndFound.Application.DTOs.User;
using System.Security.Cryptography;
using System.Text;

namespace LostAndFound.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar si existe (pseudo-código, omitido por brevedad, asumiendo que el controlador lo valida o se atrapa en la BD)
        
        // 2. Hashear password
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.Dto.Password));
        var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

        // 3. Crear User
        var user = new User
        {
            FirstName = request.Dto.FirstName,
            LastName = request.Dto.LastName,
            Email = request.Dto.Email,
            EnrollmentNumber = request.Dto.EnrollmentNumber,
            PasswordHash = hash,
            Role = UserRole.Student // Default
        };

        var createdUser = await _userRepository.AddAsync(user);

        // 4. Generar Token
        var token = _jwtProvider.GenerateToken(createdUser);

        // 5. Retornar
        return new AuthResponseDto
        {
            Token = token,
            User = new UserResponseDto
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                EnrollmentNumber = createdUser.EnrollmentNumber,
                Role = createdUser.Role
            }
        };
    }
}
