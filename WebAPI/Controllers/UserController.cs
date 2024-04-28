using ApplicationCore.DTOs.UserDto;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IWebHostEnvironment env, IPhotoService photoService, UserManager<AppUser> userManager)
        {
            _env = env;
            _photoService = photoService;
            _userManager=userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDto user, string id)
        {
            try
            {
                string fileUrl = "";

                var appUser = await _userManager.FindByIdAsync(id);

                if (user.Picture != null)
                {
                    fileUrl = _photoService.UploadFile(user.Picture, id);

                    appUser.PersonalDetail.FileName = appUser.PersonalDetail.FirstName + "-image";
                    appUser.PersonalDetail.FilePath = fileUrl;
                }

                appUser.PersonalDetail.Address = user.Address;
                appUser.PhoneNumber = user.PhoneNumber;

                await _userManager.UpdateAsync(appUser);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }



        }
    }
}
    


