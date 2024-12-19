namespace BankingAPI.Domain.Entities;

public enum ErrorMessageType
{
    GenericError,
    InvalidToken,
    EmailAlreadyExists,
    InvalidCredentials,
    NotFound,
    Unauthorized
}