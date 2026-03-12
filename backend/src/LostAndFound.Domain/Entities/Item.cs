using LostAndFound.Domain.Common;
using LostAndFound.Domain.Enums;

namespace LostAndFound.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ItemCategory Category { get; set; } = ItemCategory.Other;
    public string LocationFound { get; set; } = string.Empty; // Dónde se encontró
    public DateTime DateFound { get; set; } = DateTime.UtcNow;
    public string? ImageUrl { get; set; }
    public ItemStatus Status { get; set; } = ItemStatus.Found;

    // Relación: El usuario que reportó el objeto
    public Guid ReporterId { get; set; }
    public User Reporter { get; set; } = null!;

    // Relación: Reclamos del objeto
    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}
