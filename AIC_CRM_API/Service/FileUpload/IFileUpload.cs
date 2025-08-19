using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Service.FileUpload
{
    public interface IFileUpload
    {
        void fileUpload(IFormFile file, string tenantId, string folderName, string fileName, IWebHostEnvironment hostingEnvironment);
    }
}
