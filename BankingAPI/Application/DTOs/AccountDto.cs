namespace BankingAPI.Application.DTOs;

public record AccountDto(
    Guid Id,
    string? AccountName,
    string? AccountNumber,
    string? Iban,
    decimal? Balance,
    bool? IsActive
);