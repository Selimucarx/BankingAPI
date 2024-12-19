namespace BankingAPI.Domain.Enums;

public enum ErrorMessageType
{
    GenericError,
    InvalidToken,
    EmailAlreadyExists,
    InvalidCredentials,
    NotFound,
    Unauthorized
}