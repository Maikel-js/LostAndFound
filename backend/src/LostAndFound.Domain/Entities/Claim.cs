using LostAndFound.Domain.Common;
using LostAndFound.Domain.Enums;

namespace LostAndFound.Domain.Entities;

public class Claim : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;
    public string FeatureDescription { get; set; } = string.Empty;
    public string LocationLost { get; set; } = string.Empty;
    public DateTime TimeLost { get; set; }
    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
    public DateTime? DateResolved { get; set; }
    public Guid? ResolvedById { get; set; }
}
