using LostAndFound.Domain.Common;
using LostAndFound.Domain.Enums;

namespace LostAndFound.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty; // Dónde se perdió o encontró
    public DateTime DateReported { get; set; } = DateTime.UtcNow;
    public string? ImageUrl { get; set; }
    public ItemStatus Status { get; set; } = ItemStatus.Lost;

    // Relación: El usuario que reportó el objeto
    public Guid ReporterId { get; set; }
    public User Reporter { get; set; } = null!;
}
