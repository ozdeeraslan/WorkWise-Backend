using ApplicationCore.DTOs.ExpenseDto;
using ApplicationCore.DTOs.LeaveDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ILeaveRepository
    {
        Task<Leave> AddLeaveAsync(AddLeaveDto leave, string userId);

        Task<LeavesStatusDto> CancelStatus(string appUserId, int expenseId);

        Task<List<Leave>> GetUserLeavesAsync(string userId);

        Task<LeavesStatusDto> UpdateStatusByCompanyManager(int expenseId, ApprovalStatus newStatus);

        Task<List<CompanyLeaveDto>> GetAllCompanyLeaves(int companyId);
    }
}
