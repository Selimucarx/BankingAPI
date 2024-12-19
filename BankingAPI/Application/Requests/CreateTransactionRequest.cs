namespace BankingAPI.Application.DTOs;

public class CreateTransactionRequest
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string SenderIban { get; set; }
    public string ReceiverIban { get; set; }
}
