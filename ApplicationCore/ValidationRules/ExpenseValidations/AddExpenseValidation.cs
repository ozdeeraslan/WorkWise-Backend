using ApplicationCore.DTOs.ExpenseDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ValidationRules.ExpenseValidations
{
	public class AddExpenseValidation : AbstractValidator<AddExpenseDto>
	{
        public AddExpenseValidation()
        {
			RuleFor(x => x.File)
			   .Must(x => x == null || new[] { ".pdf" }.Contains(Path.GetExtension(x.FileName).ToLower()))
			   .WithMessage("Please upload a file with .pdf extension.");
		}
    }
}
