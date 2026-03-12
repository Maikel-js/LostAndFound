using MediatR;
using LostAndFound.Application.DTOs.Item;
using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Application.Interfaces;

namespace LostAndFound.Application.Features.Items.Queries.GetItemById;

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDetailDto?>
{
    private readonly IItemRepository _itemRepository;

    public GetItemByIdQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ItemDetailDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _itemRepository.GetByIdAsync(request.ItemId, request.IncludeClaims);

        if (item == null)
            return null;

        var dto = new ItemDetailDto
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
        };

        if (request.IncludeClaims)
        {
            dto.Claims = item.Claims.Select(c => new ClaimResponseDto
            {
                Id = c.Id,
                ItemId = c.ItemId,
                ItemName = item.Name,
                UserId = c.UserId,
                UserName = $"{c.User.FirstName} {c.User.LastName}".Trim(),
                UserEmail = c.User.Email,
                Status = c.Status,
                DateSubmitted = c.DateSubmitted,
                DateResolved = c.DateResolved,
                FeatureDescription = c.FeatureDescription,
                LocationLost = c.LocationLost,
                TimeLost = c.TimeLost
            }).ToList();
        }

        return dto;
    }
}
