namespace BankingAPI.Application.Responses;

public record CreateTransactionResponse(
    Guid Id,
    Guid? CardId,
    Guid? AccountId,
    Guid? CustomerId,
    string? CardNumber,
    string? SenderIbanNumber,
    string? ReceiverIbanNumber,
    decimal? Amount,
    string? Description,
    int? PaymentType,
    DateTime? CreatedAt,
    bool? IsDeleted
);