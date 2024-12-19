using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Account, AccountDto>().ReverseMap();
        CreateMap<Transaction, TransactionDto>().ReverseMap();
        CreateMap<Card, CardDto>().ReverseMap();

    }
}