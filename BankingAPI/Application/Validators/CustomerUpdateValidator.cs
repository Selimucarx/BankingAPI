using BankingAPI.Application.DTOs;
using FluentValidation;

namespace BankingAPI.Application.Validators;

public class UpdateCustomerValidator : AbstractValidator<CustomerUpdateRequest>
{

    UpdateCustomerValidator()
    {
        ValidateFullName();
        ValidateEmail();
    }
    
    
    private void ValidateFullName() 
    {
        RuleFor(c => c.FullName)
            .NotNull().WithMessage("Full name cannot be null.")
            .NotEmpty().WithMessage("Full name cannot be empty.")
            .Length(2, 100).WithMessage("Full name must be between 2 and 100 characters.");
    }

    private void ValidateEmail()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .NotNull().WithMessage("Email cannot be null.")
            .EmailAddress().WithMessage("Invalid email address.")
            .Matches(@"^.+@.+\..+$").WithMessage("Email must have a valid domain.");
    }
}