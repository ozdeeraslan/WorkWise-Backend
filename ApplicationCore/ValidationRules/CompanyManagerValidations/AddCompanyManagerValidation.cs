using ApplicationCore.DTOs.CompanyDto;
using ApplicationCore.DTOs.UserDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ValidationRules.CompanyManagerValidations
{
    public class AddCompanyManagerValidation : AbstractValidator<AddUserDto>
    {
        public AddCompanyManagerValidation()
        {
            RuleFor(x => x.FirstName)
            .MaximumLength(25).WithMessage("First name must be at most 25 characters long");

            RuleFor(x => x.SecondName)
                .MaximumLength(25).WithMessage("Second name must be at most 25 characters long");

            RuleFor(x => x.LastName)
                .MaximumLength(25).WithMessage("Last name must be at most 25 characters long");

            RuleFor(x => x.SecondLastName)
                .MaximumLength(25).WithMessage("Second last name must be at most 25 characters long");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address must be at most 255 characters long");

            RuleFor(x => x.BirthDate)
                .Must(x => x < DateTime.Now).WithMessage("Birth date cannot be in the future")
                .Must(x => DateTime.Today.Year - x.Year >= 18).WithMessage("Company Manager must be at least 18 years old");

            RuleFor(x => x.PlaceOfBirth)
                .MaximumLength(100).WithMessage("Place of birth must be at most 100 characters long");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

            RuleFor(x => x.TRIdentityNumber)
                .Matches("[0-9]").WithMessage("Please enter only numbers in the identity number field.")
                .Length(11).WithMessage("TR identity number must be 11 characters long");

            RuleFor(x => x.Profession)
                .MaximumLength(50).WithMessage("Profession must be at most 50 characters long");

            RuleFor(x => x.Department)
                .MaximumLength(50).WithMessage("Department must be at most 50 characters long");

            RuleFor(x => x.Email)
                .Must(x => x.Contains("@") && x.Contains(".")).WithMessage("Please enter a valid email address.")
                .MaximumLength(50).WithMessage("Email must be at most 50 characters long");

            RuleFor(x => x.PhoneNumber)
                .Matches("^[0-9]+$").WithMessage("Please enter only numbers in the phone number field.")
                .Length(11).WithMessage("The phone number must be 11 digits long.");
        }
    }
}
