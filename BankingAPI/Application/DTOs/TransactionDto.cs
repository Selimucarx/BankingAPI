namespace BankingAPI.Application.DTOs;

public record TransactionDto(
    Guid CardId,
    decimal? Amount,
    string? Description,
    string? SenderIbanNumber,
    string? ReceiverIbanNumber
);