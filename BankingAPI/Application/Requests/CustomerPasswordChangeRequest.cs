namespace BankingAPI.Application.Requests;

public record CustomerPasswordChangeRequest(
    string? CurrentPassword,
    string? NewPassword);