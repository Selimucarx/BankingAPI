namespace BankingAPI.Exception;

public class ErrorDetails
{
    public string Field { get; set; } // Hata alanı (isteğe bağlı)
    public string Reason { get; set; } // Hata nedeni (isteğe bağlı)
}