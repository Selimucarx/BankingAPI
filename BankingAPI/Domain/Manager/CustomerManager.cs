using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Validators;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Enums;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Exception;
using BankingAPI.Infrastructure.Services;

namespace BankingAPI.Domain.Manager;

public class CustomerManager
{
    private readonly IAccountService accountService;
    private readonly ICustomerRepository customerRepository;
    private readonly IJwtService jwtService;
    private readonly ILogger<CustomerManager> logger;
    private readonly IMapper mapper;
    private readonly PasswordHasher passwordHasher;

    public CustomerManager(
        ICustomerRepository customerRepository,
        IAccountService accountService,
        IMapper mapper,
        IJwtService jwtService,
        PasswordHasher passwordHasher,
        ILogger<CustomerManager> logger)
    {
        this.customerRepository = customerRepository;
        this.accountService = accountService;
        this.mapper = mapper;
        this.jwtService = jwtService;
        this.passwordHasher = passwordHasher;
        this.logger = logger;
    }

    public async Task<CustomerDto> RegisterCustomerAsync(CreateCustomerRequest request)
    {
        var validator = new CustomerValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid) throw new ValidationException("Invalid customer request.");

        var customer = mapper.Map<Customer>(request);
        customer.Password = passwordHasher.HashPassword(request.Password);

        var account = AccountFactory.CreateDefaultAccount(request, customer);
        account.Customer = customer;

        await customerRepository.AddCustomerAsync(customer);
        await accountService.AddAccountAsync(account);

        await customerRepository.SaveChangesAsync();

        return mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto?> GetCustomerDetailsByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdExcludingDeletedAsync(id);
        if (customer == null)
            throw new CustomException(
                "Customer not found.",
                "ERR_CUSTOMER_NOT_FOUND",
                "The customer with the provided ID does not exist.",
                "Please verify the information and try again."
            );

        return mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> ChangeCustomerPasswordAsync(Guid customerId, CustomerPasswordChangeRequest request)
    {
        // Validate the input request
        var validator = new CustomerPasswordChangeValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed: " +
                                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        // Fetch customer from the repository
        var customer = await customerRepository.GetByIdExcludingDeletedAsync(customerId);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer with the provided ID not found.");
        }

        // Verify current password
        if (!passwordHasher.VerifyPassword(request.CurrentPassword, customer.Password))
        {
            throw new UnauthorizedAccessException("Current password is incorrect.");
        }

        // Update password and save changes
        customer.Password = passwordHasher.HashPassword(request.NewPassword);
        await customerRepository.SaveChangesAsync();

        return true;
    }


    public async Task<CustomerDto> UpdateCustomerDetailsAsync(Guid customerId, UpdateCustomerRequest request)
    {
        var customer = await customerRepository.GetByIdExcludingDeletedAsync(customerId);
        if (customer == null) return null;

        customer.Email = request.Email;
        customer.FullName = request.FullName;

        await customerRepository.SaveChangesAsync();

        return mapper.Map<CustomerDto>(customer);
    }

    public async Task<IEnumerable<CustomerDto>> RetrieveAllCustomersAsync()
    {
        var customers = await customerRepository.GetAllAsync();
        return mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        try
        {
            return await customerRepository.GetByEmailAsync(email);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching customer by email: {Email}", email);
            throw;
        }
    }
    
    public async Task PermanentlyDeleteCustomerAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdIncludingDeletedAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found.");
        }

        // Perform the actual deletion and await it
        await customerRepository.DeleteAsync(customer);
    
        // Ensure changes are saved after deletion
        await customerRepository.SaveChangesAsync();
    }


    // Soft delete: Mark the customer as deleted without removing from the database
    public async Task DeactivateCustomerAsync(Guid id)
    {
        var customer = await customerRepository.GetByIdExcludingDeletedAsync(id);
        if (customer == null)
        {
            throw new KeyNotFoundException("Customer not found.");
        }

        // Mark the customer as deleted (soft delete)
        customer.IsDeleted = true;  // Set IsDeleted to true for soft delete

        await customerRepository.SaveChangesAsync();  // Persist the changes
    }

    // Şifreyi doğrulama
    public bool VerifyPassword(string providedPassword, string storedPasswordHash)
    {
        return passwordHasher.VerifyPassword(providedPassword, storedPasswordHash);
    }

    // Login işlemi - Token oluşturma ve kullanıcı doğrulama
    public async Task<TokenDto?> AuthenticateCustomerAsync(string email, string password)
    {
        // Müşteri bilgilerini email ile sorgula
        var customer = await GetCustomerByEmailAsync(email);
        if (customer == null || !VerifyPassword(password, customer.Password))
            return null;

        // Token oluştur ve döndür
        var (token, expiration) = jwtService.GenerateToken(customer.Email, Role.Customer);
        return new TokenDto(token, expiration);
    }
}