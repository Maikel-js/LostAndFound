using MediatR;
using LostAndFound.Application.DTOs.Item;

namespace LostAndFound.Application.Features.Items.Commands.CreateItem;

public class CreateItemCommand : IRequest<ItemResponseDto>
{
    public CreateItemDto Dto { get; set; }

    public CreateItemCommand(CreateItemDto dto)
    {
        Dto = dto;
    }
}
