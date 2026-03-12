using LostAndFound.Application.DTOs.User;
using MediatR;

namespace LostAndFound.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public RegisterUserDto Dto { get; set; }

    public RegisterCommand(RegisterUserDto dto)
    {
        Dto = dto;
    }
}
