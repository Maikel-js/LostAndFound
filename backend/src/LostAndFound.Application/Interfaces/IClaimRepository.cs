using LostAndFound.Domain.Entities;

namespace LostAndFound.Application.Interfaces;

public interface IClaimRepository
{
    Task<Claim?> GetByIdAsync(Guid id);
    Task<IEnumerable<Claim>> GetAllAsync();
    Task<IEnumerable<Claim>> GetByItemIdAsync(Guid itemId);
    Task<IEnumerable<Claim>> GetByUserIdAsync(Guid userId);
    Task<Claim> AddAsync(Claim claim);
    Task UpdateAsync(Claim claim);
}
