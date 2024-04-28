using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.RegisterDto
{
    public class RegisterDto
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool EmailComfirm { get; set; }
        public int CompanyId { get; set; }
    }
}
