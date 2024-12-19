using BankingAPI.Application.Requests;
using FluentValidation;

namespace BankingAPI.Application.Validators;

public class PurchaseRequestValidator : AbstractValidator<PurchaseRequest>
{
    public PurchaseRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}