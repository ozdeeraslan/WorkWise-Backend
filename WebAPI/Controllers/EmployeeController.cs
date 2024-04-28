using ApplicationCore.DTOs.UserDto;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public EmployeeController(IPhotoService photoService, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _photoService = photoService;
            _userManager = userManager;
            _emailService = emailService;
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {

            var allEmployees = await _userManager.Users.Include(x => x.Company)
            .ToListAsync();

            // Employee rolüne sahip olan kullanıcıları filtreleyip çekiyoruz
            var employees = allEmployees.Where(user => _userManager.IsInRoleAsync(user, "Employee").Result).ToList();

            if (employees == null)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");

            var employee = employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost(Name ="Add")]
        public async Task<IActionResult> Add(AddUserDto user)
        {
            var employee = user.ToAppUser();


            string fileUrl = "";
            fileUrl = _photoService.UploadFile(user.Picture, employee.Id);
            employee.PersonalDetail.FileName = employee.PersonalDetail.FirstName + "-image";
            employee.PersonalDetail.FilePath = fileUrl;

            string password = GeneratePassword();
             await _userManager.CreateAsync(employee, password);

            
            await _userManager.AddToRoleAsync(employee, "Employee");
            _emailService.SendPassword(employee.Email, password);
            return Ok();
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> SendResetPasswordMail(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return NotFound();
                }

                _emailService.SendResetPasswordMail(user.Id, email);
                user.EmailConfirmed = false;
                await _userManager.UpdateAsync(user);

                return Ok("Reset link sent to your mail");
            }

            return BadRequest("Please enter a your email");
        }

        private string GeneratePassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            // Password validation
            string generatedPassword = password.ToString();
            if (!Regex.IsMatch(generatedPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$"))
            {
                // If generated password does not meet the validation criteria, generate a new one
                return GeneratePassword();
            }

            return generatedPassword;
        }



    }
}

