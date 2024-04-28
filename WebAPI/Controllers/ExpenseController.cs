using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;
        private readonly WorkWiseContext _db;

        public ExpenseController(IExpenseService expenseService, UserManager<AppUser> userManager, IAuthService authService, WorkWiseContext db)
        {
            _expenseService = expenseService;
            _userManager = userManager;
            _authService = authService;
            _db = db;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserExpense(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var expenses = await _expenseService.GetAllUserExpenses(user.Id);

            return Ok(expenses);
        }

        [HttpGet("companies/{companyManagerId}")]
        public async Task<IActionResult> GetCompanyExpense(string companyManagerId)
        {
            var companyManager = await _userManager.FindByIdAsync(companyManagerId);

            if (companyManager == null)
            {
                return NotFound();
            }

            var expenses = await _expenseService.GetAllCompanyExpenses(companyManager.CompanyId);

            return Ok(expenses);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddExpense(string userId, AddExpenseDto addExpenseDto)
        {
            try
            {
                await _expenseService.CreateExpense(userId, addExpenseDto);

                return Ok("Successfuly added expense.");

               
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> CancelApprovalStatus([FromRoute]string userId, [FromForm]int expenseId)
        {
            try
            {
                var cancelledExpense = await _expenseService.CancelStatus(userId, expenseId);

                return Ok(cancelledExpense);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("expenses/{expenseId}")]
        public async Task<IActionResult> UpdateExpenseStatusByCompanyManager(int expenseId, [FromForm]ApprovalStatus newStatus)
        {
            try
            {
                var updatedExpenseStatus = await _expenseService.UpdateStatusByCompanyManager(expenseId, newStatus);

                return Ok(updatedExpenseStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

