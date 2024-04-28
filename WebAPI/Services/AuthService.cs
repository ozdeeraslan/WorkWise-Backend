using ApplicationCore.DTOs.RegisterDto;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<string> GenerateTokenString(LoginDto user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);

            if (appUser == null)
            {
                throw new Exception("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(appUser);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim("Email", appUser.Email!),
                new Claim("Id", appUser.Id),
                new Claim("IsConfirmed", appUser.EmailConfirmed.ToString()),
                new Claim("Role", roles[0].ToString()),
                new Claim("CompanyId", appUser.CompanyId.ToString())
            };

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                issuer: _config.GetSection("JwtSettings:Issuer").Value,
                audience: _config.GetSection("JwtSettings:Audience").Value,
                signingCredentials: signingCred
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }

        public async Task<bool> Login(LoginDto user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);

            if (identityUser is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        //public async Task<bool> RegisterUser(RegisterDto user)
        //{
        //    var identityUser = new AppUser
        //    {
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        EmailConfirmed = user.EmailComfirm,
        //        CompanyId = user.CompanyId
        //    };

        //    var result = await _userManager.CreateAsync(identityUser, user.Password);
        //    return result.Succeeded;

        //}
    }
}
