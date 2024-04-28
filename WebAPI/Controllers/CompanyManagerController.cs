using ApplicationCore.DTOs.UserDto;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyManagerController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public CompanyManagerController(IPhotoService photoService, UserManager<AppUser> userManager)
        {
            _photoService = photoService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //var managers = await _userManager.GetUsersInRoleAsync("CompanyManager");
            var allUsers = await _userManager.Users.Include(x => x.Company).ToListAsync();

            // CompanyManager rolüne sahip olan kullanıcıları filtreleyip çekiyoruz
            var managers = allUsers.Where(user => _userManager.IsInRoleAsync(user, "CompanyManager").Result).ToList();

            if (managers == null)
            {
                return NotFound();
            }

            return Ok(managers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var managers = await _userManager.GetUsersInRoleAsync("CompanyManager");

            var manager = managers.FirstOrDefault(x => x.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return Ok(manager);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddUserDto user)
        {
            var manager = user.ToAppUser();

            string fileUrl = "";

            fileUrl = _photoService.UploadFile(user.Picture, manager.Id);
            manager.PersonalDetail.FileName = manager.PersonalDetail.FirstName + "-image";
            manager.PersonalDetail.FilePath = fileUrl;

            
            await _userManager.AddToRoleAsync(manager, "CompanyManager");
            await _userManager.CreateAsync(manager, "P@ssword1");

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDto user, string id)
        {
            try
            {
                string fileUrl = "";

                var employee = await _userManager.FindByIdAsync(id);

                if (user.Picture != null)
                {
                    fileUrl = _photoService.UploadFile(user.Picture, id);

                    employee.PersonalDetail.FileName = employee.PersonalDetail.FirstName + "-image";
                    employee.PersonalDetail.FilePath = fileUrl;
                }

                employee.PersonalDetail.Address = user.Address;
                employee.PhoneNumber = user.PhoneNumber;


                await _userManager.UpdateAsync(employee);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }
    }
}
