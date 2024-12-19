using System.Linq.Expressions;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Domain.Interfaces;

public interface IAccountRepository
{
    Task AddAsync(Account account);
    Task<Account?> GetByIdAsync(Guid id);
    Task<IEnumerable<Account>> GetAllAsync();
    Task DeleteAsync(Account account);

    Task SaveChangesAsync(Account account);

    Task<Account?> GetByExpressionAsync(Expression<Func<Account, bool>> predicate);
    Task SaveChangesAsync();

    Task<bool> ExistsAsync(Guid? accountId);

    Task UpdateAsync(Account account);
}