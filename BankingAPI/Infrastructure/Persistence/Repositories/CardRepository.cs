using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Infrastructure.Persistence.Repositories;

public class CardRepository : ICardRepository
{
    private readonly AppDbContext _context;

    public CardRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(Card card)
    {
        await _context.Cards.AddAsync(card);
    }


    public async Task<Card?> GetByIdAsync(Guid id)
    {
        return await _context.Cards.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }


    public async Task<IEnumerable<Card>> GetAllAsync()
    {
        return await _context.Cards.Where(a => !a.IsDeleted).ToListAsync();
    }


    public async Task DeleteAsync(Card card)
    {
        _context.Cards.Remove(card);
    }


    public async Task SaveChangesAsync()
    {
        // Güncellenmiş varlıkları kontrol et ve UpdatedAt değerini güncelle
        var updatedEntities = _context.ChangeTracker.Entries<Card>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in updatedEntities) entry.Entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }


    public async Task UpdateAsync(Card card)
    {
        _context.Set<Card>().Update(card);
        await _context.SaveChangesAsync();
    }
}