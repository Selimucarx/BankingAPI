namespace BankingAPI.Domain.Entities;

public class Account(
    string accountName,
    string accountNumber,
    string iban,
    decimal balance,
    bool isActive
) : BaseEntity
{
    public string? AccountName { get; set; } = accountName;
    public string? AccountNumber { get; set; } = accountNumber;
    public Guid? CustomerId { get; set; }
    public string? Iban { get; set; } = iban;
    public decimal Balance { get; set; } = balance;
    public bool IsActive { get; set; } = isActive;

    public Customer? Customer { get; set; }
    public ICollection<Card>? Cards { get; set; } = new List<Card>();
    public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();
}