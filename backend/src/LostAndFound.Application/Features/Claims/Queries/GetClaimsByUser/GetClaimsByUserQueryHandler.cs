using MediatR;
using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Application.Interfaces;

namespace LostAndFound.Application.Features.Claims.Queries.GetClaimsByUser;

public class GetClaimsByUserQueryHandler : IRequestHandler<GetClaimsByUserQuery, IEnumerable<ClaimResponseDto>>
{
    private readonly IClaimRepository _claimRepository;

    public GetClaimsByUserQueryHandler(IClaimRepository claimRepository)
    {
        _claimRepository = claimRepository;
    }

    public async Task<IEnumerable<ClaimResponseDto>> Handle(GetClaimsByUserQuery request, CancellationToken cancellationToken)
    {
        var claims = await _claimRepository.GetByUserIdAsync(request.UserId);

        return claims.Select(c => new ClaimResponseDto
        {
            Id = c.Id,
            ItemId = c.ItemId,
            ItemName = c.Item.Name,
            UserId = c.UserId,
            UserName = $"{c.User.FirstName} {c.User.LastName}".Trim(),
            UserEmail = c.User.Email,
            Status = c.Status,
            DateSubmitted = c.DateSubmitted,
            DateResolved = c.DateResolved,
            FeatureDescription = c.FeatureDescription,
            LocationLost = c.LocationLost,
            TimeLost = c.TimeLost
        });
    }
}
