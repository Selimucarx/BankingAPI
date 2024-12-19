using BankingAPI.Domain.Entities;

namespace BankingAPI.Domain.Interfaces;

public interface ITransactionRepository
{
    Task AddTransactionAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<List<Transaction>> GetAllAsync();
    Task DeleteAsync(Transaction transaction);
    Task SaveChangesAsync();
}