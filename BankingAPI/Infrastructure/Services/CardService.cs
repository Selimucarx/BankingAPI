using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Enums;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Infrastructure.Services;

public class CardService(
    ICardRepository cardRepository,
    IMapper mapper,
    IAccountRepository accountRepository,
    AppDbContext context,
    ILogger<TransactionService> logger)
    : ICardService
{

    public async Task<CardDto> CreateCardAsync(CreateCardRequest request)
    {
        var customer = await context.Customers
            .Include(c => c.Cards)
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId);

        if (customer == null)
        {
            throw new System.Exception("Customer not found.");
        }

        var newCardLimit = request.LimitAmount ?? 0;

        if (customer.TotalCardLimit + newCardLimit > customer.RiskLimit)
        {
            throw new System.Exception("Risk limit exceeded.");
        }

        // Kartı oluştur
        var card = mapper.Map<Card>(request);

        // Kartı repository'e ekleyin
        await cardRepository.AddAsync(card);

        // Değişiklikleri kaydedin
        await cardRepository.SaveChangesAsync();

        // DTO'ya dönüştürüp geri döndürün
        return mapper.Map<CardDto>(card);
    }


    public async Task MakeCardPurchaseAsync(PurchaseRequest request)
    {
        var card = await cardRepository.GetByIdAsync(request.Id);
        if (card == null) throw new InvalidOperationException("Card not found.");

        if (!card.IsActive ?? false) throw new InvalidOperationException("The card is not active.");

        // Switch case for card type
        switch (card.CardType)
        {
            case CardType.Debit:
                await HandleDebitCardPurchaseAsync(card, request);
                break;

            case CardType.Credit:
                await HandleCreditCardPurchaseAsync(card, request);
                break;

            default:
                throw new InvalidOperationException("Invalid card type.");
        }
    }

    public async Task HandleDebitCardPurchaseAsync(Card card, PurchaseRequest request)
    {
        logger.LogInformation($"Attempting to find account with AccountId: {card.AccountId}");

        // Ensure we only look for active accounts
        var account = await accountRepository.GetByIdAsync(card.AccountId);
        if (account == null)
        {
            logger.LogError($"Active account not found for CardId: {card.Id} with AccountId: {card.AccountId}");
            throw new InvalidOperationException("Account not found or is inactive.");
        }

        if (account.Balance < request.Amount) throw new InvalidOperationException("Insufficient balance.");

        // Deduct amount from the account's balance
        account.Balance -= request.Amount;

        var transaction = new Transaction(
            PaymentType.Purchase,
            account.Id,
            card.CustomerId,
            card.CardNumber,
            null,
            null,
            request.Amount,
            "Debit card purchase",
            card.Id
        );

        await context.Transactions.AddAsync(transaction);
        context.Accounts.Update(account);
        await context.SaveChangesAsync();

        logger.LogInformation("Debit card purchase completed successfully.");
    }

    public async Task HandleCreditCardPurchaseAsync(Card card, PurchaseRequest request)
    {
        if (card.AvailableLimit < request.Amount) throw new InvalidOperationException("Insufficient available limit.");

        // Deduct amount from the card's available limit
        card.AvailableLimit -= request.Amount;

        var transaction = new Transaction(
            PaymentType.Purchase,
            null,
            card.CustomerId,
            card.CardNumber,
            null,
            null,
            request.Amount,
            "Credit card purchase",
            card.Id
        );

        await context.Transactions.AddAsync(transaction);
        context.Cards.Update(card);
        await context.SaveChangesAsync();

        logger.LogInformation("Credit card purchase completed successfully.");
    }
}