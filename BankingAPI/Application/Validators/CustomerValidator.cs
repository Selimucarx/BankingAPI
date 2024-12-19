using BankingAPI.Application.Requests;
using FluentValidation;

namespace BankingAPI.Application.Validators;

public class CustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CustomerValidator()
    {
        ValidateFullName();
        ValidateEmail();
        ValidatePassword();
        ValidateNationalNumber();
        ValidatePlaceOfBirth();
        ValidateDateOfBirth();
    }

    // Validate Full Name
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


    private void ValidatePassword()
    {
        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .NotNull().WithMessage("Password cannot be null.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
    }


    private void ValidateNationalNumber()
    {
        RuleFor(c => c.NationalNumber)
            .NotEmpty().WithMessage("National number cannot be empty.")
            .NotNull().WithMessage("National number cannot be null.")
            .Length(11)
            .WithMessage(
                "National number must be 11 characters.") // This will trigger if it's not exactly 11 characters
            .Matches(@"^\d{11}$").WithMessage("National number must consist of 11 digits.");
    }


    private void ValidatePlaceOfBirth()
    {
        RuleFor(c => c.PlaceOfBirth)
            .NotEmpty().WithMessage("Place of birth cannot be empty.")
            .NotNull().WithMessage("Place of birth cannot be null.")
            .Length(2, 100).WithMessage("Place of birth must be between 2 and 100 characters.")
            .Matches("^[a-zA-ZğüşöçıĞÜŞÖÇİ ]+$")
            .WithMessage("Place of birth must only contain alphabetic characters.");
    }


    private void ValidateDateOfBirth()
    {
        RuleFor(c => c.DateOfBirth)
            .NotNull().WithMessage("Date of birth cannot be null.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date of birth cannot be in the future.")
            .WithMessage("Customer must be at least 18 years old.");
    }
}