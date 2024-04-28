using ApplicationCore.DTOs.UserDto;
using ApplicationCore.Entities;
using ApplicationCore.Enums;
using Infrastructure.Data;

namespace WebAPI.Extensions
{
    public static class MappingExtensions
    {
        public static AppUser ToAppUser(this AddUserDto userDto)
        {
            return new AppUser()
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                EmailConfirmed = false,
                PhoneNumber = userDto.PhoneNumber,
                CompanyId = userDto.CompanyId,
                PersonalDetail = new PersonalDetail()
                {
                    FirstName = userDto.FirstName,
                    SecondName = userDto.SecondName,
                    LastName = userDto.LastName,
                    SecondLastName = userDto.SecondLastName,
                    Address = userDto.Address,
                    BirthDate = userDto.BirthDate,
                    PlaceOfBirth = userDto.PlaceOfBirth,
                    StartDate = userDto.StartDate,
                    EndDate = userDto.EndDate,
                    TRIdentityNumber = userDto.TRIdentityNumber,
                    Status = userDto.Status,
                    Profession = userDto.Profession,
                    Department = userDto.Department,
                    Wage=userDto.Wage,
                    
                }
            };
        }
    }
}
