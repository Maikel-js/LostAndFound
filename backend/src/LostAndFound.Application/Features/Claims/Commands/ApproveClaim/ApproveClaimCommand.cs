using MediatR;
using LostAndFound.Application.DTOs.Claim;

namespace LostAndFound.Application.Features.Claims.Commands.ApproveClaim;

public class ApproveClaimCommand : IRequest<ClaimResponseDto?>
{
    public Guid ClaimId { get; }
    public Guid AdminId { get; }

    public ApproveClaimCommand(Guid claimId, Guid adminId)
    {
        ClaimId = claimId;
        AdminId = adminId;
    }
}
