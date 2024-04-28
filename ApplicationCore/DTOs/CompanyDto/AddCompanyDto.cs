using ApplicationCore.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.CompanyDto
{
    public class AddCompanyDto
    {
        public string Name { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string MersisNumber { get; set; } = null!;

        public string TaxNumber { get; set; } = null!;

        public string TaxAdministration { get; set; } = null!;

        public IFormFile Logo { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string FoundingYear { get; set; } = null!;

        public DateTime ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int NumberOfEmployees { get; set; }

        public Status Status { get; set; }
    }
}
