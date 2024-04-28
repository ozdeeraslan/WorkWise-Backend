using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.RegisterDto
{
    public class ResetPasswordDto
    {
        public string UserId { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
