using LostAndFound.Application.DTOs.User;
using MediatR;

namespace LostAndFound.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<AuthResponseDto?>
{
    public LoginDto Dto { get; set; }

    public LoginCommand(LoginDto dto)
    {
        Dto = dto;
    }
}
