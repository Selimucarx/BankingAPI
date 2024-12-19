using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Responses;
using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Enums;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Infrastructure.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper; // AutoMapper dependency

    public TransactionService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper; // Inject AutoMapper
    }


    public async Task<CreateTransactionResponse> TransferBorc(CreateBorcOde createBorcOde)
    {
        // 1. Hesabı AccountId ile bul
        var account =
            await _context.Accounts.FirstOrDefaultAsync(a =>
                a.Id == createBorcOde.AccountId && a.IsActive && !a.IsDeleted);
        if (account == null) throw new System.Exception("Hesap bulunamadı veya geçersiz.");

        // 2. Kartı Banka Adı ve Kart Numarasına göre bul
        var card = await _context.Cards.FirstOrDefaultAsync(c => c.CardNumber == createBorcOde.CardNumber);
        if (card == null) throw new System.Exception("Kart bulunamadı.");

        // 3. Gönderilecek miktarın null olmadığını kontrol et
        if (!createBorcOde.Amount.HasValue) throw new System.Exception("Gönderilecek miktar belirtilmedi.");

        // 4. Gönderilecek miktarın hesap bakiyesinden düşük olduğunu kontrol et
        var amount = createBorcOde.Amount.Value; // Nullable decimal'i decimal'e dönüştürdük
        if (account.Balance < amount) throw new System.Exception("Yetersiz bakiye.");

        // 5. Hesap bakiyesinden düş ve karta ekle
        account.Balance -= amount;
        card.AvailableLimit += amount;

        // 6. Güncellenen değerleri kaydet
        _context.Accounts.Update(account);
        _context.Cards.Update(card);
        await _context.SaveChangesAsync();

        // 7. İşlem sonucunu döndür
        return new CreateTransactionResponse(
            account.Id,
            card.Id, // CardId
            account.Id, // AccountId
            card.CustomerId, // CustomerId
            card.CardNumber, // CardNumber
            "SenderIban", // SenderIbanNumber (Bu örnekte boş veya geçerli bir değer verin)
            "ReceiverIban", // ReceiverIbanNumber (Bu örnekte boş veya geçerli bir değer verin)
            amount, // Amount
            "Payment", // Description (Burada işlem açıklamasını ekleyin)
            3, // PaymentType (Örnek olarak 3 verilmiştir)
            DateTime.UtcNow, // CreatedAt
            false // IsDeleted
        );
    }


    public async Task<CreateTransactionResponse> TransferMoneyAsync(CreateTransactionRequest request)
    {
        try
        {
            // Map the CreateTransactionRequest to the Transaction entity
            var transaction = _mapper.Map<Transaction>(request);

            transaction.PaymentType = (PaymentType)2;

            // Fetch the card associated with the transaction
            var card = await _context.Cards
                .Where(c => c.Id == transaction.CardId && c.IsActive == true)
                .FirstOrDefaultAsync();


            // Create the transaction in the database
            transaction.AccountId = card.AccountId;
            transaction.CustomerId = card.CustomerId;
            transaction.CardNumber = card.CardNumber;

            // Update the available limit on the card
            card.AvailableLimit -= transaction.Amount;

            // Save the transaction and updated card info to the database
            await _context.Transactions.AddAsync(transaction);
            _context.Cards.Update(card);
            await _context.SaveChangesAsync();


            // Map the transaction entity to the CreateTransactionResponse
            var response = _mapper.Map<CreateTransactionResponse>(transaction);

            return response;
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }
}