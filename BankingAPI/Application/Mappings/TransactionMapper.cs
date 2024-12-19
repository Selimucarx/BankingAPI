using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Application.Responses;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.Mappings;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<Transaction, TransactionDto>().ReverseMap();


        CreateMap<CreateTransactionRequest, Transaction>()
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.SenderIbanNumber, opt => opt.MapFrom(src => src.SenderIban))
            .ForMember(dest => dest.ReceiverIbanNumber, opt => opt.MapFrom(src => src.ReceiverIban));


        // Map Transaction to CreateTransactionResponse
        CreateMap<Transaction, CreateTransactionResponse>()
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));


        CreateMap<Puschadebit, Transaction>()
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => CardType.Debit)); 


    }
}