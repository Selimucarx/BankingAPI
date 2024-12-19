namespace BankingAPI.Application.Responses;

public record CustomerCreateResponse(Guid Id,string? Message, bool? Success);