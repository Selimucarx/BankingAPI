using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.Requests;

public record CreateCardRequest(
    Guid CustomerId,
    Guid? AccountId,
    CardType? CardType,
    string? CardNumber,
    string? ExpirationDate,
    string? Cvv,
    decimal? LimitAmount,
    decimal? AvailableLimit,
    bool? IsActive);