namespace BankingAPI.Application.Requests;

public record PurchaseRequest(
    Guid Id,
    decimal Amount
);