namespace BankingAPI.Application.Requests;

public record CustomerCreateRequest(
    string? FullName,
    string? Email,
    string? Password,
    string? NationalNumber,
    string? PlaceOfBirth,
    DateOnly? DateOfBirth,
    string? Iban,
    string? AccountName,
    string? AccountNumber
);