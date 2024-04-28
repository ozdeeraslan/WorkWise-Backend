using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class PersonalDetail
    {
        public string FirstName { get; set; } = null!;

        public string? SecondName { get; set; }

        public string LastName { get; set; } = null!;

        public string? SecondLastName { get; set; }

        public string? Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? PlaceOfBirth { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        // public string? PictureUri { get; set; }

        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        public string TRIdentityNumber { get; set; } = null!;

        public Status Status { get; set; }

        public string Profession { get; set; } = null!;

        public string Department { get; set; } = null!;

        public decimal? Wage { get; set; }

        
    }
}
