using MediatR;
using LostAndFound.Application.DTOs.Claim;

namespace LostAndFound.Application.Features.Claims.Queries.GetClaimsByUser;

public class GetClaimsByUserQuery : IRequest<IEnumerable<ClaimResponseDto>>
{
    public Guid UserId { get; }

    public GetClaimsByUserQuery(Guid userId)
    {
        UserId = userId;
    }
}
