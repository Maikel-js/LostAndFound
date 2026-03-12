using MediatR;
using LostAndFound.Domain.Entities;
using LostAndFound.Domain.Enums;
using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Application.Interfaces;

namespace LostAndFound.Application.Features.Claims.Commands.CreateClaim;

public class CreateClaimCommandHandler : IRequestHandler<CreateClaimCommand, ClaimResponseDto>
{
    private readonly IClaimRepository _claimRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUserRepository _userRepository;

    public CreateClaimCommandHandler(IClaimRepository claimRepository, IItemRepository itemRepository, IUserRepository userRepository)
    {
        _claimRepository = claimRepository;
        _itemRepository = itemRepository;
        _userRepository = userRepository;
    }

    public async Task<ClaimResponseDto> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Dto.FeatureDescription) ||
            string.IsNullOrWhiteSpace(request.Dto.LocationLost))
        {
            throw new ArgumentException("Datos inválidos para el reclamo.");
        }

        var item = await _itemRepository.GetByIdAsync(request.Dto.ItemId);
        if (item == null)
            throw new InvalidOperationException("Item no encontrado.");

        if (item.Status == ItemStatus.Returned)
            throw new InvalidOperationException("Este objeto ya fue reclamado.");

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new InvalidOperationException("Usuario inválido.");

        var claim = new Claim
        {
            ItemId = item.Id,
            UserId = user.Id,
            FeatureDescription = request.Dto.FeatureDescription,
            LocationLost = request.Dto.LocationLost,
            TimeLost = request.Dto.TimeLost,
            Status = ClaimStatus.Pending
        };

        var created = await _claimRepository.AddAsync(claim);

        return new ClaimResponseDto
        {
            Id = created.Id,
            ItemId = item.Id,
            ItemName = item.Name,
            UserId = user.Id,
            UserName = $"{user.FirstName} {user.LastName}".Trim(),
            UserEmail = user.Email,
            Status = created.Status,
            DateSubmitted = created.DateSubmitted,
            DateResolved = created.DateResolved,
            FeatureDescription = created.FeatureDescription,
            LocationLost = created.LocationLost,
            TimeLost = created.TimeLost
        };
    }
}
