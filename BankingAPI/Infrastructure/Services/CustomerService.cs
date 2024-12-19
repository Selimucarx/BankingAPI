using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace BankingAPI.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _jwtTokenGenerator = _jwtTokenGenerator;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }
    
    
    public async Task<TokenDto> LoginCustomerAsync(string email, string password)
    {
        var customer = await _customerRepository.GetByEmailAsync(email);

        if (customer == null || customer.Password != password)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString())
        };

        var token = _jwtTokenGenerator.GenerateToken(claims);

        return new TokenDto
        {
            Token = token.Token,
            Expiration = token.Expiration
        };
    }
}