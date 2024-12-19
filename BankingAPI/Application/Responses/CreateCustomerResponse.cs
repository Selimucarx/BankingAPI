namespace BankingAPI.Application.Responses;

public record CreateCustomerResponse(Guid Id, string? Message, bool? Success);