using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Application.DTOs.Claim;

public class CreateClaimDto
{
    [Required]
    public Guid ItemId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string FeatureDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string LocationLost { get; set; } = string.Empty;

    [Required]
    public DateTime TimeLost { get; set; }
}
