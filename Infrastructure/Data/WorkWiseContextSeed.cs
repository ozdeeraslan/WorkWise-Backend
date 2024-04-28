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
    public static class WorkWiseContextSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IRepository<Company> companyRepository)
        {
            var companies = await companyRepository.GetAllAsync();

            if(companies.Count == 0)
            {
                var company = new Company()
                {
                    Name = "WorkWise",
                    Title = "WorkWise A.Ş.",
                    MersisNumber = "1234567890123456",
                    TaxNumber = "9876543210",
                    TaxAdministration = "Taksim",
                    Logo = "https://siteownerphoto.blob.core.windows.net/companylogos/company_logo.png",
                    PhoneNumber = "1234567890",
                    FoundingYear = "2000",
                    ContractStartDate = DateTime.Now,
                    ContractEndDate = null,
                    Address = "Company Address",
                    Email = "info@workwise.com",
                    NumberOfEmployees = 100,
                    Status = Status.Active
                };

                await companyRepository.AddAsync(company);
            }

            

            if (!await roleManager.RoleExistsAsync("SiteOwner"))
            {
                await roleManager.CreateAsync(new IdentityRole("SiteOwner"));
            }

            if (!await userManager.Users.AnyAsync(u => u.UserName == "siteowner@example.com"))
            {
                var adminUser = new AppUser()
                {
                    UserName = "siteowner@example.com",
                    Email = "siteowner@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "123-456-7890",
                    CompanyId = 1,
                    PersonalDetail = new PersonalDetail()
                    {
                        FirstName = "John",
                        SecondName = "",
                        LastName = "Doe",
                        SecondLastName = "",
                        Address = "221 Baker St.",
                        BirthDate = new DateTime( 1992,3,17),
                        PlaceOfBirth = "PlaceOfBirth",
                        StartDate = new DateTime(2023, 8, 28),
                        EndDate = DateTime.Now,
                        TRIdentityNumber = "12345678901",
                        Status = Status.Active,
                        Profession = "Site Owner",
                        Department = "IT"
                    }
                };

                await userManager.CreateAsync(adminUser, "P@ssword1");
                await userManager.AddToRoleAsync(adminUser, "SiteOwner");
            }

            if (!await roleManager.RoleExistsAsync("CompanyManager"))
            {
                await roleManager.CreateAsync(new IdentityRole("CompanyManager"));
            }

            if (!await userManager.Users.AnyAsync(u => u.UserName == "companymanager@example.com"))
            {
                var companyManager = new AppUser()
                {
                    UserName = "companymanager@example.com",
                    Email = "companymanager@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "555-123-4567",
                    CompanyId = 1,
                    PersonalDetail = new PersonalDetail()
                    {
                        FirstName = "Emily",
                        SecondName = "Anne",
                        LastName = "Smith",
                        SecondLastName = "",
                        Address = "123 Elm Street",
                        BirthDate = new DateTime(1988, 9, 5),
                        PlaceOfBirth = "Springfield",
                        StartDate = new DateTime(2022, 5, 12),
                        EndDate = null,
                        TRIdentityNumber = "98765432109",
                        Status = Status.Active,
                        Profession = "Software Engineer",
                        Department = "Engineering"
                    }
                };

                await userManager.CreateAsync(companyManager, "P@ssword1");
                await userManager.AddToRoleAsync(companyManager, "CompanyManager");
            }

            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            if (!await userManager.Users.AnyAsync(u => u.UserName == "employee@example.com"))
            {
                var employee = new AppUser()
                {
                    UserName = "employee@example.com",
                    Email = "employee@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "555-987-6543",
                    CompanyId = 1,
                    PersonalDetail = new PersonalDetail()
                    {
                        FirstName = "Sarah",
                        SecondName = "Jane",
                        LastName = "Doe",
                        SecondLastName = "",
                        Address = "789 Pine Street",
                        BirthDate = new DateTime(1995, 7, 20),
                        PlaceOfBirth = "Oakland",
                        StartDate = new DateTime(2023, 2, 8),
                        EndDate = null,
                        TRIdentityNumber = "11223344556",
                        Status = Status.Active,
                        Profession = "Administrative Assistant",
                        Department = "Administration"
                    }
                };

                await userManager.CreateAsync(employee, "P@ssword1");
                await userManager.AddToRoleAsync(employee, "Employee");
            }
        }
    }
}
