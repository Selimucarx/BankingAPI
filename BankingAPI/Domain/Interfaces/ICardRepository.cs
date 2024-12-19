using BankingAPI.Domain.Entities;

namespace BankingAPI.Domain.Interfaces;

public interface ICardRepository
{
    Task AddAsync(Card card);
    Task<Card?> GetByIdAsync(Guid id);

    Task<IEnumerable<Card>> GetAllAsync();

    Task DeleteAsync(Card card);

    Task SaveChangesAsync();

    Task UpdateAsync(Card card);
}