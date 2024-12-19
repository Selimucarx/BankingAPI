namespace BankingAPI.Application.DTOs;

public record TokenDto(string? Token, DateTime? Expiration);