using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.AdvancePaymentDto
{
    public class AddAdvancePaymentDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public Currency Currency { get; set; }

        public AdvanceType AdvanceType { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? ApprovalDate { get; set; }
    }
}
