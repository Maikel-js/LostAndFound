using MediatR;
using LostAndFound.Application.DTOs.Claim;

namespace LostAndFound.Application.Features.Claims.Commands.RejectClaim;

public class RejectClaimCommand : IRequest<ClaimResponseDto?>
{
    public Guid ClaimId { get; }
    public Guid AdminId { get; }

    public RejectClaimCommand(Guid claimId, Guid adminId)
    {
        ClaimId = claimId;
        AdminId = adminId;
    }
}
