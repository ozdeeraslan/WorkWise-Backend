using ApplicationCore.DTOs.RegisterDto;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid email or password");
            }

            if (await _authService.Login(user))
            {
                var tokenString = await _authService.GenerateTokenString(user);

                return Ok(new
                {
                    token = tokenString,
                    message = "Success"
                });
            }

            return BadRequest();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto passwordDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(passwordDto.UserId);

                if (user == null)
                {
                    return NotFound();
                }

                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, passwordDto.Password);

                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return Ok("Password has changed succesfully. Please re-login with new password");
            }

            return BadRequest("Please enter a new password");
        }

        
    }
}
