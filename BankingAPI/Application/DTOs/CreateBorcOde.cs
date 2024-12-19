using BankingAPI.Domain.Enums;

namespace BankingAPI.Application.DTOs;

public record CreateBorcOde(
    Guid AccountId,
    BankName BankName,
    string? CardNumber,
    decimal? Amount
);