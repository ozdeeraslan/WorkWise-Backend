using ApplicationCore.DTOs.UserDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ValidationRules.SiteOwnerValidations
{
    public class UpdateProfileValidation : AbstractValidator<UpdateUserDto>
    {
        public UpdateProfileValidation()
        {
            RuleFor(x => x.Picture) 
                .Must(x => x == null || x.Length > 0 && x.Length <= 2 * 1024 * 1024)
                .WithMessage("Image size must be no more than 2 MB.")
                .Must(x => x == null || new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(x.FileName).ToLower()))
                .WithMessage("Please upload an image with .jpg, .jpeg or .png extension.");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address must be at least 255 characters long.");

            RuleFor(x => x.PhoneNumber)
                .Matches("^[0-9]+$").WithMessage("Please enter only numbers in the phone number field.")
                .Length(11).WithMessage("The phone number must be 11 digits long.");
        }
    }
}
