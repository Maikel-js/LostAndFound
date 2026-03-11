using MediatR;
using Microsoft.AspNetCore.Mvc;
using LostAndFound.Application.DTOs.Item;
using LostAndFound.Application.Features.Items.Commands.CreateItem;
using LostAndFound.Application.Features.Items.Queries.GetItems;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize] // 🔒 Requiere token JWT válido
    public async Task<ActionResult<ItemResponseDto>> CreateItem([FromBody] CreateItemDto dto)
    {
        var command = new CreateItemCommand(dto);
        var result = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetItems), result);
    }

    // GET: api/items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItems()
    {
        var query = new GetItemsQuery();
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }
}
