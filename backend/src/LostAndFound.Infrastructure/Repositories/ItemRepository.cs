using LostAndFound.Application.Interfaces;
using LostAndFound.Domain.Entities;
using LostAndFound.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Item?> GetByIdAsync(Guid id)
    {
        return await _context.Items
            .Include(i => i.Reporter)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Item?> GetByIdAsync(Guid id, bool includeClaims)
    {
        var query = _context.Items.AsQueryable();

        if (includeClaims)
        {
            query = query
                .Include(i => i.Reporter)
                .Include(i => i.Claims)
                    .ThenInclude(c => c.User);
        }
        else
        {
            query = query.Include(i => i.Reporter);
        }

        return await query.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _context.Items
            .Include(i => i.Reporter)
            .ToListAsync();
    }

    public async Task<Item> AddAsync(Item item)
    {
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Item item)
    {
        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }
}
