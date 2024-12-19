using BankingAPI.Domain.Enums;

public interface IJwtService
{
    (string Token, DateTime Expiration) GenerateToken(string userId, Role role);
    bool IsTokenBlacklisted(string token);
    string? GetUserIdFromToken(string token);

    bool ValidateToken(string token);

    Task AddTokenToBlacklistAsync(string token);

}