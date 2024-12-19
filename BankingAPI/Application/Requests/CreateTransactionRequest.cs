namespace BankingAPI.Application.Requests;

public record CreateTransactionRequest(
    Guid AccountId,
    decimal? Amount,
    string? Description,
    string? SenderIban,
    string? ReceiverIban
);