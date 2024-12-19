using BankingAPI.Application.Requests;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Infrastructure.Services;

public class AccountFactory
{
    public static Account CreateDefaultAccount(CreateCustomerRequest createCustomerRequest, Customer customer)
    {
        // Primary constructor kullanımı
        var account = new Account(
            createCustomerRequest.AccountName,
            createCustomerRequest.AccountNumber,
            createCustomerRequest.Iban,
            1000.00m,
            true
        )
        {
            // Ek olarak set edilecek property
            CustomerId = customer.Id
        };

        return account;
    }
}