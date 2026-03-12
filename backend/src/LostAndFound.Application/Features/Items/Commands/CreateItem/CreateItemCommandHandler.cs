using MediatR;
using LostAndFound.Domain.Entities;
using LostAndFound.Domain.Enums;
using LostAndFound.Application.Interfaces;
using LostAndFound.Application.DTOs.Item;

namespace LostAndFound.Application.Features.Items.Commands.CreateItem;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemResponseDto>
{
    private readonly IItemRepository _itemRepository;

    public CreateItemCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ItemResponseDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Name = request.Dto.Name,
            Description = request.Dto.Description,
            Location = request.Dto.Location,
            ImageUrl = request.Dto.ImageUrl,
            ReporterId = request.Dto.ReporterId,
            DateReported = DateTime.UtcNow,
            Status = ItemStatus.Lost
        };

        var createdItem = await _itemRepository.AddAsync(item);

        return new ItemResponseDto
        {
            Id = createdItem.Id,
            Name = createdItem.Name,
            Description = createdItem.Description,
            Location = createdItem.Location,
            ImageUrl = createdItem.ImageUrl,
            DateReported = createdItem.DateReported,
            Status = createdItem.Status,
            ReporterId = createdItem.ReporterId
        };
    }
}
