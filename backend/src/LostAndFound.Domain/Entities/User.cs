using LostAndFound.Domain.Common;
using LostAndFound.Domain.Enums;

namespace LostAndFound.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string EnrollmentNumber { get; set; } = string.Empty; // Matrícula
    public UserRole Role { get; set; } = UserRole.Student;

    // Relación: Un usuario puede reportar muchos objetos
    public ICollection<Item> ReportedItems { get; set; } = new List<Item>();

    // Relación: Un usuario puede crear muchos reclamos
    public ICollection<Claim> Claims { get; set; } = new List<Claim>();
}
