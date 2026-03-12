using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Application.DTOs.Item;

public class CreateItemDto
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string Category { get; set; } = "Other";

    [Required]
    [MaxLength(200)]
    public string LocationFound { get; set; } = string.Empty;

    [Required]
    public DateTime DateFound { get; set; } = DateTime.UtcNow;
    public string? ImageUrl { get; set; }
}
