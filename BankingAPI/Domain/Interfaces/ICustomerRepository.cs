using System.Linq.Expressions;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Domain.Interfaces;

public interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);
    Task<Customer?> GetByIdExcludingDeletedAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByEmailAsync(string email);
    Task DeleteAsync(Customer customer);
    Task SaveChangesAsync();


      Task<Customer?> GetByIdIncludingDeletedAsync(Guid id);

    Task<IEnumerable<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>> filter);
}