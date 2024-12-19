using System.Linq.Expressions;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPatch("{id}/balance/increase")]
    [Authorize]
    public async Task<ActionResult<AccountDto>> IncreaseBalance(Guid id, [FromBody] UpdateBalanceRequest request)
    {
        var updatedAccount = await _accountService.IncreaseAccountBalanceAsync(id, request);
        return Ok(updatedAccount);
    }

    [HttpPatch("{id}/balance/decrease")]
    [Authorize]
    public async Task<ActionResult<AccountDto>> DecreaseBalance(Guid id, [FromBody] UpdateBalanceRequest request)
    {
        var updatedAccount = await _accountService.DecreaseAccountBalanceAsync(id, request);
        return Ok(updatedAccount);
    }

    [HttpPatch("{id}/status")]
    [Authorize]
    public async Task<ActionResult<AccountDto>> UpdateAccountStatus(Guid id, [FromBody] UpdateActivityRequest request)
    {
        var updatedAccount = await _accountService.UpdateAccountStatusAsync(id, request);
        return Ok(updatedAccount);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccountById(Guid id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
    {
        var accounts = await _accountService.GetAllAccountsAsync();
        return Ok(accounts);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        var result = await _accountService.DeleteAccountAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
