using ApplicationCore.DTOs.PasswordDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ValidationRules.PasswordValidations
{
    public class PasswordValidation : AbstractValidator<PasswordDto>
    {
        public PasswordValidation()
        {
            RuleFor(dto => dto.Password)
                                          .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                                          .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                                          .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                                          .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                                          .Matches("[!@#$%^&*]").WithMessage("Password must contain at least one special character (!@#$%^&*).");

            RuleFor(dto => dto.ConfirmPassword).Equal(dto => dto.Password).WithMessage("Passwords do not match.");
        }
    }
}

