using BankingAPI.Domain.Enums;

namespace BankingAPI.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public PaymentType PaymentType { get; set; }
        public Guid? AccountId { get; set; }
        public Guid CustomerId { get; set; }
        public string CardNumber { get; set; }
        public CardType CardType { get; set; } 
        public string SenderIbanNumber { get; set; }
        public string ReceiverIbanNumber { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid? CardId { get; set; }

        // Navigasyon özellikleri
        public Account? Account { get; set; }
        public Customer? Customer { get; set; }
        public Card? Card { get; set; }

        public Transaction() {}

        public Transaction(PaymentType paymentType, Guid? accountId, Guid customerId, string cardNumber, string senderIbanNumber, string receiverIbanNumber, decimal amount, string description, Guid? cardId)
        {
            PaymentType = paymentType;
            AccountId = accountId;
            CustomerId = customerId;
            CardNumber = cardNumber;
            SenderIbanNumber = senderIbanNumber;
            ReceiverIbanNumber = receiverIbanNumber;
            Amount = amount;
            Description = description;
            CardId = cardId;
        }
    }
}