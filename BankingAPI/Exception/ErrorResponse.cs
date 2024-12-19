namespace BankingAPI.Exception;

public class ErrorResponse
{
    public string Status { get; set; } // Hata durumunu (örneğin, "error")
    public string Message { get; set; } // Kısa hata mesajı
    public string Code { get; set; } // Hata kodu (örneğin, "ERR_CUSTOMER_NOT_FOUND")
    public DateTime Timestamp { get; set; } // Hata zaman damgası
    public string Path { get; set; } // Hata oluşan endpoint yolu
    public ErrorDetails ErrorDetails { get; set; } // Hata detayları (isteğe bağlı)
    public string DeveloperMessage { get; set; } // Geliştiriciye özel mesaj
    public string UserMessage { get; set; } // Kullanıcıya özel mesaj
}