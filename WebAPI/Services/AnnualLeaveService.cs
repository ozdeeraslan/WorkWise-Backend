using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class AnnualLeaveService : IAnnualLeaveService
    {
        private readonly WorkWiseContext _db;

        public AnnualLeaveService(WorkWiseContext db)
        {
            _db = db;
        }



        public async Task<int> GetRemainingAnnualLeaveDays(string userId)
        {
            var remainingAnnualLeaveDays = TotalAnnualLeaveDays(userId) - await GetUsedAnnualLeaveDaysLastYear(userId);

            return remainingAnnualLeaveDays;
        }

        public async Task<int> GetUsedAnnualLeaveDaysLastYear(string userId)
        {
            var lastYearStartDate = DateTime.Now.AddYears(-1).Date;
            var lastYearEndDate = DateTime.Now.Date;

            // Son bir yılda yapılan yıllık izin 
            var usedAnnualLeaveDaysLastYear = await _db.Leaves
                .Where(history => history.AppUserId == userId
                    && history.LeaveType == LeaveType.Annual
                    && (history.ApprovalStatus == ApprovalStatus.Approved || history.ApprovalStatus == ApprovalStatus.Pending)
                    && history.StartDate >= lastYearStartDate)
                .SumAsync(history => history.Days);

            return usedAnnualLeaveDaysLastYear;
        }

        public int TotalAnnualLeaveDays(string userId)
        {
            var user = _db.Users.Include(u => u.PersonalDetail).FirstOrDefault(u => u.Id == userId);

            if (user != null && user.PersonalDetail != null && user.PersonalDetail.StartDate != null)
            {
                var employmentStartDate = user.PersonalDetail.StartDate.Value;
                var employmentDurationYears = CalculateEmploymentDurationYears(employmentStartDate);

                var annualLeaveDaysPerYear = 6; // Her yıl için varsayılan izin gün sayısı
                var annualLeaveDays = employmentDurationYears * annualLeaveDaysPerYear;

                if (annualLeaveDays > 30)
                {
                    annualLeaveDays = 30;
                }

                return annualLeaveDays;
            }

            return 0;
        }

        private int CalculateEmploymentDurationYears(DateTime employmentStartDate)
        {
            var currentDate = DateTime.Now;
            var employmentDurationYears = currentDate.Year - employmentStartDate.Year;

            if (currentDate < employmentStartDate.AddYears(employmentDurationYears))
            {
                employmentDurationYears--;
            }

            return employmentDurationYears;
        }
    }
}
