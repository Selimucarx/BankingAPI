using BankingAPI.Domain.Enums;

namespace BankingAPI.Exception;

public static class ErrorMessageTypeExtensions
{
    public static string GetMessage(this ErrorMessageType errorMessageType, string language = "tr")
    {
        return (errorMessageType, language) switch
        {
            (ErrorMessageType.NotFound, "tr") => "Customer bulunamadi",
            (ErrorMessageType.NotFound, "en") => "Customer Not found",
            (ErrorMessageType.GenericError, "tr") => "Sistem kaynaklı bir sorun",
            (ErrorMessageType.GenericError, "en") => "A system-related problem occurred",
            (ErrorMessageType.InvalidToken, "tr") => "JWT Token yanlış",
            (ErrorMessageType.InvalidToken, "en") => "Invalid JWT Token",
            (ErrorMessageType.EmailAlreadyExists, "tr") => "Bu Email adresi zaten kayıtlı!",
            (ErrorMessageType.EmailAlreadyExists, "en") => "This email address is already registered!",
            (ErrorMessageType.InvalidCredentials, "tr") => "Kullanıcı adı ya da şifreniz yanlış",
            (ErrorMessageType.InvalidCredentials, "en") => "Your username or password is incorrect",
            _ => language == "tr" ? "Bilinmeyen bir hata oluştu" : "An unknown error occurred"
        };
    }
}