using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Responses;

namespace BankingAPI.Domain.Interfaces;

public interface ITransactionService
{
    Task<CreateTransactionResponse> TransferMoneyAsync(CreateTransactionRequest request);


    Task<CreateTransactionResponse> TransferBorc(CreateBorcOde createBorcOde);
}