using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Enums;

namespace BankingAPI.Application.Requests;

public record Puschadebit(
    Guid CustomerId, 
    Guid AccountId,
    CardType CardType,
    string CardNumber,
    string Cvv,
    bool IsActive,
    BankName BankName,
    decimal Amount 
);
