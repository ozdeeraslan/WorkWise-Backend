using ApplicationCore.DTOs.CompanyDto;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IRepository<Company> _companyRepo;
        private readonly IPhotoService _photoService;

        public CompanyController(IRepository<Company> companyRepo, IPhotoService photoService)
        {
            _companyRepo = companyRepo;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyRepo.GetAllAsync();

            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCompanyDto companyDto)
        {
            if (companyDto == null)
            {
                return BadRequest();
            }

            string fileUrl = "";




            var company = new Company()
            {
                Name = companyDto.Name,
                Title = companyDto.Title,
                MersisNumber = companyDto.MersisNumber,
                TaxNumber = companyDto.TaxNumber,
                TaxAdministration = companyDto.TaxAdministration,
                PhoneNumber = companyDto.PhoneNumber,
                FoundingYear = companyDto.FoundingYear,
                ContractStartDate = companyDto.ContractStartDate,
                ContractEndDate = companyDto.ContractEndDate != null ? companyDto.ContractEndDate : null,
                Address = companyDto.Address,
                Email = companyDto.Email,
                NumberOfEmployees = companyDto.NumberOfEmployees,
                Status = companyDto.Status,
                
            };

            if (companyDto.Logo != null)
            {
                fileUrl = _photoService.UploadCompanyLogo(companyDto.Logo, companyDto.Name);


                company.Logo = fileUrl;
            }
            await _companyRepo.AddAsync(company);


            return Ok();
        }
    }
}
