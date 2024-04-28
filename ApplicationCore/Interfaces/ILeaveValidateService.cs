using ApplicationCore.DTOs.LeaveDto;

namespace WebAPI.Services
{
    public interface ILeaveValidateService
    {
        Task<AddLeaveDto> ValidateLeave(AddLeaveDto leave, string id);
    }
}