using ApplicationCore.Entities;
using ApplicationCore.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.UserDto
{
    public class AddUserDto
    {
        public string FirstName { get; set; } = null!;

        public string? SecondName { get; set; }

        public string LastName { get; set; } = null!;

        public string? SecondLastName { get; set; }

        public string Address { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public string PlaceOfBirth { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Status Status { get; set; }

        public string TRIdentityNumber { get; set; } = null!;

        public string Profession { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public IFormFile? Picture { get; set; }

        public int CompanyId { get; set; }

        public decimal? Wage { get; set; }
    }
}
