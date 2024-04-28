using ApplicationCore.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class FileService : IFileService
    {
        public string GenerateFileName(string fileName)
        {
            
                try
                {
                    string strFileName = string.Empty;
                    string[] strName = fileName.Split('.');
                    strFileName =  DateTime.Now.ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
                    return strFileName;
                }
                catch (Exception ex)
                {
                    return fileName;
                }
            
        }

        public string UploadFile(IFormFile myFile)
        {
            var filename = GenerateFileName(myFile.FileName);
            var fileUrl = "";

            BlobContainerClient container = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=workwiseblobs;AccountKey=Q2ytkmrpUCLbTcq4peLpPLDGmxXPPVy32bL3NOCW8b1T+2X3sWXnnNIHrSpNb3qwOFZU/g1AISRd+ASt8Z6QCA==;EndpointSuffix=core.windows.net", "files");
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
