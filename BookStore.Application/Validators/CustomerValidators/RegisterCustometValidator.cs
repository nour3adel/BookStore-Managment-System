using BookStore.Domain.DTOs.CustomerDTOs;
using FluentValidation;

namespace BookStore.Application.Validators.CustomerValidators
{
    public class RegisterCustomerValidator : AbstractValidator<RegisterCustomerDTO>
    {
        public RegisterCustomerValidator()
        {
            // Full name validation (optional)
            RuleFor(x => x.fullname)
                .NotEmpty().WithMessage("Full name is required.");

            // Username validation
            RuleFor(x => x.username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(4, 20).WithMessage("Username must be between 4 and 20 characters.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

            // Password validation
            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

            // Email validation
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            // Address validation
            RuleFor(x => x.address)
                .NotEmpty().WithMessage("Address is required.")
                .Length(5, 100).WithMessage("Address must be between 5 and 100 characters.");

            // Phone number validation
            RuleFor(x => x.phonenumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[\d\s\(\)-]{10,15}$").WithMessage("Phone number must be between 10 and 15 digits, and may contain spaces, dashes, and parentheses.");

        }



    }
}

