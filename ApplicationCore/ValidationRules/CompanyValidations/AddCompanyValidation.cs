using ApplicationCore.DTOs.CompanyDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ValidationRules.CompanyValidations
{
    public class AddCompanyValidation : AbstractValidator<AddCompanyDto>
    {
        public AddCompanyValidation()
        {
            RuleFor(x => x.Name)
                .MaximumLength(50)
                .WithMessage("Company name must be at most 50 characters long");

            RuleFor(x => x.Title)
                .MaximumLength(100)
                .WithMessage("Company title must be at most 100 characters long");

            RuleFor(x => x.MersisNumber)
                .Matches("[0-9]").WithMessage("Please enter only numbers in the MERSIS number field.")
                .Length(16).WithMessage("MERSIS number must be exactly 16 characters long");

            RuleFor(x => x.TaxNumber)
                .Matches("[0-9]").WithMessage("Please enter only numbers in the Tax number field.")
                .Length(10).WithMessage("Tax number must be 10 characters long");

            RuleFor(x => x.TaxAdministration)
                .MaximumLength(50).WithMessage("Tax administration must be at most 50 characters long");

            RuleFor(x => x.Logo)
                .Must(x => x.Length > 0 && x.Length <= 2 * 1024 * 1024)
                .WithMessage("Image size must be no more than 2 MB.")
                .Must(x => x == null || new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(x.FileName).ToLower()))
                .WithMessage("Please upload an image with .jpg, .jpeg or .png extension.");

            RuleFor(x => x.PhoneNumber)
                .Matches("^[0-9]+$").WithMessage("Please enter only numbers in the phone number field.")
                .Length(11).WithMessage("The phone number must be 11 digits long.");

            RuleFor(x => x.FoundingYear)
                .Matches("[0-9]").WithMessage("Please enter only numbers in the founding year field.")
                .Length(4).WithMessage("Founding year must 4 digits long");           

            RuleFor(x => x.ContractEndDate)
                .GreaterThan(x => x.ContractStartDate).WithMessage("Contract end date must be after contract start date");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address must be at most 255 characters long");

            RuleFor(x => x.Email)
                .Must(x => x.Contains("@") && x.Contains(".")).WithMessage("Please enter a valid email address.")
                .MaximumLength(50).WithMessage("Email must be at most 50 characters long");

            RuleFor(x => x.NumberOfEmployees)
                .NotEmpty().WithMessage("Number of employees cannot be empty")
                .GreaterThan(0).WithMessage("Number of employees must be greater than 0");
        }
    }
}
