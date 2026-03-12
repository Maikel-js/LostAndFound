using LostAndFound.Application.DTOs.Claim;
using LostAndFound.Domain.Enums;

namespace LostAndFound.Application.DTOs.Item;

public class ItemDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "Other";
    public string LocationFound { get; set; } = string.Empty;
    public DateTime DateFound { get; set; }
    public string? ImageUrl { get; set; }
    public ItemStatus Status { get; set; }
    public Guid ReporterId { get; set; }
    public IEnumerable<ClaimResponseDto> Claims { get; set; } = [];
}
