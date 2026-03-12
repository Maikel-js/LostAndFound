using LostAndFound.Domain.Enums;

namespace LostAndFound.Application.DTOs.User;

public class RegisterUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EnrollmentNumber { get; set; } = string.Empty;
}
