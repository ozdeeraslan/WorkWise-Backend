using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface  IFileService
    {
        public string UploadFile(IFormFile myFile);
        public string GenerateFileName(string fileName);
    }
}
