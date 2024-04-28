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
    public interface IAdvanceService
    {
        Task<List<AddAdvancePaymentDto>> GetAllUserAdvances(string appUserId);

        Task<AdvancePayment> CreateAdvance(string appUserId, AddAdvancePaymentDto addAdvancePaymentDto);

        Task<List<CompanyAdvanceDto>> GetAllCompanyAdvances(int companyId);

        Task<AdvanceStatusDto> CancelStatus(string appUserId, int advanceId);

        Task<AdvanceStatusDto> UpdateStatusByCompanyManager(int advanceId, ApprovalStatus newStatus);
    }
}
