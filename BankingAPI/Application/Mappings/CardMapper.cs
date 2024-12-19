using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.Mappings;

public class CardMapper : Profile
{
    public CardMapper()
    {
        CreateMap<Card, CardDto>().ReverseMap();


        CreateMap<Card, CreateCardRequest>().ReverseMap();

        CreateMap<Card, PurchaseRequest>().ReverseMap();
        
        CreateMap<Card, PuschaseCredit>().ReverseMap();

        
        CreateMap<Card, Puschadebit>().ReverseMap();

    }
}