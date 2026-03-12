using LostAndFound.Domain.Enums;

namespace LostAndFound.Application.DTOs.Claim;

public class ClaimResponseDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public ClaimStatus Status { get; set; }
    public DateTime DateSubmitted { get; set; }
    public DateTime? DateResolved { get; set; }
    public string FeatureDescription { get; set; } = string.Empty;
    public string LocationLost { get; set; } = string.Empty;
    public DateTime TimeLost { get; set; }
}
