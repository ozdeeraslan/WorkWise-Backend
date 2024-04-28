using ApplicationCore.DTOs.LeaveDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class LeaveValidateService : ILeaveValidateService
    {
        private readonly IAnnualLeaveService _annualLeaveService;
        private readonly WorkWiseContext _db;

        public LeaveValidateService(IAnnualLeaveService annualLeaveService, WorkWiseContext db)
        {
            _annualLeaveService = annualLeaveService;
            _db = db;
        }

        public async Task<AddLeaveDto> ValidateLeave(AddLeaveDto leave, string userId)
        {
            if (leave == null)
                throw new ArgumentNullException(nameof(leave));

            var newStartDate = leave.StartDate;
            var newEndDate = leave.EndDate;

            var overlappingLeaves = await _db.Leaves
                .Where(l => l.AppUserId == userId &&
                            ((l.EndDate > newStartDate && l.EndDate <= newEndDate) ||
                             (l.StartDate >= newStartDate && l.StartDate < newEndDate) ||
                             (l.StartDate <= newStartDate && l.EndDate >= newEndDate)) &&
                            (l.ApprovalStatus == ApprovalStatus.Pending || l.ApprovalStatus == ApprovalStatus.Approved))
                .ToListAsync();

            if (overlappingLeaves.Any())
            {
                throw new Exception("The requested leave dates conflict with a previously requested leave.");
            }

            var realLeaveDays = CalculateRealLeaveDays(leave.StartDate, leave.EndDate);

            switch (leave.LeaveType)
            {
                case LeaveType.Unpaid:
                    if (realLeaveDays > 3)
                    {
                        throw new ArgumentException("Unpaid leave cannot exceed 3 days.");
                    }
                    break;

                case LeaveType.Excuse:
                    if (realLeaveDays > 2)
                    {
                        throw new ArgumentException("Excuse leave cannot exceed 2 days.");
                    }
                    break;

                case LeaveType.Sick:

                    break;

                case LeaveType.Maternity:

                    break;

                case LeaveType.Marriage:
                    if (realLeaveDays > 3)
                    {
                        throw new ArgumentException("Marriage leave cannot exceed 3 days.");
                    }
                    break;

                case LeaveType.Bereavement:
                    if (realLeaveDays > 3)
                    {
                        throw new ArgumentException("Bereavement leave cannot exceed 3 days.");
                    }
                    break;

                case LeaveType.Annual:
                    var remainingAnnualLeaveDays = await _annualLeaveService.GetRemainingAnnualLeaveDays(userId);

                    if (realLeaveDays > remainingAnnualLeaveDays || realLeaveDays <= 0)
                    {
                         throw new ArgumentException("Invalid annual leave request. The requested leave days exceed the remaining annual leave days or are zero/negative.");
                    }
                    break;


                default:
                    break;
            }

            leave.Days = realLeaveDays;
         
            return leave;
        }

        public int CalculateRealLeaveDays(DateTime startDate, DateTime endDate)
        {
            int weekdays = 0;
            DateTime date = startDate;

            while (date < endDate)
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !IsOfficialHoliday(date))
                {
                    weekdays++;
                }
                date = date.AddDays(1);
            }

            return weekdays;
        }

        public bool IsOfficialHoliday(DateTime date)
        {
            List<DateTime> Holiday = new List<DateTime>()
            {
                new DateTime(DateTime.Now.Year, 1, 1),
                new DateTime(DateTime.Now.Year, 4, 23),
                new DateTime(DateTime.Now.Year, 5, 1),
                new DateTime(DateTime.Now.Year, 5, 19),
                new DateTime(DateTime.Now.Year, 7, 15),
                new DateTime(DateTime.Now.Year, 8, 30),
                new DateTime(DateTime.Now.Year, 10, 29)
            };

            if (Holiday.Any(x => x.Date == date.Date))
            {
                return true;
            }

            return false;

        }
    }
}
