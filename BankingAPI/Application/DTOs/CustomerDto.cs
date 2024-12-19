using BankingAPI.Domain.Enums;

namespace BankingAPI.Application.DTOs;

public record CustomerDto(
    Guid Id,
    string? FullName,
    string? Email,
    string? Password,
    string? NationalNumber,
    string? PlaceOfBirth,
    DateOnly? DateOfBirth,
    decimal? RiskLimit,
    Role? Role
);