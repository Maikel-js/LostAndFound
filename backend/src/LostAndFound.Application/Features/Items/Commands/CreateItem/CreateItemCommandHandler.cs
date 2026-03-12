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
        if (string.IsNullOrWhiteSpace(request.Dto.Name) ||
            string.IsNullOrWhiteSpace(request.Dto.Description) ||
            string.IsNullOrWhiteSpace(request.Dto.Category) ||
            string.IsNullOrWhiteSpace(request.Dto.LocationFound))
        {
            throw new ArgumentException("Datos inválidos para crear el objeto.");
        }

        var item = new Item
        {
            Name = request.Dto.Name,
            Description = request.Dto.Description,
            Category = Enum.TryParse<ItemCategory>(request.Dto.Category, true, out var category)
                ? category
                : ItemCategory.Other,
            LocationFound = request.Dto.LocationFound,
            DateFound = request.Dto.DateFound,
            ImageUrl = request.Dto.ImageUrl,
            ReporterId = request.ReporterId,
            Status = ItemStatus.Found
        };

        var createdItem = await _itemRepository.AddAsync(item);

        return new ItemResponseDto
        {
            Id = createdItem.Id,
            Name = createdItem.Name,
            Description = createdItem.Description,
            Category = createdItem.Category.ToString(),
            LocationFound = createdItem.LocationFound,
            DateFound = createdItem.DateFound,
            ImageUrl = createdItem.ImageUrl,
            Status = createdItem.Status,
            ReporterId = createdItem.ReporterId
        };
    }
}
