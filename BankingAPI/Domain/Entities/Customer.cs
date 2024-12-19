using BankingAPI.Domain.Enums;

namespace BankingAPI.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Role Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string NationalNumber { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public decimal RiskLimit { get; set; }
        
        public decimal TotalCardLimit => Cards?.Sum(card => card.LimitAmount) ?? 0;


        // Navigasyon özellikleri
        public IEnumerable<Card>? Cards { get; set; }
        
        public ICollection<Account> Accounts { get; set; } // Add this line

        public IEnumerable<Transaction>? Transactions { get; set; }

        // Parametre içermeyen bir yapıcı ekleyin
        public Customer() {}

        // Eğer gerekliyse, parametreli bir yapıcı ekleyebilirsiniz ancak EF Core'un bunu kullanmasına gerek yok
        public Customer(Role role, string email, string password, string fullName, string nationalNumber, string placeOfBirth, DateOnly dateOfBirth, decimal riskLimit)
        {
            Role = role;
            Email = email;
            Password = password;
            FullName = fullName;
            NationalNumber = nationalNumber;
            PlaceOfBirth = placeOfBirth;
            DateOfBirth = dateOfBirth;
            RiskLimit = riskLimit;
        }
    }
}