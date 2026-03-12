using MediatR;
using LostAndFound.Domain.Enums;
using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Application.Interfaces;

namespace LostAndFound.Application.Features.Claims.Queries.GetClaims;

public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, IEnumerable<ClaimResponseDto>>
{
    private readonly IClaimRepository _claimRepository;

    public GetClaimsQueryHandler(IClaimRepository claimRepository)
    {
        _claimRepository = claimRepository;
    }

    public async Task<IEnumerable<ClaimResponseDto>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Claim> claims;

        if (request.ItemId.HasValue)
        {
            claims = await _claimRepository.GetByItemIdAsync(request.ItemId.Value);
        }
        else
        {
            claims = await _claimRepository.GetAllAsync();
        }

        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<ClaimStatus>(request.Status, true, out var status))
        {
            claims = claims.Where(c => c.Status == status);
        }

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
