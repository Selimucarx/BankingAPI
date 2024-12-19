using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;

namespace BankingAPI.Domain.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> RegisterCustomerAsync(CreateCustomerRequest request);
    Task<TokenDto?> AuthenticateCustomerAsync(LoginRequest loginRequest);
    Task<CustomerDto?> GetCustomerDetailsByIdAsync(Guid customerId);
    Task<bool> ChangeCustomerPasswordAsync(Guid customerId, CustomerPasswordChangeRequest request);
    Task<CustomerDto> UpdateCustomerDetailsAsync(Guid customerId, UpdateCustomerRequest request);
    Task<IEnumerable<CustomerDto>> RetrieveAllCustomersAsync();
    Task PermanentlyDeleteCustomerAsync(Guid customerId);
    Task DeactivateCustomerAsync(Guid customerId);
    Task LogoutCustomerAsync(string token);

}