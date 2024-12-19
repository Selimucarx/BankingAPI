namespace BankingAPI.Application.DTOs;

public record CustomerPasswordChangeRequest(string CurrentPassword, string NewPassword);