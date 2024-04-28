using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.UserDto
{
    public class UpdateUserDto
    {
        public IFormFile? Picture { get; set; }
       
        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

    }
}
