using ApplicationCore.DTOs.AdvancePaymentDto;
using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceController : ControllerBase
    {
        private readonly IAdvanceService _advanceService;
        private readonly UserManager<AppUser> _userManager;
        private readonly WorkWiseContext _db;
        public AdvanceController(IAdvanceService advanceService, UserManager<AppUser> userManager, WorkWiseContext db)
        {
            _advanceService = advanceService;
            _userManager = userManager;
            _db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAdvance(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var advances = await _advanceService.GetAllUserAdvances(user.Id);

            return Ok(advances);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddAdvance(string userId, [FromForm]AddAdvancePaymentDto addAdvancePaymentDto)
        {
            try
            {
                await _advanceService.CreateAdvance(userId, addAdvancePaymentDto);


                return Ok("Successfuly added advance request.");


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpGet("companies/{companyManagerId}")]
        public async Task<IActionResult> GetCompanyAdvance(string companyManagerId)
        {
            var companyManager = await _userManager.FindByIdAsync(companyManagerId);

            if (companyManager == null)
            {
                return NotFound();
            }

            var advances = await _advanceService.GetAllCompanyAdvances(companyManager.CompanyId);

            return Ok(advances);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> CancelApprovalStatus([FromRoute] string userId, [FromForm] int advanceId)
        {
            try
            {
                var cancelledAdvance = await _advanceService.CancelStatus(userId, advanceId);

                return Ok(cancelledAdvance);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }


        [HttpPut("advances/{advanceId}")]
        public async Task<IActionResult> UpdateExpenseStatusByCompanyManager(int advanceId, [FromForm]ApprovalStatus newStatus)
        {
            try
            {
                var updatedAdvanceStatus = await _advanceService.UpdateStatusByCompanyManager(advanceId, newStatus);

                return Ok(updatedAdvanceStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

