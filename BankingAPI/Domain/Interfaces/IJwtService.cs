using System.Security.Claims;
using BankingAPI.Application.DTOs;

public interface IJwtService
{
      string GenerateToken(string email, string role);
}


