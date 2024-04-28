using ApplicationCore.DTOs.AdvancePaymentDto;
using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly WorkWiseContext _context;
        private readonly IFileService _fileService;

        public ExpenseService(UserManager<AppUser> userManager, WorkWiseContext context, IFileService fileService)
        {
            _userManager = userManager;
            _context = context;
            _fileService = fileService;
        }

        public async Task<ExpenseStatusDto> CancelStatus(string appUserId, int expenseId)
        {
            
            var expense = await _context.Expenses.FirstOrDefaultAsync(a => a.Id == expenseId && a.AppUserId == appUserId);

            if (expense.ApprovalStatus == ApprovalStatus.Pending)
            {
                expense.ApprovalStatus = ApprovalStatus.Cancelled;

                _context.Update(expense);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Invalid expenseId.");
            }

            return new ExpenseStatusDto
            {
                ExpenseStatus = ApprovalStatus.Cancelled.ToString()
            };
        }

        public async Task<Expense> CreateExpense(string appUserId, AddExpenseDto addExpenseDto)
        {
            try
            {
                var fileUrl = _fileService.UploadFile(addExpenseDto.File); 
               
                var expense = new Expense
                {
                    Currency = addExpenseDto.Currency,
                    ExpenseType = addExpenseDto.ExpenseType,
                    ApprovalStatus = 0,
                    Amount = addExpenseDto.Amount,
                    RequestDate = DateTime.Now,
                    ApprovalDate = null,
                    AppUserId = appUserId,
                    FilePath = fileUrl
                };

                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return expense;
            }
            catch (Exception)
            {

                throw new Exception("Expense could not be created.");
            }
        }

        public async Task<List<CompanyExpensesDto>> GetAllCompanyExpenses(int companyId)
        {
            try
            {
                var companyUsers = await _context.Users
                    .Where(u => u.CompanyId == companyId)
                    .Include(u => u.Expenses)
                    .ToListAsync();

                if (companyUsers == null)
                {
                    throw new InvalidOperationException("Users not found in the company.");
                }

                var companyExpenses = new List<CompanyExpensesDto>();

                foreach (var user in companyUsers)
                {
                    var userExpenses = user.Expenses.Select(expense => new AddExpenseDto
                    {
                        Id = expense.Id,
                        Amount = expense.Amount,
                        RequestDate = expense.RequestDate,
                        ApprovalDate = expense.ApprovalDate,
                        ApprovalStatus = expense.ApprovalStatus,
                        Currency = expense.Currency,
                        ExpenseType = expense.ExpenseType,
                        FilePath = expense.FilePath,
                    }).ToList();
                    if (userExpenses.Count>0)
                    {
                        foreach (var expense in userExpenses)
                        {
                            companyExpenses.Add(new CompanyExpensesDto
                            {
                                AppUserId = user.Id,
                                FirstName = user.PersonalDetail.FirstName,
                                LastName = user.PersonalDetail.LastName,
                                Profession = user.PersonalDetail.Profession,
                                Department = user.PersonalDetail.Department,
                                FilePath = user.PersonalDetail.FilePath,
                                Expense = expense
                            });
                        }
                    }
                }

                return companyExpenses;
            }
            catch (Exception ex)
            {
                throw new Exception("Expenses could not be retrieved.", ex);
            }
        }


        public async Task<List<AddExpenseDto>> GetAllUserExpenses(string appUserId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Expenses)
                    .FirstOrDefaultAsync(u => u.Id == appUserId);

                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }

                var expenses = user.Expenses.Select(e => new AddExpenseDto
                {
                    // Expense özelliklerini AddExpenseDto özelliklerine eşleştir
                    Amount = e.Amount,
                    RequestDate = e.RequestDate,
                    ApprovalDate = e.ApprovalDate,
                    ApprovalStatus = e.ApprovalStatus,
                    Currency = e.Currency,
                    ExpenseType = e.ExpenseType,
                    FilePath = e.FilePath,
                    Id = e.Id

                }).ToList();

                return expenses.OrderByDescending(e => e.RequestDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Expenses could not be received.", ex);
            }
        }

        public async Task<ExpenseStatusDto> UpdateStatusByCompanyManager(int expenseId, ApprovalStatus newStatus)
        {
            try
            {
                var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId);
                if (expense == null)
                {
                    throw new KeyNotFoundException("Expense not found for the user.");
                }

           
                    expense.ApprovalStatus = newStatus;
                    expense.ApprovalDate = DateTime.Now;
                

                _context.Update(expense);
                await _context.SaveChangesAsync();

                return new ExpenseStatusDto
                {
                    ExpenseStatus = newStatus.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Expense status could not be updated.", ex);
            }
        }
    }
}

