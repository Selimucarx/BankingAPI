namespace BankingAPI.Application.DTOs;

public record LoginCustomerDto(
    string? Email,
    string? Password);