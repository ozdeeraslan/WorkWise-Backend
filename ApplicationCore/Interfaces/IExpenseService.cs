using ApplicationCore.DTOs.AdvancePaymentDto;
using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
	public interface IExpenseService
	{
		Task<Expense> CreateExpense(string appUserId, AddExpenseDto addExpenseDto);

		Task<List<AddExpenseDto>> GetAllUserExpenses(string appUserId);

		Task<List<CompanyExpensesDto>> GetAllCompanyExpenses(int companyId);

        Task<ExpenseStatusDto> CancelStatus(string appUserId, int expenseId);

		Task<ExpenseStatusDto> UpdateStatusByCompanyManager(int expenseId, ApprovalStatus newStatus);
    }
}
