using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankingAPI.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BankingAPI.Infrastructure.Services;

public class JwtService : IJwtService
{
    private static readonly List<string> BlacklistedTokens = new();
    private readonly string _audience;
    private readonly string _issuer;
    private readonly string _secret;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _secret = jwtSettings.Value.Secret;
        _issuer = jwtSettings.Value.Issuer;
        _audience = jwtSettings.Value.Audience;
    }

    public (string Token, DateTime Expiration) GenerateToken(string userId, Role role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(30);
        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: expiration,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }


    public Task AddTokenToBlacklistAsync(string token)
    {
        if (!BlacklistedTokens.Contains(token))
        {
            BlacklistedTokens.Add(token);
        }
        return Task.CompletedTask;
    }

    public bool IsTokenBlacklisted(string token)
    {
        return BlacklistedTokens.Contains(token);
    }

    public string? GetUserIdFromToken(string token)
    {
        if (IsTokenBlacklisted(token)) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }

    public bool ValidateToken(string token)
    {
        // Check if the token is blacklisted first
        if (IsTokenBlacklisted(token))
            return false;  // Token is blacklisted, invalid.

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            }, out _);

            return true;  // Token is valid
        }
        catch
        {
            return false;  // Token validation failed
        }
    }

}

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}