using MediatR;
using LostAndFound.Domain.Enums;
using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Application.Interfaces;

namespace LostAndFound.Application.Features.Claims.Commands.RejectClaim;

public class RejectClaimCommandHandler : IRequestHandler<RejectClaimCommand, ClaimResponseDto?>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUserRepository _userRepository;

    public RejectClaimCommandHandler(IClaimRepository claimRepository, IItemRepository itemRepository, IUserRepository userRepository)
    {
        _claimRepository = claimRepository;
        _itemRepository = itemRepository;
        _userRepository = userRepository;
    }

    public async Task<ClaimResponseDto?> Handle(RejectClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = await _claimRepository.GetByIdAsync(request.ClaimId);
        if (claim == null)
            return null;

        var item = await _itemRepository.GetByIdAsync(claim.ItemId);
        if (item == null)
            return null;

        var user = await _userRepository.GetByIdAsync(claim.UserId);
        if (user == null)
            return null;

        claim.Status = ClaimStatus.Rejected;
        claim.DateResolved = DateTime.UtcNow;
        claim.ResolvedById = request.AdminId;
        await _claimRepository.UpdateAsync(claim);

        return new ClaimResponseDto
        {
            Id = claim.Id,
            ItemId = item.Id,
            ItemName = item.Name,
            UserId = user.Id,
            UserName = $"{user.FirstName} {user.LastName}".Trim(),
            UserEmail = user.Email,
            Status = claim.Status,
            DateSubmitted = claim.DateSubmitted,
            DateResolved = claim.DateResolved,
            FeatureDescription = claim.FeatureDescription,
            LocationLost = claim.LocationLost,
            TimeLost = claim.TimeLost
        };
    }
}
