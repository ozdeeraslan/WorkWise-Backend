using ApplicationCore.DTOs.AdvancePaymentDto;
using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AdvanceService : IAdvanceService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly WorkWiseContext _context;


        public AdvanceService(UserManager<AppUser> userManager, WorkWiseContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<List<AddAdvancePaymentDto>> GetAllUserAdvances(string appUserId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.AdvancePayments)
                    .FirstOrDefaultAsync(u => u.Id == appUserId);

                if (user == null)
                {
                    throw new InvalidOperationException("User not found.");
                }

                var advances = user.AdvancePayments.Select(e => new AddAdvancePaymentDto
                {
                    Currency = e.Currency,
                    AdvanceType = e.AdvanceType,
                    ApprovalStatus = e.ApprovalStatus,
                    Amount = e.Amount,
                    RequestDate = e.RequestDate,
                    ApprovalDate = e.ApprovalDate,
                    Description = e.Description,
                    Id = e.Id
                }).ToList();

                return advances.OrderByDescending(a => a.RequestDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Advances could not be received.", ex);


            }
        }

        public async Task<AdvancePayment> CreateAdvance(string appUserId,AddAdvancePaymentDto addAdvancePaymentDto)
        {
            try
            {
                var advance = new AdvancePayment
                {
                    Currency = addAdvancePaymentDto.Currency,
                    AdvanceType = addAdvancePaymentDto.AdvanceType,
                    ApprovalStatus = 0,
                    Amount = addAdvancePaymentDto.Amount,
                    RequestDate = DateTime.Now,
                    ApprovalDate = null,
                    AppUserId = appUserId,
                    Description = addAdvancePaymentDto.Description
                };


                _context.AdvancePayments.Add(advance);
                await _context.SaveChangesAsync();

                return advance;
            }
            catch (Exception ex)
            {
                throw new Exception("Advance payment could not be created.", ex);
            }
        }

        public async Task<List<CompanyAdvanceDto>> GetAllCompanyAdvances(int companyId)
        {
            try
            {
                var companyUsers = await _context.Users
                    .Where(u => u.CompanyId == companyId)
                    .Include(u => u.AdvancePayments)
                    .ToListAsync();

                if (companyUsers == null)
                {
                    throw new InvalidOperationException("Users not found in the company.");
                }

                var companyAdvances = new List<CompanyAdvanceDto>();

                foreach (var user in companyUsers)
                {
                    var userAdvances = user.AdvancePayments.Select(advance => new AddAdvancePaymentDto
                    {
                        Id = advance.Id,
                        Amount = advance.Amount,
                        RequestDate = advance.RequestDate,
                        ApprovalDate = advance.ApprovalDate,
                        ApprovalStatus = advance.ApprovalStatus,
                        Currency = advance.Currency,
                        AdvanceType = advance.AdvanceType,
                        Description = advance.Description
                    }).ToList();
                    if (userAdvances.Count>0)
                    {
                        foreach (var advance in userAdvances)
                        {
                            companyAdvances.Add(new CompanyAdvanceDto
                            {
                                AppUserId = user.Id,
                                FirstName = user.PersonalDetail.FirstName,
                                LastName = user.PersonalDetail.LastName,
                                Profession = user.PersonalDetail.Profession,
                                Department = user.PersonalDetail.Department,
                                FilePath = user.PersonalDetail.FilePath,
                                Advance = advance
                            });
                        }
                    }
                        
                }

                return companyAdvances;
            }
            catch (Exception ex)
            {
                throw new Exception("Advances could not be retrieved.", ex);
            }
        }

        public async Task<AdvanceStatusDto> CancelStatus(string appUserId, int advanceId)
        {
            var advancePayment = await _context.AdvancePayments.FirstOrDefaultAsync(a => a.Id == advanceId && a.AppUserId == appUserId);

            if (advancePayment.ApprovalStatus == ApprovalStatus.Pending)
            {
                advancePayment.ApprovalStatus = ApprovalStatus.Cancelled;

                _context.Update(advancePayment);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Invalid advanceId.");
            }

            return new AdvanceStatusDto
            {
                ApprovalStatus = ApprovalStatus.Cancelled.ToString()
            };
        }

        public async Task<AdvanceStatusDto> UpdateStatusByCompanyManager(int advanceId, ApprovalStatus newStatus)
        {
            try
            {
                var advance = await _context.AdvancePayments.FirstOrDefaultAsync(e => e.Id == advanceId);
                if (advance == null)
                {
                    throw new KeyNotFoundException("Advance not found for the user.");
                }

                advance.ApprovalStatus = newStatus;
                advance.ApprovalDate = DateTime.Now;

                _context.Update(advance);
                await _context.SaveChangesAsync();

                return new AdvanceStatusDto
                {
                    ApprovalStatus = newStatus.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Advance status could not be updated.", ex);
            }
        }
    }
}


