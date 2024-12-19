namespace BankingAPI.Application.DTOs;

public class CreateTransactionResponse
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid AccountId { get; set; }
    public Guid CustomerId { get; set; }
    public string? CardNumber { get; set; }
    public string SenderIbanNumber { get; set; }
    public string ReceiverIbanNumber { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public int PaymentType { get; set; } 
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
