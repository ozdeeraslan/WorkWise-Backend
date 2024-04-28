using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.DTOs.LeaveDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly WorkWiseContext _db;

        public LeaveRepository(WorkWiseContext db)
        {
            _db = db;
        }

        public async Task<Leave> AddLeaveAsync(AddLeaveDto leave, string userId)
        {
            try
            {

                var newLeave = new Leave
                {
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    LeaveType = leave.LeaveType,
                    ApprovalStatus = ApprovalStatus.Pending,
                    RequestDate = DateTime.Now,
                    ResponseDate = null,
                    Days = leave.Days,
                    AppUserId = userId
                };

                _db.Leaves.Add(newLeave);
                await _db.SaveChangesAsync();

                return newLeave;
            }
            catch (Exception ex)
            {
                throw new Exception("Leave could not be created.", ex);
            }

        }

        public async Task<List<CompanyLeaveDto>> GetAllCompanyLeaves(int companyId)
        {
            try
            {
                var companyUsers = await _db.Users
                    .Where(u => u.CompanyId == companyId)
                    .Include(u => u.Leaves)
                    .ToListAsync();

                if (companyUsers == null)
                {
                    throw new InvalidOperationException("Users not found in the company.");
                }

                var companyLeaves = new List<CompanyLeaveDto>();

                foreach (var user in companyUsers)
                {
                    var userLeaves = user.Leaves
                        .OrderBy(leave => leave.ApprovalStatus)
                        .Select(leave => new AddLeaveDto
                    {
                        Days = leave.Days,
                        StartDate = leave.StartDate,
                        LeaveType = leave.LeaveType,
                        EndDate = leave.EndDate,
                        RequestDate= leave.RequestDate,
                        ResponseDate = leave.ResponseDate,
                        ApprovalStatus = leave.ApprovalStatus,

                    }).ToList();

                    if (userLeaves.Count > 0)
                    {
                        foreach (var leave in userLeaves)
                        {
                            if (leave.ApprovalStatus != ApprovalStatus.Cancelled)
                            {
                                companyLeaves.Add(new CompanyLeaveDto
                                {
                                    AppUserId = user.Id,
                                    FirstName = user.PersonalDetail.FirstName,
                                    LastName = user.PersonalDetail.LastName,
                                    Profession = user.PersonalDetail.Profession,
                                    Department = user.PersonalDetail.Department,
                                    FilePath = user.PersonalDetail.FilePath,
                                    Leave = leave
                                });
                            }
                        }
                    }
                }

                return companyLeaves.OrderBy(o => o.Leave.ApprovalStatus).ThenByDescending(o => o.Leave.RequestDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Leaves could not be retrieved.", ex);
            }
        }

        public async Task<List<Leave>> GetUserLeavesAsync(string userId)
        {
            return await _db.Leaves
                .Where(p => p.AppUserId == userId)
                .OrderByDescending(l => l.RequestDate)
                .ToListAsync();
        }

        public async Task<LeavesStatusDto> CancelStatus(string appUserId, int leaveId)
        {

            var leave = await _db.Leaves.FirstOrDefaultAsync(a => a.Id == leaveId && a.AppUserId == appUserId);

            if (leave.ApprovalStatus == ApprovalStatus.Pending)
            {
                leave.ApprovalStatus = ApprovalStatus.Cancelled;

                _db.Update(leave);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Invalid leaveId.");
            }

            return new LeavesStatusDto
            {
                LeaveStatus = ApprovalStatus.Cancelled.ToString()
            };
        }

        public async Task<LeavesStatusDto> UpdateStatusByCompanyManager(int leaveId, ApprovalStatus newStatus)
        {
            try
            {
                var leave = await _db.Leaves.FirstOrDefaultAsync(e => e.Id == leaveId);
                if (leave == null)
                {
                    throw new KeyNotFoundException("Leave not found for the user.");
                }

                leave.ApprovalStatus = newStatus;
                leave.ResponseDate = DateTime.Now;

                _db.Update(leave);
                await _db.SaveChangesAsync();

                return new LeavesStatusDto
                {
                    LeaveStatus = newStatus.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Leave status could not be updated.", ex);
            }
        }
    }
}
