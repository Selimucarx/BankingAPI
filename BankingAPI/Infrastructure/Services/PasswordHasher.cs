using System.Security.Cryptography;
using System.Text;

namespace BankingAPI.Infrastructure.Services;

public class PasswordHasher
{
    private readonly string _salt = "randomSalt"; // Güvenlik için sabit bir salt değeri

    // Hash the password using SHA256 and salt
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + _salt;
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToBase64String(hashBytes);
        }
    }

    // Verify if the password matches the stored hash
    public bool VerifyPassword(string inputPassword, string storedPassword)
    {
        var inputHash = HashPassword(inputPassword); // Hash the input password
        return inputHash == storedPassword; // Compare it to the stored password hash
    }
}