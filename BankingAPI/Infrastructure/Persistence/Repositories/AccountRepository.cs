using System.Linq.Expressions;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        // Sadece soft delete edilmemiş hesapları döndürüyoruz
        return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.IsActive && !a.IsDeleted);
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        // Soft delete edilmemiş tüm hesapları liste olarak döndürüyoruz
        return await _context.Accounts.Where(a => !a.IsActive).ToListAsync();
    }


    public async Task<Account?> GetByExpressionAsync(Expression<Func<Account, bool>> predicate)
    {
        return await _context.Accounts.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid? accountId)
    {
        return await _context.Accounts
            .AnyAsync(account => account.Id == accountId);
    }


    public async Task DeleteAsync(Account account)
    {
        _context.Accounts.Remove(account);
    }

    public async Task SaveChangesAsync(Account account)
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        _context.Set<Account>().Update(account);
        await _context.SaveChangesAsync();
    }
}