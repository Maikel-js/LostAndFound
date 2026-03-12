using MediatR;
using LostAndFound.Application.Interfaces;
using LostAndFound.Application.DTOs.Item;

namespace LostAndFound.Application.Features.Items.Queries.GetItems;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<ItemResponseDto>>
{
    private readonly IItemRepository _itemRepository;

    public GetItemsQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<IEnumerable<ItemResponseDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetAllAsync();

        return items.Select(item => new ItemResponseDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category.ToString(),
            LocationFound = item.LocationFound,
            DateFound = item.DateFound,
            ImageUrl = item.ImageUrl,
            Status = item.Status,
            ReporterId = item.ReporterId
        });
    }
}
