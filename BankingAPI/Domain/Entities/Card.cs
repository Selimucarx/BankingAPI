using BankingAPI.Domain.Enums;

namespace BankingAPI.Domain.Entities
{
    public class Card : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid AccountId { get; set; }
        public CardType? CardType { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public string? Cvv { get; set; }
        public decimal? LimitAmount { get; set; }
        public decimal? AvailableLimit { get; set; }
        public bool? IsActive { get; set; }
        public BankName? BankName { get; set; }

        // Navigasyon özellikleri
        public IEnumerable<Transaction>? Transactions { get; set; }
        public Account? Account { get; set; }
        public Customer? Customer { get; set; } 

        public Card(){}
        
        // Yapıcı sadece gerekli alanları kabul eder
        public Card(Guid customerId, Guid accountId, CardType? cardType, string? cardNumber,
            string? expirationDate, string? cvv, decimal? limitAmount, decimal? availableLimit,
                          bool? isActive, BankName? bankName)
        {
            CustomerId = customerId;
            AccountId = accountId;
            CardType = cardType;
            CardNumber = cardNumber;
            ExpirationDate = expirationDate;
            Cvv = cvv;
            LimitAmount = limitAmount;
            AvailableLimit = availableLimit;
            IsActive = isActive;
            BankName = bankName;
        }
    }
}