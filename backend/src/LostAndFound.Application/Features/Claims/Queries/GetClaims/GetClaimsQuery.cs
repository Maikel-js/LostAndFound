using MediatR;
using LostAndFound.Application.DTOs.Claim;

namespace LostAndFound.Application.Features.Claims.Queries.GetClaims;

public class GetClaimsQuery : IRequest<IEnumerable<ClaimResponseDto>>
{
    public Guid? ItemId { get; }
    public string? Status { get; }

    public GetClaimsQuery(Guid? itemId, string? status)
    {
        ItemId = itemId;
        Status = status;
    }
}
