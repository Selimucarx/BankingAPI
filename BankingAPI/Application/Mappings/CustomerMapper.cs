using AutoMapper;
using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Entities;

namespace BankingAPI.Application.Mappings;

public class CustomerMapper : Profile
{
    public CustomerMapper()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();

        CreateMap<CreateCustomerRequest, Customer>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.NationalNumber, opt => opt.MapFrom(src => src.NationalNumber))
            .ForMember(dest => dest.PlaceOfBirth, opt => opt.MapFrom(src => src.PlaceOfBirth))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));
    }
}