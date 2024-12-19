namespace BankingAPI.Application.Requests;

public record UpdateCustomerRequest(string? Email, string? FullName);