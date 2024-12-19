using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.Mappings;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<Account, AccountDto>().ReverseMap();
    }
}