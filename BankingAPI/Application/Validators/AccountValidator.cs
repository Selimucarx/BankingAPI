using BankingAPI.Application.DTOs;
using FluentValidation;

namespace BankingAPI.Application.Validators;

public abstract class AccountValidator : AbstractValidator<AccountDto>
{
    private void ValidateAccountName()
    {
        RuleFor(c => c.AccountName)
            .NotEmpty().WithMessage("Account name cannot be empty")
            .Length(2, 50).WithMessage("Account name must be between 2 and 100 characters");
    }

    private void ValidateIban()
    {
        RuleFor(c => c.Iban)
            .NotEmpty().WithMessage("Iban cannot be empty")
            .Length(26).WithMessage("Iban must be between 2 and 100 characters");
    }
}