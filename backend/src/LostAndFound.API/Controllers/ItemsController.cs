using MediatR;
using Microsoft.AspNetCore.Mvc;
using LostAndFound.Application.DTOs.Item;
using LostAndFound.Application.Features.Items.Commands.CreateItem;
using LostAndFound.Application.Features.Items.Queries.GetItems;
using LostAndFound.Application.Features.Items.Queries.GetItemById;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace LostAndFound.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/items
    [HttpPost]
    [Authorize(Roles = "Administrator")] // 🔒 Solo admin
    public async Task<ActionResult<ItemResponseDto>> CreateItem([FromBody] CreateItemDto dto)
    {
        var userId = GetUserId();
        var command = new CreateItemCommand(dto, userId);
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetItemById), new { id = result.Id }, result);
    }

    // GET: api/items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItems()
    {
        var query = new GetItemsQuery();
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    // GET: api/items/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemDetailDto>> GetItemById(Guid id, [FromQuery] bool includeClaims = false)
    {
        var isAdmin = User.IsInRole("Administrator");
        var query = new GetItemByIdQuery(id, includeClaims && isAdmin);
        var result = await _mediator.Send(query);

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
