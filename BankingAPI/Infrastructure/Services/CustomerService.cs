using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Domain.Manager;

namespace BankingAPI.Infrastructure.Services;

public class CustomerService(
    CustomerManager customerManager,
    IJwtService jwtService)
    : ICustomerService
{
    public async Task<CustomerDto> RegisterCustomerAsync(CreateCustomerRequest request)
    {
        return await customerManager.RegisterCustomerAsync(request);
    }

    public async Task<TokenDto?> AuthenticateCustomerAsync(LoginRequest loginRequest)
    {
        return await customerManager.AuthenticateCustomerAsync(loginRequest.Email, loginRequest.Password);
    }

    public async Task<CustomerDto?> GetCustomerDetailsByIdAsync(Guid customerId)
    {
        return await customerManager.GetCustomerDetailsByIdAsync(customerId);
    }

    public async Task<bool> ChangeCustomerPasswordAsync(Guid customerId, CustomerPasswordChangeRequest request)
    {
        return await customerManager.ChangeCustomerPasswordAsync(customerId, request);
    }

    public async Task<CustomerDto> UpdateCustomerDetailsAsync(Guid customerId, UpdateCustomerRequest request)
    {
        return await customerManager.UpdateCustomerDetailsAsync(customerId, request);
    }

    public async Task<IEnumerable<CustomerDto>> RetrieveAllCustomersAsync()
    {
        return await customerManager.RetrieveAllCustomersAsync();
    }

    public async Task PermanentlyDeleteCustomerAsync(Guid customerId)
    {
        await customerManager.PermanentlyDeleteCustomerAsync(customerId);
    }

    public async Task DeactivateCustomerAsync(Guid customerId)
    {
        await customerManager.DeactivateCustomerAsync(customerId);
    }

    public async Task LogoutCustomerAsync(string token)
    {
        await jwtService.AddTokenToBlacklistAsync(token); 
    }
}