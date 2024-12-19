using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Responses;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionService> _logger;
    private readonly ITransactionService _transactionService;


    public TransactionController(ITransactionService transactionService, ILogger<TransactionService> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CreateTransactionResponse>> CreateTransaction(
        [FromBody] CreateTransactionRequest request)
    {
        try
        {
            var transactionResponse = await _transactionService.TransferMoneyAsync(request);
            return Ok(transactionResponse);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the transaction.");
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPost("borc")]
    public async Task<ActionResult<CreateTransactionResponse>> transferborc([FromBody] CreateBorcOde createBorcOde)
    {
        try
        {
            var transactionResponse = await _transactionService.TransferBorc(createBorcOde);
            return Ok(transactionResponse);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the transaction.");
            return StatusCode(500, "Internal server error");
        }
    }
}