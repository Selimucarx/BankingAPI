using System.Linq.Expressions;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public async Task<Customer?> GetByIdExcludingDeletedAsync(Guid id)
    {
        return await _context.Customers.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }
    
    public async Task<Customer?> GetByIdIncludingDeletedAsync(Guid id)
    {
        return await _context.Customers.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.Where(a => !a.IsDeleted).ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task DeleteAsync(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");

        _context.Customers.Remove(customer);
    }


    public async Task SaveChangesAsync()
    {
        // Güncellenmiş varlıkları kontrol et ve UpdatedAt değerini güncelle
        var updatedEntities = _context.ChangeTracker.Entries<Customer>()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in updatedEntities) entry.Entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync(Expression<Func<Customer, bool>> filter)
    {
        return await _context.Customers.Where(filter).ToListAsync();
    }
}