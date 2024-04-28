using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.ExpenseDto
{
    public class CompanyExpensesDto
    {
        public string AppUserId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Profession { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string? FilePath { get; set; }

        public AddExpenseDto Expense { get; set; }
    }
}
