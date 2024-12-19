using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.DTOs;

public record CardDto(
    Guid Id,
    Guid? CustomerId,
    CardType? CardType, // Enum field
    string? CardNumber,
    string? ExpirationDate,
    string? Cvv,
    decimal? LimitAmount,
    decimal? AvailableLimit
);