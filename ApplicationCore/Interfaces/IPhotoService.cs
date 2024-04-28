using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Interfaces
{
    public interface IPhotoService
    {
        public string UploadFile(IFormFile myFile, string id);
        public string GenerateFileName(string fileName, string EmployeeId);
        public string UploadCompanyLogo(IFormFile companyLogo, string companyName);
        public string GenerateLogoFileName(string fileName, string companyName);
    }
}