using System.Linq.Expressions;
using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Responses;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Infrastructure.Services;

public class AccountService(IAccountRepository accountRepository, IMapper mapper) : IAccountService
{
    public async Task<AccountDto?> GetAccountByIdAsync(Guid id)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account == null)
            return null;

        return mapper.Map<AccountDto>(account);
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await accountRepository.GetAllAsync();
        return accounts.Select(account => mapper.Map<AccountDto>(account)).ToList();
    }

    public async Task<bool> DeleteAccountAsync(Guid id)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account == null)
            return false;

        await accountRepository.DeleteAsync(account);
        await accountRepository.SaveChangesAsync();
        return true;
    }

    
    public async Task<UpdateBalanceResponse> IncreaseAccountBalanceAsync(Guid id, UpdateBalanceRequest request)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new ArgumentException("Account not found.");

        if (request.Balance.HasValue)
        {
            account.Balance += request.Balance.Value;
        }
        else
        {
            throw new InvalidOperationException("Balance value is null.");
        }

        await accountRepository.SaveChangesAsync();
        return new UpdateBalanceResponse(account.Balance);
    }

    public async Task<UpdateBalanceResponse> DecreaseAccountBalanceAsync(Guid id, UpdateBalanceRequest request)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new ArgumentException("Account not found.");

        if (request.Balance.HasValue)
        {
            account.Balance -= request.Balance.Value;
        }
        else
        {
            throw new InvalidOperationException("Balance value is null.");
        }

        await accountRepository.SaveChangesAsync();
        return new UpdateBalanceResponse(account.Balance);
    }

    public async Task<UpdateActivityResponse> UpdateAccountStatusAsync(Guid id, UpdateActivityRequest request)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new ArgumentException("Account not found.");

        account.IsActive = request.IsActive;
        await accountRepository.SaveChangesAsync();
        return new UpdateActivityResponse(account.IsActive);
    }

    public async Task AddAccountAsync(Account account)
    {
        await accountRepository.AddAsync(account);
        await accountRepository.SaveChangesAsync();
    }
}
