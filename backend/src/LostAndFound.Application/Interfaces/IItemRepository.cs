using LostAndFound.Domain.Entities;

namespace LostAndFound.Application.Interfaces;

public interface IItemRepository
{
    Task<Item?> GetByIdAsync(Guid id);
    Task<Item?> GetByIdAsync(Guid id, bool includeClaims);
    Task<IEnumerable<Item>> GetAllAsync();
    Task<Item> AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(Item item);
}
