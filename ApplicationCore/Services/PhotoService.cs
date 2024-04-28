using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ApplicationCore.Services
{
    public class PhotoService : IPhotoService
    {

        public string GenerateFileName(string fileName, string EmployeeId)
        {
            try
            {
                string strFileName = string.Empty;
                string[] strName = fileName.Split('.');
                strFileName = EmployeeId + DateTime.Now.ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
                return strFileName;
            }
            catch (Exception ex)
            {
                return fileName;
            }
        }

        public string GenerateLogoFileName(string fileName, string companyName)
        {
            try
            {
                string strFileName = string.Empty;
                string[] strName = fileName.Split('.');
                strFileName = companyName + DateTime.Now.ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
                return strFileName;
            }
            catch (Exception ex)
            {
                return fileName;
            }
        }

        public string UploadCompanyLogo(IFormFile companyLogo, string companyName)
        {
            var fileName = GenerateLogoFileName(companyLogo.FileName, companyName);
            var fileUrl = "";

            BlobContainerClient container = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=workwiseblobs;AccountKey=Q2ytkmrpUCLbTcq4peLpPLDGmxXPPVy32bL3NOCW8b1T+2X3sWXnnNIHrSpNb3qwOFZU/g1AISRd+ASt8Z6QCA==;EndpointSuffix=core.windows.net", "companylogos");
            try
            {
                BlobClient blobClient = container.GetBlobClient(fileName);
                using (Stream stream = companyLogo.OpenReadStream())
                {
                    blobClient.Upload(stream);
                }
                fileUrl = blobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex) { }
            var result = fileUrl;
            return result;
        }

        public string UploadFile(IFormFile? myFile, string id)
        {

            var filename = GenerateFileName(myFile.FileName, id);
            var fileUrl = "";

            BlobContainerClient container = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=workwiseblobs;AccountKey=Q2ytkmrpUCLbTcq4peLpPLDGmxXPPVy32bL3NOCW8b1T+2X3sWXnnNIHrSpNb3qwOFZU/g1AISRd+ASt8Z6QCA==;EndpointSuffix=core.windows.net", "images");
            try
            {
                BlobClient blobClient = container.GetBlobClient(filename);
                using (Stream stream = myFile.OpenReadStream())
                {
                    blobClient.Upload(stream);
                }
                fileUrl = blobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex) { }
            var result = fileUrl;
            return result;
        }
    }
}

