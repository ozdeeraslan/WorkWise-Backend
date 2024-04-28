using ApplicationCore.DTOs.LeaveDto;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly ILeaveValidateService _leaveValidateService;
        private readonly IAnnualLeaveService _annualLeaveService;
        private readonly UserManager<AppUser> _userManager;

        public LeaveController(ILeaveRepository leaveRepository, ILeaveValidateService leaveValidateService, IAnnualLeaveService annualLeaveService, UserManager<AppUser> userManager)
        {
            _leaveRepository = leaveRepository;
            _leaveValidateService = leaveValidateService;
            _annualLeaveService = annualLeaveService;
            _userManager = userManager;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddLeave(AddLeaveDto leave, string userId)
        {
            try
            {
                var validatedLeave= await _leaveValidateService.ValidateLeave(leave, userId);

                var leaveSucces = await _leaveRepository.AddLeaveAsync(validatedLeave, userId);
                
                return Ok(leaveSucces);

                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserLeaves(string userId)
        {
            var leaves = await _leaveRepository.GetUserLeavesAsync(userId);
            return Ok(leaves);
        }

        [HttpGet("Leaves/{companyManagerId}")]
        public async Task<IActionResult> GetCompanyLeaves(string companyManagerId)
        {
            var companyManager = await _userManager.FindByIdAsync(companyManagerId);

            if (companyManager == null)
            {
                return NotFound();
            }

            var expenses = await _leaveRepository.GetAllCompanyLeaves(companyManager.CompanyId);

            return Ok(expenses);
        }

        [HttpGet("RemainingLeaves/{userId}")]
        public async Task<IActionResult> GetRemainingLeaves(string userId)
        {
            var remainingLeaves = await _annualLeaveService.GetRemainingAnnualLeaveDays(userId);

            return Ok(remainingLeaves);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> CancelApprovalStatus([FromRoute] string userId, [FromForm] int leaveId)
        {
            try
            {
                var cancelledLeave = await _leaveRepository.CancelStatus(userId, leaveId);

                return Ok(cancelledLeave);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("Leaves/{leaveId}")]
        public async Task<IActionResult> UpdateLeavesStatusByCompanyManager(int leaveId, [FromForm] ApprovalStatus newStatus)
        {
            try
            {
                var updatedLeaveeStatus = await _leaveRepository.UpdateStatusByCompanyManager(leaveId, newStatus);

                return Ok(updatedLeaveeStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
