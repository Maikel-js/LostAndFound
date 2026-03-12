using MediatR;
using LostAndFound.Application.DTOs.Item;

namespace LostAndFound.Application.Features.Items.Queries.GetItemById;

public class GetItemByIdQuery : IRequest<ItemDetailDto?>
{
    public Guid ItemId { get; }
    public bool IncludeClaims { get; }

    public GetItemByIdQuery(Guid itemId, bool includeClaims)
    {
        ItemId = itemId;
        IncludeClaims = includeClaims;
    }
}
