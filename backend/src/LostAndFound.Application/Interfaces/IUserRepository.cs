using LostAndFound.Domain.Entities;

namespace LostAndFound.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEnrollmentNumberAsync(string enrollmentNumber);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
}
