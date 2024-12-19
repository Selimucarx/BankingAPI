namespace BankingAPI.Application.Requests;

public record UpdateBalanceRequest(Guid Id, decimal? Balance);