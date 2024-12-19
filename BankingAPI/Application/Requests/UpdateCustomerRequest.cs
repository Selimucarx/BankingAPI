namespace BankingAPI.Application.Requests;

public record CustomerUpdateRequest(string? Email, string? FullName);