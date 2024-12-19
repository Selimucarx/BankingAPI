using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Domain.Interfaces;

public interface ICardService
{
    Task<CardDto> CreateCardAsync(CreateCardRequest request);

      Task MakeCardPurchaseAsync(PurchaseRequest request);

      Task HandleDebitCardPurchaseAsync(Card card, PurchaseRequest request);


      Task HandleCreditCardPurchaseAsync(Card card, PurchaseRequest request);
}