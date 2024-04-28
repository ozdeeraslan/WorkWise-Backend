using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppUser : IdentityUser
    {
        public PersonalDetail? PersonalDetail { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        public List<Leave>? Leaves { get; set; }

        public List<AdvancePayment>? AdvancePayments { get; set; }

        public List<Expense>? Expenses { get; set; }
    }
}
