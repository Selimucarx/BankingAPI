using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task AddTransactionAsync(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }


    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions.Where(a => !a.IsDeleted).ToListAsync();
    }


    public async Task DeleteAsync(Transaction transaction)
    {
        _context.Transactions.Remove(transaction);
    }


    public async Task SaveChangesAsync()
    {
        // Güncellenmiş varlıkları kontrol et ve UpdatedAt değerini güncelle
        var updatedEntities = _context.ChangeTracker.Entries<Transaction>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in updatedEntities) entry.Entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}