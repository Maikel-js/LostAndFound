using MediatR;
using Microsoft.AspNetCore.Mvc;
using LostAndFound.Application.DTOs.User;
using LostAndFound.Application.Features.Auth.Commands.Register;
using LostAndFound.Application.Features.Auth.Commands.Login;

namespace LostAndFound.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserDto dto)
    {
        var command = new RegisterCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto);
        var result = await _mediator.Send(command);
        
        if (result == null)
            return Unauthorized(new { message = "Email o contraseña incorrectos" });
            
        return Ok(result);
    }
}
