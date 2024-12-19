using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Enums;

namespace BankingAPI.Application.Requests
{
    public class PuschaseCredit
    {
        public Guid CustomerId { get; init; }
        public CardType CardType { get; init; }
        public string CardNumber { get; init; }
        public string ExpirationDate { get; init; }
        public string Cvv { get; init; }
        public decimal LimitAmount { get; init; }
        
        public decimal Amount { get; init; }
    
        // Burada AvailableLimit özelliğini normal bir özellik olarak tanımlıyoruz.
        public decimal AvailableLimit { get; set; }  // Bu, değiştirilebilir hale gelir.

        public bool IsActive { get; init; }
        public BankName BankName { get; init; }
    }

}