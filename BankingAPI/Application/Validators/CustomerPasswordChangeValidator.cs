using BankingAPI.Application.DTOs;
using FluentValidation;

namespace BankingAPI.Application.Validators;

public class PasswordChangeValidator : AbstractValidator<CustomerPasswordChangeRequest>
{
    
    public PasswordChangeValidator()
    {
        ValidatePassword();

    }
    
    
    
    private void ValidatePassword()
    {
        RuleFor(c => c.NewPassword)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .NotNull().WithMessage("Password cannot be null.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
    }

}