using System.Linq.Expressions;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Responses;
using BankingAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Domain.Interfaces;

public interface IAccountService
{
    Task<AccountDto?> GetAccountByIdAsync(Guid id);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task<bool> DeleteAccountAsync(Guid id);
    Task<UpdateBalanceResponse> IncreaseAccountBalanceAsync(Guid id, UpdateBalanceRequest request);
    Task<UpdateBalanceResponse> DecreaseAccountBalanceAsync(Guid id, UpdateBalanceRequest request);
    Task<UpdateActivityResponse> UpdateAccountStatusAsync(Guid id, UpdateActivityRequest request);
    Task AddAccountAsync(Account account);
}