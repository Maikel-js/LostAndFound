using MediatR;
using LostAndFound.Application.DTOs.Claim;

namespace LostAndFound.Application.Features.Claims.Commands.CreateClaim;

public class CreateClaimCommand : IRequest<ClaimResponseDto>
{
    public CreateClaimDto Dto { get; }
    public Guid UserId { get; }

    public CreateClaimCommand(CreateClaimDto dto, Guid userId)
    {
        Dto = dto;
        UserId = userId;
    }
}
