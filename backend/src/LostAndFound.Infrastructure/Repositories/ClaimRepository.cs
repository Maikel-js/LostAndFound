using LostAndFound.Application.Interfaces;
using LostAndFound.Domain.Entities;
using LostAndFound.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories;

public class ClaimRepository : IClaimRepository
{
    private readonly ApplicationDbContext _context;

    public ClaimRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Claim?> GetByIdAsync(Guid id)
    {
        return await _context.Claims
            .Include(c => c.Item)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Claim>> GetAllAsync()
    {
        return await _context.Claims
            .Include(c => c.Item)
            .Include(c => c.User)
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> GetByItemIdAsync(Guid itemId)
    {
        return await _context.Claims
            .Include(c => c.Item)
            .Include(c => c.User)
            .Where(c => c.ItemId == itemId)
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Claims
            .Include(c => c.Item)
            .Include(c => c.User)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.DateSubmitted)
            .ToListAsync();
    }

    public async Task<Claim> AddAsync(Claim claim)
    {
        await _context.Claims.AddAsync(claim);
        await _context.SaveChangesAsync();
        return claim;
    }

    public async Task UpdateAsync(Claim claim)
    {
        _context.Claims.Update(claim);
        await _context.SaveChangesAsync();
    }
}
