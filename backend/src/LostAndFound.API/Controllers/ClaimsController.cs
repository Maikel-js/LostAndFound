using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Application.Features.Claims.Commands.CreateClaim;
using LostAndFound.Application.Features.Claims.Commands.ApproveClaim;
using LostAndFound.Application.Features.Claims.Commands.RejectClaim;
using LostAndFound.Application.Features.Claims.Queries.GetClaims;
using LostAndFound.Application.Features.Claims.Queries.GetClaimsByUser;

namespace LostAndFound.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClaimsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/claims
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ClaimResponseDto>> CreateClaim([FromBody] CreateClaimDto dto)
    {
        var userId = GetUserId();
        var command = new CreateClaimCommand(dto, userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // GET: api/claims?itemId=...&status=Pending
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<IEnumerable<ClaimResponseDto>>> GetClaims([FromQuery] Guid? itemId, [FromQuery] string? status)
    {
        var query = new GetClaimsQuery(itemId, status);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // GET: api/claims/mine
    [HttpGet("mine")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ClaimResponseDto>>> GetMyClaims()
    {
        var userId = GetUserId();
        var query = new GetClaimsByUserQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    // PUT: api/claims/{id}/approve
    [HttpPut("{id:guid}/approve")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ClaimResponseDto>> ApproveClaim(Guid id)
    {
        var adminId = GetUserId();
        var command = new ApproveClaimCommand(id, adminId);
        var result = await _mediator.Send(command);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // PUT: api/claims/{id}/reject
    [HttpPut("{id:guid}/reject")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ClaimResponseDto>> RejectClaim(Guid id)
    {
        var adminId = GetUserId();
        var command = new RejectClaimCommand(id, adminId);
        var result = await _mediator.Send(command);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    private Guid GetUserId()
    {
        var raw = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(raw) || !Guid.TryParse(raw, out var userId))
            throw new InvalidOperationException("Token inválido: no se pudo obtener el ID de usuario.");

        return userId;
    }
}
