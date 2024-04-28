using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Expense
    {
        public int Id { get; set; }

        public Currency Currency { get; set; }

        public ExpenseType ExpenseType { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public string? FilePath { get; set; }

        public string AppUserId { get; set; }
    }
}
