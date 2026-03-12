namespace LostAndFound.Application.DTOs.Item;

public class CreateItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public Guid ReporterId { get; set; }
}
